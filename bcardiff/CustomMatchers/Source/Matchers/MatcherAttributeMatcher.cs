using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Linq.Expressions;

namespace Moq.Matchers
{
	/// <summary>
	/// Matcher to treat static functions as matchers.
	/// 
	/// mock.Expect(x => x.StringMethod(A.MagicString()));
	/// 
	/// pbulic static class A 
	/// {
	///     [Matcher]
	///     public static string MagicString() { return null; }
	///     public static bool MagicString(string arg)
	///     {
	///         return arg == "magic";
	///     }
	/// }
	/// 
	/// Will success if: mock.Object.StringMethod("magic");
	/// and fail with any other call.
	/// </summary>
	internal class MatcherAttributeMatcher : IMatcher
	{
		MethodInfo validatorMethod;
		Expression matcherExpression;

		public MatcherAttributeMatcher(MethodInfo validatorMethod)
		{
			this.validatorMethod = validatorMethod;
		}

		public void Initialize(Expression matcherExpression)
		{
			this.matcherExpression = matcherExpression;
		}

		public bool Matches(object value)
		{
			// use matcher Expression to get extra arguments
			MethodCallExpression call = (MethodCallExpression)matcherExpression;
			var extraArgs = call.Arguments.Select(ae => ((ConstantExpression)ae.PartialEval()).Value);
			var args = new[] { value }.Concat(extraArgs).ToArray();
			return (bool) validatorMethod.Invoke( null, args );
		}
	}
}
