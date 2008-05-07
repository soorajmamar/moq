using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Moq
{
	internal class DefaultProxyCallRepository : IProxyCallRepository
	{
		Dictionary<string, IProxyCall> calls = new Dictionary<string, IProxyCall>();

		public void Add(IProxyCall call, ExpectKind kind)
		{
			if (kind == ExpectKind.PropertySet)
				calls["set::" + call.ExpectExpression.ToStringFixed()] = call;
			else
				calls[call.ExpectExpression.ToStringFixed()] = call;
		}

		public IEnumerator<IProxyCall> GetEnumerator()
		{
			return calls.Values.GetEnumerator();
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return calls.Values.GetEnumerator();
		}
	}
}
