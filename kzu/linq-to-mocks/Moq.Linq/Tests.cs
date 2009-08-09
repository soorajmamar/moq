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
	public static class AsMockExtensions
	{
		public static TMock AsMocked<TMock>(this IReturnsResult<TMock> returns)
			where TMock : class
		{
			return returns.AsMock().Object;
		}

		public static Mock<TMock> AsMock<TMock>(this IReturnsResult<TMock> returns)
			where TMock : class
		{
			// Make this properly supported.
			return (Mock<TMock>)returns
				.GetType()
				.GetField("mock", BindingFlags.Instance | BindingFlags.NonPublic)
				.GetValue(returns);
		}

		public static Mock<T> AsMock<T>(this T @object)
			where T : class
		{
			return Mock.Get(@object);
		}
	}

	public class Tests
	{
		public void TranslateToFluentMocks()
		{
			var fooMock = new Mock<IFoo>(MockBehavior.Strict);
			fooMock.Setup(f => f.Bar.Baz("hey").Value).Returns(5);

			var expression = ToExpression<IFoo, int>(f => f.Bar.Baz("hey").Value);

			var factory = new MockFactory(MockBehavior.Strict);
			var baz = factory
				.Create<IBaz>()
				.Setup(z => z.Value)
				.Returns(5)
				.AsMocked();

			var bar = factory
				.Create<IBar>()
				.Setup(b => b.Baz("hey"))
				.Returns(baz)
				.AsMocked();

			fooMock.Setup(f => f.Bar).Returns(bar);

			// For linq querying we need something different.
			factory
				.Create<IBar>()
				.Setup(b => b.Baz("hey"))
				.Returns(
					factory.Create<IBaz>()
					.Setup(z => z.Value)
					.Returns(5)
					.AsMocked()
				)
				.AsMocked();

			var value = foo.Bar.Baz("hey").Value;

			Debug.Assert(value == 5);
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
