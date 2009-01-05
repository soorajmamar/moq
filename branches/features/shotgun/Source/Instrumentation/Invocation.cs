using System;
using System.Reflection;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Moq.Instrumentation
{
	internal class Invocation : IInvocation
	{
		public Invocation(object target, Type targetType, MethodBase method, List<object> args, object returnValue)
		{
			this.Target = target;
			this.TargetType = targetType;
			this.Method = method;
			this.ReturnValue = ReturnValue;
			this.Arguments = new ReadOnlyCollection<object>(args);
		}

		public object Target { get; private set; }
		public Type TargetType { get; private set; }
		public MethodBase Method { get; private set; }
		public ReadOnlyCollection<object> Arguments { get; private set; }
		public object ReturnValue { get; set; }
	}
}
