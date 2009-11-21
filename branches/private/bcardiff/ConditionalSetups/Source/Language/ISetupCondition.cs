using System;
using Moq.Language.Flow;

namespace Moq.Language
{
	/// <summary>
	/// Defines the 'When'
	/// </summary>
	public interface ISetupCondition<TMock>
		where TMock : class
	{
		/// <summary>
		/// The expectation will be considered only when the condition
		/// evaluates to true.
		/// </summary>
		ISetupConditionResult<TMock> When(Func<bool> condition);
	}

	/// <summary>
	/// Defines the 'When'
	/// </summary>
	public interface ISetupCondition<TMock, TResult>
		where TMock : class
	{
		/// <summary>
		/// The expectation will be considered only when the condition
		/// evaluates to true.
		/// </summary>
		ISetupConditionResult<TMock, TResult> When(Func<bool> condition);
	}
}
