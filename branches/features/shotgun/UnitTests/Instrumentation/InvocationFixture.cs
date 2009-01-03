using System;
using Moq.Instrumentation;
using System.Reflection;

namespace Moq.Tests.Instrumentation
{
	public class InvocationFixture
	{
		public class Foo
		{
			public IInterceptor __Interceptor;

			public int Submit(string value1, ref string value2)
			{
				if (__Interceptor != null)
				{
					var invocation = __Interceptor.Intercept(new Invocation(this, typeof(Foo), MethodBase.GetCurrentMethod(), value1, value2));
					value2 = (string)invocation.Arguments[1];

					// set other ref/out arguments.
					if (!invocation.ShouldContinue)
					{
						return (int)invocation.ReturnValue;
					}
				}

				return value1.GetHashCode() + value2.GetHashCode();
			}
		}
	}
}
