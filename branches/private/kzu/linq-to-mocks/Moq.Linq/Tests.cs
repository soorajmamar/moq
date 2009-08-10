using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using Moq.Language;
using System.Reflection;
using Moq.Language.Flow;
using System.Diagnostics;

namespace Moq.Linq
{
	public static class FluentMocks
	{
		public static Mock<TResult> FluentMock<T, TResult>(this Mock<T> mock, Expression<Func<T, TResult>> setup)
			where T : class
			where TResult : class
		{
			MethodInfo info;

			if (setup.Body.NodeType == ExpressionType.MemberAccess)
			{
				var property = ((MemberExpression)setup.Body).Member as PropertyInfo;
				if (property == null)
					throw new NotSupportedException("Fields are not supported");

				info = property.GetGetMethod();
			}
			else if (setup.Body.NodeType == ExpressionType.Call)
			{
				info = ((MethodCallExpression)setup.Body).Method;
			}
			else
			{
				throw new NotSupportedException("Unsupported expression: " + setup.ToString());
			}

			if (!info.ReturnType.IsMockeable())
				// We should have a type.ThrowIfNotMockeable() rather, so that we can reuse it.
				throw new NotSupportedException();

			Mock fluentMock;
			if (!mock.InnerMocks.TryGetValue(info, out fluentMock))
			{
				fluentMock = ((IMocked)new MockDefaultValueProvider(mock).ProvideDefault(info)).Mock;
			}

			var result = (TResult)fluentMock.Object;

			mock.Setup(setup).Returns(result);

			return (Mock<TResult>)fluentMock;
		}
	}

	public class Tests
	{
		public void TranslateToFluentMocks()
		{
			var expression = ToExpression<IFoo, int>(f => f.Bar.Baz("hey").Value);

			// For linq querying we need something different.
			var foo = new Mock<IFoo>(MockBehavior.Strict).Object;
			// We construct a factory 
			var factory = new MockFactory(MockBehavior.Strict);
			var bar = factory
				.Create<IBar>()
				.Setup(b => b.Baz("hey"))
				.Returns(
					factory.Create<IBaz>()
					.Setup(z => z.Value)
					.Returns(5)
					.AsMocked()
				)
				.AsMocked();

			foo.AsMock().Setup(f => f.Bar).Returns(bar);

			var value = foo.Bar.Baz("hey").Value;

			//var instance = (from f in CreateMany<IFoo>()
			//                where f.Bar.Baz("hey").Value == 5
			//                select f)
			//                .First();

			var instance2 = (from f in CreateMany<IFoo>()
							where
								Mock.Get(f)
									.FluentMock(f1 => f1.Bar)
									.FluentMock(b1 => b1.Baz("hey"))
									.Setup(b2 => b2.Value).Returns(5) != null
							select f)
							.First();

			value = instance2.Bar.Baz("hey").Value;

			Debug.Assert(value == 5);
		}

		private IEnumerable<T> CreateMany<T>()
			where T : class
		{
			while (true)
			{
				yield return new Mock<T>().Object;
			}
		}

		private LambdaExpression ToExpression<T1, T2>(Expression<Func<T1, T2>> expression)
		{
			return expression;
		}

		public interface IFoo
		{
			IBar Bar { get; set; }
		}

		public interface IBar
		{
			IBaz Baz(string value);
		}

		public interface IBaz
		{
			int Value { get; set; }
		}
	}
}
