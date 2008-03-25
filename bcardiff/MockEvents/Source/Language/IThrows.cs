﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Moq.Language.Flow;
using System.ComponentModel;

namespace Moq.Language
{
	/// <summary>
	/// Defines the <c>Throws</c> verb.
	/// </summary>
	[EditorBrowsable(EditorBrowsableState.Never)]
	public interface IThrows : IHideObjectMembers
	{
		/// <summary>
		/// Specifies the exception to throw when the method is invoked.
		/// </summary>
		/// <param name="exception">Exception instance to throw.</param>
		/// <example>
		/// This example shows how to throw an exception when the method is 
		/// invoked with an empty string argument:
		/// <code>
		/// mock.Expect(x => x.Execute(""))
		///     .Throws(new ArgumentException());
		/// </code>
		/// </example>
		IOnceVerifies Throws(Exception exception);
	}
}
