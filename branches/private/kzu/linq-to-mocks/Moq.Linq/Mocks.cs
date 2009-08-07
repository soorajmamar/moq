using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using Moq;
using IQToolkit;
using System.Linq.Expressions;
using System.Reflection;
using Moq.Language.Flow;
using Moq.Language;
using System.Collections;

namespace Moq.Linq
{
	public static class Mocks
	{
		public static IQueryable<T> Create<T>()
		{
			return new MockQueryable<T>();
		}

		class MockQueryable<T> : Query<T>
		{
			public MockQueryable()
				: base(new MockQueryProvider())
			{
			}
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		public static IEnumerable<T> CreateReal<T>()
			where T : class
		{
			while (true)
			{
				yield return new Mock<T>().Object;
			}
		}

		public static bool Mock<T>(T source, Action<Mock<T>> setup)
			where T : class
		{
			setup(Moq.Mock.Get(source));
			return true;
		}

		class MockQueryProvider : QueryProvider
		{
			static readonly MethodInfo createMockMethod = typeof(Mocks).GetMethod("Create");
			static readonly MethodInfo createRealMockMethod = typeof(Mocks).GetMethod("CreateReal");

			public override object Execute(Expression expression)
			{
				var collector = new ExpressionCollector();
				var createCalls = collector
					.Collect(expression)
					.OfType<MethodCallExpression>()
					.Where(call => call.Method.IsGenericMethod &&
						call.Method.GetGenericMethodDefinition() == createMockMethod)
					.ToArray();
				var replaceWith = createCalls
					.Select(call => Expression.Call(
						call.Object,
						createRealMockMethod.MakeGenericMethod(
							call.Method.GetGenericArguments()),
						call.Arguments.ToArray()))
					.ToArray();

				var replaced = ExpressionReplacer.ReplaceAll(expression, createCalls, replaceWith);
				replaced = new MockSetupsReplacer().SetupMocks(replaced);
				replaced = new QueryableToEnumerableReplacer().ReplaceAll(replaced);

				var lambda = Expression.Lambda(typeof(Func<>).MakeGenericType(expression.Type), replaced);
				return lambda.Compile().DynamicInvoke(null);
			}

			public override string GetQueryText(Expression expression)
			{
				throw new NotImplementedException();
			}
		}

		class ExpressionCollector : ExpressionVisitor
		{
			List<Expression> expressions = new List<Expression>();

			public IEnumerable<Expression> Collect(Expression exp)
			{
				Visit(exp);
				return expressions;
			}

			protected override Expression Visit(Expression exp)
			{
				expressions.Add(exp);
				return base.Visit(exp);
			}
		}

		class MockSetupsReplacer : ExpressionVisitor
		{
			static readonly MethodInfo MockMethod = typeof(Mocks).GetMethod("Mock");
			static readonly Type MockType = typeof(Mock<>);

			Stack<MethodCallExpression> whereCalls = new Stack<MethodCallExpression>();

			public Expression SetupMocks(Expression expression)
			{
				return Visit(expression);
			}

			protected override Expression VisitMethodCall(MethodCallExpression m)
			{
				// We only translate Where for now.
				if (m.Method.DeclaringType == typeof(Queryable) && m.Method.Name == "Where")
				{
					whereCalls.Push(m);
					var result = base.VisitMethodCall(m);
					whereCalls.Pop();

					return result;
				}
				else
				{
					return base.VisitMethodCall(m);
				}
			}

			protected override Expression VisitBinary(BinaryExpression b)
			{
				if (whereCalls.Count != 0 && b.NodeType == ExpressionType.Equal && 
					(b.Left.NodeType == ExpressionType.MemberAccess || b.Left.NodeType == ExpressionType.Call))
				{
					var isMember = b.Left.NodeType == ExpressionType.MemberAccess;
					var methodCall = b.Left as MethodCallExpression;
					var memberAccess = isMember ? b.Left as MemberExpression : (MemberExpression)methodCall.Object;

					var targetObject = memberAccess.Expression;
					var sourceType = memberAccess.Expression.Type;
					var returnType = isMember ? memberAccess.Type : methodCall.Method.ReturnType;

					var mockType = MockType.MakeGenericType(sourceType);
					var actionType = typeof(Action<>).MakeGenericType(mockType);
					var funcType = typeof(Func<,>).MakeGenericType(sourceType, returnType);

					// dte.Solution == solution
					// becomes:
					// Mocks.Mock(dte, mock => mock.SetupGet(x => x.Solution).Returns(solution))
					var xParam = Expression.Parameter(sourceType, "x");
					var mockParam = Expression.Parameter(mockType, "mock");
					//var setupGetMethod = mockType.GetMethod("SetupGet").MakeGenericMethod(returnType);
					var setupMethod = mockType.GetMethods().Where(mi => mi.Name == "Setup" && mi.IsGenericMethod).First().MakeGenericMethod(returnType);
					var returnsMethod = typeof(IReturns<,>)
						.MakeGenericType(sourceType, returnType)
						.GetMethod("Returns", new [] { returnType });

					// Replace: dte.Solution => x.Solution
					var mockLambda = new MemberAccessReplacer().Replace(xParam, b.Left);

					//var setup = isMember ? setupGetMethod : setupMethod;

					// mock.Setup[Get](
					var setupExpr = Expression.Call(mockParam, setupMethod,
						// x => x.Solution
						Expression.Lambda(funcType, mockLambda, xParam)
					);
					// .Returns(
					var returnsExpr = Expression.Call(setupExpr, returnsMethod, b.Right);

					// Mocks.Mock(
					var mock = Expression.Call(null, MockMethod.MakeGenericMethod(sourceType),
						// dte
						targetObject,
						// mock => mock.SetupGet(..).Returns(..)
						Expression.Lambda(actionType, returnsExpr, mockParam)
					);

					return mock;
				}

				return base.VisitBinary(b);
			}

			private static Expression StripQuotes(Expression e)
			{
				while (e.NodeType == ExpressionType.Quote)
				{
					e = ((UnaryExpression)e).Operand;
				}
				return e;
			}
		}

		class MemberAccessReplacer : ExpressionVisitor
		{
			Expression newTarget;
			Expression toReplace;

			public Expression Replace(Expression newTarget, Expression expression)
			{
				var expressions = new ExpressionCollector().Collect(expression).OfType<MemberExpression>();

				this.toReplace = expressions.First();
				this.newTarget = newTarget;

				return this.Visit(expression);
			}

			protected override Expression VisitMemberAccess(MemberExpression m)
			{
				if (m == toReplace)
				{
					return Expression.MakeMemberAccess(newTarget, m.Member);

				}
				return base.VisitMemberAccess(m);
			}

		}

		class QueryableToEnumerableReplacer : ExpressionVisitor
		{
			public Expression ReplaceAll(Expression expression)
			{
				return this.Visit(expression);
			}

			protected override Expression VisitMethodCall(MethodCallExpression m)
			{
				if (m.Method.DeclaringType == typeof(Queryable))
				{
					var queryMethod = m.Method.GetGenericMethodDefinition();
					var enumerableMethod = typeof(Enumerable).GetMethods()
						.Where(mi => mi.IsGenericMethod == queryMethod.IsGenericMethod &&
							mi.Name == queryMethod.Name &&
							mi.GetGenericArguments().Length == queryMethod.GetGenericArguments().Length &&
							// Yes, this is not precise either :)
							mi.GetParameters().Length == queryMethod.GetParameters().Length).First();

					enumerableMethod = enumerableMethod.MakeGenericMethod(m.Method.GetGenericArguments());

					return Expression.Call(m.Object, enumerableMethod, m.Arguments.ToArray());
				}

				return base.VisitMethodCall(m);
			}

			private bool AreEqual<T>(IEnumerable<T> first, IEnumerable<T> second)
			{
				return AreEqual(first, second, (obj1, obj2) => Object.Equals(obj1, obj2));
			}

			private bool AreEqual<T>(IEnumerable<T> first, IEnumerable<T> second, Func<T, T, bool> equalityComparer)
			{
				var firstEnum = first.GetEnumerator();
				var secondEnum = second.GetEnumerator();
				
				while (firstEnum.MoveNext() == secondEnum.MoveNext() == true)
				{
					if (!equalityComparer(firstEnum.Current, secondEnum.Current))
						return false;
				}

				// Yes, this is not 100% precise. That's why it's a POC
				return true;
			}
		}
	}
}
