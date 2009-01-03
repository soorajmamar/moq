using System;
using System.Reflection;

namespace Moq.Instrumentation
{
	internal class Invocation : IInvocation
	{
		public Invocation(object target, Type targetType, MethodBase method, params object[] args)
		{
			this.Target = target;
			this.TargetType = targetType;
			this.Method = method;
			this.Arguments = args;
			this.ShouldContinue = true;
		}

		public object Target { get; set; }
		public Type TargetType { get; set; }
		public MethodBase Method { get; set; }
		public object[] Arguments { get; set; }
		public object ReturnValue { get; set; }

		public bool ShouldContinue { get; set; }
	}
}
