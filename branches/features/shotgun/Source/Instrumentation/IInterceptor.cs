using System;
using System.ComponentModel;

namespace Moq.Instrumentation
{
	/// <summary>
	/// 
	/// </summary>
	[EditorBrowsable(EditorBrowsableState.Never)]
	public interface IInterceptor
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="invocation"></param>
		/// <returns></returns>
		bool Intercept(IInvocation invocation);
	}
}
