using System;

namespace Moq.Instrumentation
{
	public interface IInterceptor
	{
		IInvocation Intercept(IInvocation invocation);
	}
}
