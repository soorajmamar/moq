using System.ComponentModel;
using System;

namespace Moq.Language.Flow
{
	/// <summary>
	/// Implements the fluent API.
	/// </summary>
	[EditorBrowsable(EditorBrowsableState.Never)]
	public interface IExpect : ICallback, IThrowsOnceVerifiesRaise, IRaise, IHideObjectMembers
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="constraint"></param>
		/// <returns></returns>
		IExpect When(Func<bool> constraint);
	}

	/// <summary>
	/// Implements the fluent API.
	/// </summary>
	[EditorBrowsable(EditorBrowsableState.Never)]
	public interface IExpect<TResult> : ICallback<TResult>, IReturnsThrows<TResult>, IHideObjectMembers
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="constraint"></param>
		/// <returns></returns>
		IExpect<TResult> When(Func<bool> constraint);
	}

	/// <summary>
	/// Implements the fluent API.
	/// </summary>
	[EditorBrowsable(EditorBrowsableState.Never)]
	public interface IExpectGetter<TProperty> : ICallbackGetter<TProperty>, IReturnsThrowsGetter<TProperty>, IHideObjectMembers
	{
	}

	/// <summary>
	/// Implements the fluent API.
	/// </summary>
	[EditorBrowsable(EditorBrowsableState.Never)]
	public interface IExpectSetter<TProperty> : ICallbackSetter<TProperty>, IThrowsOnceVerifiesRaise, IRaise, IHideObjectMembers
	{
	}
}
