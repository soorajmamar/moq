using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.Core.Interceptor;
using System.Reflection;

namespace Moq
{
	class Interceptor : MarshalByRefObject, IInterceptor
	{
		List<IProxyCall> calls = new List<IProxyCall>();
		List<MethodInfo> objectMethods = new List<MethodInfo>(new MethodInfo[] {
			Reflector<object>.GetMethod(x => x.GetType()), 
			Reflector<object>.GetMethod(x => x.Equals(null)), 
			Reflector<object>.GetMethod(x => x.GetHashCode()), 
			Reflector<object>.GetMethod(x => x.ToString())});

		public void AddCall(IProxyCall call)
		{
			calls.Add(call);
		}

		public void Intercept(IInvocation invocation)
		{
			var call = calls.Find(x => x.Matches(invocation));
			if (call != null)
			{
				call.Execute(invocation);
			}
			else if (invocation.Method.DeclaringType == typeof(object))
			{
				invocation.Proceed();
			}
			else if (invocation.Method != null && invocation.Method.ReturnType != null &&
				invocation.Method.ReturnType != typeof(void))
			{
				List<string> values = new List<string>(invocation.Arguments.Length);
				// Build arguments
				invocation.Arguments.ForEach(
					x => values.Add(x == null ? "null" : (x is string ? "\"" + (string)x + "\"" : x.ToString())));

				throw new InvalidOperationException(String.Format(
					Properties.Resources.UndeterminedReturnValue,
					invocation.Method.DeclaringType.Name,
					invocation.Method.Name,
					String.Join(", ", values.ToArray())));
			}
		}
	}
}
