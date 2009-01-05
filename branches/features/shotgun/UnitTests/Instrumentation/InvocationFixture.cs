using System;
using Moq.Instrumentation;
using System.Reflection;
using System.Collections.Generic;

namespace Moq.Tests.Instrumentation
{
	public class InvocationFixture
	{
		public IInterceptor __Interceptor;

		public int Submit(int a1, int a2, int a3, int a4, int a5, int a6, int a7, int a8, int a9, int a10)
		{
			if (__Interceptor != null)
			{
				Console.WriteLine(__Interceptor);
				return 2;
			}

			return a10;
		}

		public int Submit(string value1, out string value2, ref string value3, out int value4)
		{
			int result = 5;
			if (__Interceptor != null)
			{
				var args = new List<object>();
				args.Add(value1);
				args.Add(default(string));
				args.Add(value3);
				args.Add(default(int));
				var invocation = new Invocation(
					this, 
					typeof(InvocationFixture), 
					MethodBase.GetCurrentMethod(),
					args,
					default(int));

				//if (__Interceptor.Intercept(invocation))
				//{
				//    // set out/ref arguments
				//    value2 = (string)invocation.Arguments[1];
				//    value3 = (string)invocation.Arguments[2];

				//    return (int)invocation.ReturnValue;
				//}
			}

			value2 = "done";
			value4 = 2;
			return result;
		}
	}
}
