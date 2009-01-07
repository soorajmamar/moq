using System;
using System.ComponentModel;

namespace Moq.Instrumentation
{
	[EditorBrowsable(EditorBrowsableState.Never)]
	public interface IInstrumented
	{
		IInterceptor Interceptor { get; set; }
	}
}
