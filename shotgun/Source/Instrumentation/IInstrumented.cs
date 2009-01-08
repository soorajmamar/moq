using System;
using System.ComponentModel;

namespace Moq.Instrumentation
{
	/// <summary>
	/// 
	/// </summary>
	[EditorBrowsable(EditorBrowsableState.Never)]
	public interface IInstrumented
	{
		/// <summary>
		/// 
		/// </summary>
		IInterceptor Interceptor { get; set; }
	}
}
