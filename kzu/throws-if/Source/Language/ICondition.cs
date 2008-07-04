using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Moq.Language.Flow;

namespace Moq.Language
{
	/// <summary>
	/// Defines the If API which allows restricting the 
	/// expectation using an arbitrary delegate. 
	/// </summary>
	/// <remarks>
	/// Currenly only available from <see cref="Mock{T}.ExpectSet"/>.
	/// </remarks>
	public interface ICondition<TValue>
	{
		/// <summary>
		/// Enables specification of a condition that must be met before 
		/// the expectation is considered.
		/// </summary>
		IThrowsResult If(Predicate<TValue> condition);
	}
}
