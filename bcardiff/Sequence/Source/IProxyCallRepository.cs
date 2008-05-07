using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Moq
{
	interface IProxyCallRepository : IEnumerable<IProxyCall>
	{
		void Add(IProxyCall call, ExpectKind kind);
	}
}
