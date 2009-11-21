using System.ComponentModel;
using Moq.Language.Flow;
using System.Linq.Expressions;
using System;

namespace Moq.Language
{
	/// <summary>
	/// Implements the fluent API.
	/// </summary>
	[EditorBrowsable(EditorBrowsableState.Never)]
	public interface ISetupConditionResult<TMock>
		where TMock : class
	{
		/// <summary>
		/// The expectation will be considered only in the former condition.
		/// </summary>
		/// <param name="expression"></param>
		/// <returns></returns>
		ISetup<TMock> Setup(Expression<Action<TMock>> expression);

		/// <summary>
		/// The expectation will be considered only in the former condition.
		/// </summary>
		/// <typeparam name="TResult"></typeparam>
		/// <param name="expression"></param>
		/// <returns></returns>
		ISetup<TMock, TResult> Setup<TResult>(Expression<Func<TMock, TResult>> expression);
	}
}
