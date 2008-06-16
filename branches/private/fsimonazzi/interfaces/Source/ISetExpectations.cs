using System;
using System.Linq.Expressions;
using Moq.Language.Flow;

namespace Moq
{
	/// <summary>
	/// 
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public interface ISetExpectations<T> : IHideObjectMembers
	 where T : class
	{
		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="TResult"></typeparam>
		/// <param name="expression"></param>
		/// <returns></returns>
		IExpect<TResult> Expect<TResult>(Expression<Func<T, TResult>> expression);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="expression"></param>
		/// <returns></returns>
		IExpect Expect(Expression<Action<T>> expression);

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="TProperty"></typeparam>
		/// <param name="expression"></param>
		/// <returns></returns>
		IExpectGetter<TProperty> ExpectGet<TProperty>(Expression<Func<T, TProperty>> expression);

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="TProperty"></typeparam>
		/// <param name="expression"></param>
		/// <returns></returns>
		IExpectSetter<TProperty> ExpectSet<TProperty>(Expression<Func<T, TProperty>> expression);
	}
}
