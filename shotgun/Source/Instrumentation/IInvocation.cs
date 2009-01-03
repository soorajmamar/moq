using System;
using System.Reflection;

namespace Moq.Instrumentation
{
	/// <summary>
	/// Represents an invocation to an instrumented/proxied member.
	/// </summary>
	public interface IInvocation
	{
		/// <summary>
		/// The target object where the invocation is being performed.
		/// </summary>
		object Target { get; set; }
		/// <summary>
		/// The type of the target object being invoked. Needed as 
		/// the <see cref="Target"/> may be null for static invocations.
		/// </summary>
		Type TargetType { get; set; }
		/// <summary>
		/// The member being invoked.
		/// </summary>
		MethodBase Method { get; set; }
		/// <summary>
		/// The arguments of the invocation (regular, ref and out).
		/// </summary>
		object[] Arguments { get; set; }
		/// <summary>
		/// The return value of the methor invocation, if any.
		/// </summary>
		object ReturnValue { get; set; }
		/// <summary>
		/// Whether the actual method should be invoked or not.
		/// </summary>
		bool ShouldContinue { get; set; }
	}
}
