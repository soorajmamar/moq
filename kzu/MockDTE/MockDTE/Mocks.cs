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

namespace MockDTE
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
				var replace = new MockSetupsReplacer().SetupMocks(expression);

				throw new NotImplementedException();
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
			static readonly MethodInfo MockMethod = typeof(MockExtensions).GetMethod("Mock");
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
				if (whereCalls.Count != 0 && b.NodeType == ExpressionType.Equal && b.Left.NodeType == ExpressionType.MemberAccess)
				{
					var memberAccess = (MemberExpression)b.Left;
		
					var sourceType = memberAccess.Expression.Type;
					var returnType = memberAccess.Type;

					var mockType = MockType.MakeGenericType(sourceType);
					var actionType = typeof(Action<>).MakeGenericType(mockType);
					var funcType = typeof(Func<,>).MakeGenericType(sourceType, returnType);

					// dte.Solution == solution
					// becomes:
					// Mocks.Mock(dte, mock => mock.SetupGet(x => x.Solution).Returns(solution))
					var xParam = Expression.Parameter(sourceType, "x");
					var mockParam = Expression.Parameter(mockType, "mock");
					var setupGet = mockType.GetMethod("SetupGet");

					// Mocks.Mock(
					var mock = Expression.Call(null, MockMethod.MakeGenericMethod(sourceType), 
						// dte
					    memberAccess.Expression, 
						// mock => 
						Expression.Lambda(actionType, 
							// mock.SetupGet(
							Expression.Call(mockParam, setupGet, 
								// x => 
								Expression.Lambda(funcType, 
									// x.Solution
									Expression.MakeMemberAccess(xParam, memberAccess.Member)))));
								
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
	}
}
