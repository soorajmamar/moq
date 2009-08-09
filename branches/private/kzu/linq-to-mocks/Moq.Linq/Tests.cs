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
	public class Tests
	{
		public void TranslateToFluentMocks()
		{
			var expression = ToExpression<IFoo, int>(f => f.Bar.Baz("hey").Value);

			//// For linq querying we need something different.
			//factory
			//    .Create<IBar>()
			//    .Setup(b => b.Baz("hey"))
			//    .Returns(
			//        factory.Create<IBaz>()
			//        .Setup(z => z.Value)
			//        .Returns(5)
			//        .AsMocked()
			//    )
			//    .AsMocked();

			//var value = foo.Bar.Baz("hey").Value;

			//Debug.Assert(value == 5);
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
