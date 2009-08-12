using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Linq.Expressions;
using System.ComponentModel;

namespace Moq.Linq
{
	public static class MockExtensions
	{
		[EditorBrowsable(EditorBrowsableState.Never)]
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

		//[EditorBrowsable(EditorBrowsableState.Never)]
		//public static Mock<TResult> FluentMock<T, TResult>(T mocked, Expression<Func<T, TResult>> setup)
		//    where T : class
		//    where TResult : class
		//{
		//    return FluentMock<T, TResult>(Mock.Get(mocked), setup);
		//}

		//public static T With<T>(this T @object, Action<T> initialization)
		//{
		//    initialization(@object);
		//    return @object;
		//}
	}
}
