using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Moq;
using EnvDTE;
using Moq.Linq;
using System.Linq.Expressions;
using IQToolkit;

namespace MockDTE
{
	class Program
	{
		static void Query(Expression<Func<IEnumerable<int>>> expression)
		{
			new TestVisitor().Do(expression);
			Console.WriteLine(expression.ToString());
		}

		class TestVisitor : ExpressionVisitor
		{
			public void Do(System.Linq.Expressions.Expression expression)
			{
				this.Visit(expression);
			}

			protected override System.Linq.Expressions.Expression VisitMethodCall(MethodCallExpression m)
			{
				return base.VisitMethodCall(m);
			}
		}

		static IEnumerable<T> Create<T>()
		{
			return Enumerable.Empty<T>();
		}

		static void Main(string[] args)
		{
			//Query(() =>
			//    Create<string>().SelectMany(((Func<string, IEnumerable<int>>) (strings => Create<int>())), (strings, ints) => new { strings = strings, ints = ints }).Where(((Func<<>f__AnonymousType0<string, int>, bool>) (<>h__TransparentIdentifier0 => (<>h__TransparentIdentifier0.strings.Length > 0)))).Select((Func<<>f__AnonymousType0<string, int>, int>) (<>h__TransparentIdentifier0 => <>h__TransparentIdentifier0.ints));
			//);

			Query(() => from strings in Create<string>()
						from ints in Create<int>()
						where strings.Length > 0
						select ints);

			//var linqDte2 = (from dte in Mocks.CreateReal<DTE>()
			//                from project in Mocks.CreateReal<Project>()
			//                where 
			//                     dte.Solution.Projects.GetEnumerator() == new[] { project }.GetEnumerator() &&
			//                     project.Properties.Item("SomePropertyName").Value == "Banana"
			//                select dte)
			//                .First();


			//var mockDte2 = (from dte in Mocks.CreateReal<DTE>()
			//                from project in Mocks.CreateReal<Project>()
			//                where
			//                   Mocks.Mock(dte, 
			//                    mock => mock.SetupGet(x => x.Solution)
			//                        .Returns( )) &&
			//                 Mocks.Mock(project, mock => mock.SetupGet(x => x.Properties).Returns(properties)) &&
			//                 Mocks.Mock(properties, mock => mock.Setup(x => x.Item("SomePropertyName")).Returns(property)) &&
			//                 Mocks.Mock(property, mock => mock.SetupProperty(x => x.Value, "Banana")) &&
			//                 Mocks.Mock(projects, mock => mock.Setup(x => x.GetEnumerator()).Returns(new[] { project }.GetEnumerator())) &&
			//                 Mocks.Mock(solution, mock => mock.SetupGet(x => x.Projects).Returns(projects))
			//                select dte)
			//               .First();

			var mockDte3 = (from dte in Mocks.CreateReal<DTE>()
							from solution in Mocks.CreateReal<Solution>()
							from projects in Mocks.CreateReal<Projects>()
							from project in Mocks.CreateReal<Project>()
							from property in Mocks.CreateReal<Property>()
							from properties in Mocks.CreateReal<Properties>()
							where
							   Mocks.Mock(dte, mock => mock.SetupGet(x => x.Solution).Returns(solution)) &&
							 Mocks.Mock(project, mock => mock.SetupGet(x => x.Properties).Returns(properties)) &&
							 Mocks.Mock(properties, mock => mock.Setup(x => x.Item("SomePropertyName")).Returns(property)) &&
							 Mocks.Mock(property, mock => mock.SetupProperty(x => x.Value, "Banana")) &&
							 Mocks.Mock(projects, mock => mock.Setup(x => x.GetEnumerator()).Returns(new[] { project }.GetEnumerator())) &&
							 Mocks.Mock(solution, mock => mock.SetupGet(x => x.Projects).Returns(projects))
							select dte)
						   .First();

			var mockDte = (from dte in Mocks.Create<DTE>()
						   from solution in Mocks.Create<Solution>()
						   from projects in Mocks.Create<Projects>()
						   from project in Mocks.Create<Project>()
						   from property in Mocks.Create<Property>()
						   from properties in Mocks.Create<Properties>()
						   where
							dte.Solution == solution &&
							project.Properties == properties &&
							properties.Item("SomePropertyName") == property &&
							property.Value == "Banana" &&
							projects.GetEnumerator() == new[] { project }.GetEnumerator() &&
							solution.Projects == projects
						   select dte)
						   .First();



			foreach (EnvDTE.Project project in mockDte.Solution.Projects)
			{
				Console.WriteLine(project.Properties.Item("SomePropertyName").Value);
			}

			Console.ReadLine();

			//var mockProperty = new Mock<EnvDTE.Property>();
			//mockProperty.Setup(property => property.Value).Returns("Banana");

			//var mockProject = new Mock<EnvDTE.Project>();
			//// Maybe unknowingly, you were already using a recursive mock here, for Properties...
			//mockProject.Setup(project => project.Properties.Item("SomePropertyName")).Returns(mockProperty.Object);
			//mockProject.SetupGet(project => project.Kind).Returns(VSLangProj.PrjKind.prjKindCSharpProject);

			//var mockProjects = new Mock<EnvDTE.Projects>();
			//// Can return an array with the single project enumerator to avoid mocking the enumerator.
			//mockProjects.Setup(projects => projects.GetEnumerator()).Returns(new[] { mockProject.Object }.GetEnumerator());

			//Mock<EnvDTE.DTE> mockDte = new Mock<EnvDTE.DTE>();
			// Can leverage recursive mocks to avoid explicit mocking of Solution.
			//mockDte
			//    .SetupGet(dte => dte.Solution.Projects)
			//    .Returns
			//    (
			//        new Mock<EnvDTE.Projects>().With(projects =>
			//        {

			//        }).Object
			//    );

			//foreach (EnvDTE.Project project in mockDte.Object.Solution.Projects)
			//{
			//    Console.WriteLine(project.Properties.Item("SomePropertyName").Value);
			//}

		}
	}

	//public static class WithExtension
	//{
	//    public static FluentMock<TSource, TParent> With<T>(this T source, Action<T> initializer)
	//    {
	//        initializer(source);
	//        return source;
	//    }

	//    public static T With<T, S>(this T source, Func<T, S> initializer)
	//    {
	//        initializer(source);
	//        return source;
	//    }
	//}

	//public class FluentMock<TThis, TParent>
	//{
	//    public FluentMock(TThis @this, TParent parent)
	//    {
	//        this.This = @this;
	//        this.Parent = parent;
	//    }

	//    public FluentMock<TThis> With(Action<TThis> initializer)
	//    {
	//        initializer(This);
	//        return this;
	//    }

	//    public TParent Parent { get; private set; }
	//    public TThis This { get; private set; }
	//}
}