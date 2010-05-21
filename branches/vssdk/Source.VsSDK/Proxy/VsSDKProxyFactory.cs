//Copyright (c) 2007, Moq Team 
//http://code.google.com/p/moq/
//All rights reserved.

//Redistribution and use in source and binary forms, 
//with or without modification, are permitted provided 
//that the following conditions are met:

//    * Redistributions of source code must retain the 
//    above copyright notice, this list of conditions and 
//    the following disclaimer.

//    * Redistributions in binary form must reproduce 
//    the above copyright notice, this list of conditions 
//    and the following disclaimer in the documentation 
//    and/or other materials provided with the distribution.

//    * Neither the name of the Moq Team nor the 
//    names of its contributors may be used to endorse 
//    or promote products derived from this software 
//    without specific prior written permission.

//THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND 
//CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, 
//INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF 
//MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE 
//DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR 
//CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, 
//SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, 
//BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR 
//SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS 
//INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, 
//WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING 
//NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE 
//OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF 
//SUCH DAMAGE.

//[This is the BSD license, see
// http://www.opensource.org/licenses/bsd-license.php]

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VsSDK.UnitTestLibrary;
using System.Reflection;

namespace Moq.Proxy
{
		internal class ProxyFactory : IProxyFactory
		{
				public T CreateProxy<T>(ICallInterceptor interceptor, Type[] interfaces, object[] arguments)
						where T : class
				{
						if (!typeof(T).IsInterface)
								throw new NotSupportedException("Visual Studio SDK mock generator does not support mocking classes, only interfaces.");

						if (arguments.Length != 0)
								throw new NotSupportedException("Cannot pass object constructor arguments to interface mock.");

						var factory = new GenericMockFactory("Mock" + typeof(T).Name, interfaces);

						var mock = factory.GetInstance();
						var proxyInterceptor = new ProxyInterceptor(interceptor, typeof(T));

						// hook to everything.
						foreach (var method in
								typeof(T).GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
												.Where(mi => mi.IsPublic || mi.IsFamily))
						{
								mock.AddMethodCallback(method.Name, proxyInterceptor.OnMethodCallback);
						}

						foreach (var @interface in interfaces)
						{
								foreach (var method in
										@interface.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
														.Where(mi => mi.IsPublic || mi.IsFamily))
								{
										mock.AddMethodCallback(method.Name, proxyInterceptor.OnMethodCallback);
								}
						}

						return mock as T;
				}

				private class ProxyInterceptor
				{
						ICallInterceptor interceptor;
						Type targetType;

						public ProxyInterceptor(ICallInterceptor interceptor, Type targetType)
						{
								this.interceptor = interceptor;
								this.targetType = targetType;
						}

						public void OnMethodCallback(object sender, CallbackArgs args)
						{
								interceptor.Intercept(new ProxyCallContext(targetType, args));
						}

						private class ProxyCallContext : ICallContext
						{
								Type targetType;
								CallbackArgs callbackArgs;
								object[] argumentValues;

								public ProxyCallContext(Type targetType, CallbackArgs args)
								{
										this.targetType = targetType;
										this.callbackArgs = args;
										this.argumentValues = typeof(CallbackArgs)
												.GetField("parameters", BindingFlags.NonPublic | BindingFlags.Instance)
												.GetValue(args) as object[];
								}

								public object[] Arguments
								{
										get { return argumentValues; }
								}

								public MethodInfo Method
								{
										get { throw new NotImplementedException(); }
								}

								public object ReturnValue
								{
										get { return callbackArgs.ReturnValue; }
										set { callbackArgs.ReturnValue = value; }
								}

								public Type TargetType
								{
										get { return targetType; }
								}

								public void InvokeBase()
								{
								}

								public void SetArgumentValue(int index, object value)
								{
										callbackArgs.SetParameter(index, value);
								}
						}
				}
		}
}
