﻿using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using Castle.Core.Interceptor;

namespace Moq
{
	internal class MethodCallReturn<TResult> : MethodCall, ICallReturn<TResult>, IProxyCall
	{
		TResult value;
		Func<TResult> valueFunc;

		public MethodCallReturn(Expression originalExpression, MethodInfo method, params Expression[] arguments)
			: base(originalExpression, method, arguments)
		{
		}

		public void Returns(Func<TResult> valueExpression)
		{
			this.valueFunc = valueExpression;
		}

		public void Returns(TResult value)
		{
			this.value = value;
		}

		public new ICallReturn<TResult> Callback(Action callback)
		{
			base.Callback(callback);
			return this;
		}

		public new ICallReturn<TResult> Verifiable()
		{
			IsVerifiable = true;

			return this;
		}

		public override void Execute(IInvocation call)
		{
			base.Execute(call);

			if (valueFunc != null)
				call.ReturnValue = valueFunc();
			else
				call.ReturnValue = value;
		}
	}
}
