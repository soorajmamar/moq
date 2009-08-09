using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Moq.Linq
{
	public static class MockExtensions
	{

		public static T With<T>(this T @object, Action<T> initialization)
		{
			initialization(@object);
			return @object;
		}
	}
}
