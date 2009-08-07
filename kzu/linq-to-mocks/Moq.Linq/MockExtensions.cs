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
	}
}
