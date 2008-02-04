using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Moq
{
	/// <summary>
	/// Implemented by all moked object.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public interface IMocked<T>
		where T : class
	{
		/// <summary>
		/// Reference the Mock that contains this as the Object.
		/// </summary>
		Mock<T> Mock { get; }
	}
}
