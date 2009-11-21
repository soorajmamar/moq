using System.ComponentModel;

namespace Moq.Language.Flow
{
	/// <summary>
	/// Implements the fluent API.
	/// </summary>
	[EditorBrowsable(EditorBrowsableState.Never)]
	public interface ISetupConditionResult<TMock> : ICallback, ICallbackResult, IRaise<TMock>, IRaise, INever, IVerifies, IHideObjectMembers
		where TMock : class
	{
	}

	/// <summary>
	/// Implements the fluent API.
	/// </summary>
	[EditorBrowsable(EditorBrowsableState.Never)]
	public interface ISetupConditionResult<TMock, TResult> : ICallback<TMock, TResult>, IReturnsThrows<TMock, TResult>, INever, IVerifies, IHideObjectMembers
		where TMock : class
	{
	}
}
