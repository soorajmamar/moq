using System;
using System.Reflection;
using System.Collections.ObjectModel;

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
		object Target { get; }
		/// <summary>
		/// The type of the target object being invoked. Needed as 
		/// the <see cref="Target"/> may be null for static invocations.
		/// </summary>
		Type TargetType { get; }
		/// <summary>
		/// The member being invoked.
		/// </summary>
		MethodBase Method { get; }
		/// <summary>
		/// The arguments of the invocation (regular, ref and out).
		/// </summary>
		ReadOnlyCollection<object> Arguments { get; }
		/// <summary>
		/// The return value of the method invocation, if any.
		/// </summary>
		object ReturnValue { get; set; }
	}
}
