using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Linq.Expressions;

namespace Moq.Matchers
{
	internal class CustomMatcherMatcher : IMatcher
	{
		MethodInfo validatorMethod;
		Expression matcherExpression;

		public CustomMatcherMatcher(MethodInfo validatorMethod)
		{
			this.validatorMethod = validatorMethod;
		}

		public void Initialize(Expression matcherExpression)
		{
			this.matcherExpression = matcherExpression;
		}

		public bool Matches(object value)
		{
			// TODO use matcher Expression to get extra arguments
			MethodCallExpression call = (MethodCallExpression)matcherExpression;
			var extraArgs = call.Arguments.Select(ae => ((ConstantExpression)ae.PartialEval()).Value);
			var args = new[] { value }.Concat(extraArgs).ToArray();
			return (bool) validatorMethod.Invoke( null, args );
		}
	}
}
