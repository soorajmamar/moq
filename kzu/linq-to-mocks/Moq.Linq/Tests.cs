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
		public void FluentMockMethod()
		{
			//FluentMock<T, TResult>(this Mock<T> mock, Expression<Func<T, TResult>> setup)
			var method = ((Func<Mock<string>, Expression<Func<string, string>>, Mock<string>>)
				MockExtensions.FluentMock<string, string>).Method.GetGenericMethodDefinition();

			Debug.Assert(method == typeof(MockExtensions).GetMethod("FluentMock"));

			//var prop = ((Func<IBar>)((IFoo)null).Bar).Method;

			Action<string> writeLine = Console.WriteLine;
			MethodInfo writeLineMethod = writeLine.Method;

			Func<object> cloneable = ((ICloneable)null).Clone;
			MethodInfo cloneMethod = cloneable.Method;
			

			//var method = Reflector.Reflect<MockExtensions>.GetMethod(x => MockExtensions.FluentMock);
			Func<string> createMethod = ((IFactory)null).Create<string>;
			MethodInfo genericCreate = createMethod.Method.GetGenericMethodDefinition();

		}

		public interface IFactory
		{
			T Create<T>();
		}

		public void GenerateDynamicVisitorCode()
		{
			var visitMethods = typeof(ExpressionVisitor).GetMethods(BindingFlags.NonPublic | BindingFlags.Instance)
				.Where(mi => mi.Name != "Visit" && mi.Name.StartsWith("Visit"))
				.OrderBy(mi => mi.Name);

			var properties = visitMethods.Select(mi => 
				"public Func<" + mi.GetParameters()[0].ParameterType.Name + ", " + mi.ReturnType.Name + "> " + mi.Name + "Handler { get; set; }");

			Console.WriteLine(String.Join(Environment.NewLine, properties.ToArray()));
			Console.WriteLine();
			Console.WriteLine();

			var template =
@"		protected override {0} {1}({2} expression)
		{{
			if ({3} != null)
				return {3}(expression);
			else
				return base.{1}(expression);
		}}
";

			var overrides = visitMethods.Select(mi =>
				String.Format(template, mi.ReturnType.Name, mi.Name, mi.GetParameters()[0].ParameterType.Name, mi.Name + "Handler"));

			Console.WriteLine(String.Join(Environment.NewLine, overrides.ToArray()));
		}

		public void RendersIndexersAsMethodInvocations()
		{
			var instance = (from f in Mocks.Query<IFoo>()
							where f.Bar.Baz(It.IsAny<string>())[It.IsAny<string>(), true] == 5
							select f)
							.First();

			Debug.Assert(instance.Bar.Baz("foo")["bar", true] == 5);
		}

		public void ShouldSupportMultipleSetups()
		{
			var instance = (from f in Mocks.Query<IFoo>()
							where 
								f.Name == "Foo" && 
								f.Bar.Id == "25" && 
								f.Bar.Ping(It.IsAny<string>()) == "ack" &&
								f.Bar.Ping("error") == "error" &&
								f.Bar.Baz(It.IsAny<string>()).Value == 5
							select f)
							.First();

			Debug.Assert(instance.Name == "Foo");
			Debug.Assert(instance.Bar.Id == "25");
			Debug.Assert(instance.Bar.Ping("blah") == "ack");
			Debug.Assert(instance.Bar.Ping("error") == "error");
			Debug.Assert(instance.Bar.Baz("foo").Value == 5);
		}

		public void ShouldSupportItIsAny()
		{
			var instance = (from f in Mocks.Query<IFoo>()
							where f.Bar.Baz(It.IsAny<string>()).Value == 5
							select f)
							.First();

			Debug.Assert(instance.Bar.Baz("foo").Value == 5);
			Debug.Assert(instance.Bar.Baz("bar").Value == 5);
		}

		public void TranslateToFluentMocks()
		{
			var expression = ToExpression<IFoo, int>(f => f.Bar.Baz("hey").Value);

			// For linq querying we need something different.
			//var foo = new Mock<IFoo>(MockBehavior.Strict).Object;
			//var value = foo.Bar.Baz("hey").Value;

			var where = ToExpression<IFoo>(f => Mock.Get(f)
									.FluentMock(f1 => f1.Bar)
									.FluentMock(b1 => b1.Baz("hey"))
									.Setup(b2 => b2.Value).Returns(5) != null);

			var instance = (from f in Mocks.Query<IFoo>()
							where f.Bar.Baz("hey").Value == 5
							select f)
							.First();

			// f.Bar.Baz("hey").Value 
			// f						 => Mock.Get(f)
			// [f].Bar					 => .FluentMock(mock => mock.Bar)
			// [f.Bar].Baz("hey")		 => .FluentMock(mock => mock.Baz("hey")

			// [f.Bar.Baz("hey")].Value  => .Setup(mock => mock.Value).Returns(..) != null

			Debug.Assert(instance.Bar.Baz("hey").Value == 5);


			 var instance2 = (from f in CreateMany<IFoo>()
							where
								Mock.Get(f)
									.FluentMock(f1 => f1.Bar)
									.FluentMock(b1 => b1.Baz("hey"))
									.Setup(b2 => b2.Value).Returns(5) != null
							select f)
							.First();

			 Debug.Assert(instance2.Bar.Baz("hey").Value == 5);
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

		private LambdaExpression ToExpression<T>(Expression<Func<T, bool>> filter)
		{
			return filter;
		}

		public interface IFoo
		{
			IBar Bar { get; set; }
			string Name { get; set; }
		}

		public interface IBar
		{
			IBaz Baz(string value);
			string Id { get; set; }
			string Ping(string command);
		}

		public interface IBaz
		{
			int Value { get; set; }
			int this[string key1, bool key2] { get; set; }
		}
	}
}
