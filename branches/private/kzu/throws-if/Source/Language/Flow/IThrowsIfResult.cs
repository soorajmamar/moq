using System.ComponentModel;
using System;

namespace Moq.Language.Flow
{
	/// <summary>
	/// Implements the fluent API.
	/// </summary>
	[EditorBrowsable(EditorBrowsableState.Never)]
	public interface IThrowsIfResult<TProperty> : ICondition<TProperty>, IOccurrence, IVerifies, IRaise, IHideObjectMembers
	{
	}
}
