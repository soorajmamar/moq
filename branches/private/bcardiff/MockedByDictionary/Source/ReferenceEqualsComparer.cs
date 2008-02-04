using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Moq
{
	internal class ReferenceEqualsComparer : IEqualityComparer<object>
	{
		bool IEqualityComparer<object>.Equals(object x, object y)
		{
			return object.ReferenceEquals(x, y);
		}

		int IEqualityComparer<object>.GetHashCode(object obj)
		{
			return obj.GetHashCode();
		}
	}
}
