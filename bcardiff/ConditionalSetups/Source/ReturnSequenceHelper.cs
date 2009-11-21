using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using Moq.Language.Flow;
using Moq.Language;

namespace Moq
{
	/// <summary>
	/// Helper for sequencing return values in the same method.
	/// </summary>
	public static class ReturnSequenceHelper
	{
		/// <summary>
		/// Return a sequence of values, once per call.
		/// </summary>
		public static ISetupSequentialResult<TResult> SetupSequentials<TMock,TResult>(
			this Mock<TMock> mock,
			Expression<Func<TMock, TResult>> expression) 
			where TMock : class
		{
			return new SetupSequentialContext<TMock,TResult>(mock, expression);
		}
	}

	/// <summary>
	/// Language for ReturnSequence
	/// </summary>
	public interface ISetupSequentialResult<TResult>
	{
		// would be nice to Mixin somehow the IReturn and IThrows with
		// another ReturnType
 
		/// <summary>
		/// Returns value
		/// </summary>
		ISetupSequentialResult<TResult> Returns(TResult value);

		/// <summary>
		/// Throws an exception
		/// </summary>
		void Throws(Exception exception);

		/// <summary>
		/// Throws an exception
		/// </summary>
		void Throws<TException>() where TException : Exception, new();
	}

	class SetupSequentialContext<TMock,TResult> : ISetupSequentialResult<TResult>
		where TMock : class
	{
		int currentStep;
		int countOfExpectations;
		Mock<TMock> mock;
		Expression<Func<TMock, TResult>> expression;

		public SetupSequentialContext(Mock<TMock> mock,
			Expression<Func<TMock,TResult>> expression)
		{
			currentStep = 0;
			countOfExpectations = 0;
			this.mock = mock;
			this.expression = expression;
		}

		private ISetup<TMock,TResult> GetSetup()
		{
			var expectationStep = countOfExpectations;
			countOfExpectations++;

			return this.mock
				.When(() => currentStep == expectationStep)
				.Setup<TResult>(expression);
		}

		private void EndSetup(ICallback callback)
		{
			callback.Callback(() => { currentStep++; });
		}

		public ISetupSequentialResult<TResult> Returns(TResult value)
		{
			EndSetup(GetSetup().Returns(value));
			return this;
		}

		public void Throws(Exception exception)
		{
			GetSetup().Throws(exception);
		}

		public void Throws<TException>() where TException : Exception, new()
		{
			GetSetup().Throws<TException>();
		}
	}
}
