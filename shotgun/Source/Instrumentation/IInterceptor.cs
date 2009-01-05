using System;

namespace Moq.Instrumentation
{
	public interface IInterceptor
	{
		bool Intercept(IInvocation invocation);
	}
}
