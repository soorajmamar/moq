using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Moq.Linq
{
	public static class MockExtensions
	{
		public static Mock<T> AsMock<T>(this T @object)
			where T : class
		{
			return Mock.Get(@object);
		}

		public static T With<T>(this T @object, Action<T> initialization)
		{
			initialization(@object);
			return @object;
		}
	}
}
