using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using Moq.Language.Flow;

namespace Moq.Language
{
	/// <summary>
	/// Defines the <c>Throws</c> verb for the expectations on a setter.
	/// </summary>
	[EditorBrowsable(EditorBrowsableState.Never)]
	public interface IThrowsIf<TProperty> : IHideObjectMembers
	{
		/// <summary>
		/// Specifies the exception to throw when the method is invoked.
		/// </summary>
		/// <param name="exception">Exception instance to throw.</param>
		/// <example>
		/// This example shows how to throw an exception when the method is 
		/// invoked with an empty string argument:
		/// <code>
		/// mock.ExpectSet(x =&gt; x.Value)
		///     .Throws(new ArgumentException())
		///     .If(s => String.IsNullOrEmpty(s));
		/// </code>
		/// </example>
		IThrowsIfResult<TProperty> Throws(Exception exception);

		/// <summary>
		/// Specifies the type of exception to throw when the method is invoked.
		/// </summary>
		/// <typeparam name="TException">Type of exception to instantiate and throw when the expectation is met.</typeparam>
		/// <example>
		/// This example shows how to throw an exception when the method is 
		/// invoked with an empty string argument:
		/// <code>
		/// mock.ExpectSet(x =&gt; x.Value)
		///     .Throws&lt;ArgumentException&gt;()
		///     .If(s => String.IsNullOrEmpty(s));
		/// </code>
		/// </example>
		IThrowsIfResult<TProperty> Throws<TException>() where TException : Exception, new();
	}
}
