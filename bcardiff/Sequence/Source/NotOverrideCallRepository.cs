using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Moq
{
	class NotOverrideCallRepository : IProxyCallRepository
	{
		IList<IProxyCall> calls = new List<IProxyCall>();

		public void Add(IProxyCall call, ExpectKind kind)
		{
			calls.Add(call);
		}

		public IEnumerator<IProxyCall> GetEnumerator()
		{
			return calls.GetEnumerator();
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return calls.GetEnumerator();
		}
	}
}
