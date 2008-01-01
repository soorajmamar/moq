using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using System.Reflection;
using System.Reflection.Emit;
using System.Linq.Expressions;

namespace Moq.Tests
{
	[TestFixture]
	public class MockTypeFixture
	{
		[Test]
		public void ShouldReceiveMock()
		{
			Type t = typeof(IFormattable);
			AssemblyName name = new AssemblyName("Foo");
			AssemblyBuilder asm = AppDomain.CurrentDomain.DefineDynamicAssembly(name, AssemblyBuilderAccess.Run);
			ModuleBuilder module = asm.DefineDynamicModule("Foo");
			TypeBuilder type = module.DefineType("Bar");
			foreach (var method in t.GetMethods())
			{
				MethodBuilder mb = type.DefineMethod(
					method.Name,
					method.Attributes,
					method.CallingConvention,
					method.ReturnType,
					(from prm in method.GetParameters()
					 select prm.ParameterType).ToArray());
				
			}

		}

		class DynamicMock : IFormattable
		{
			IInvoker invoker;

			public DynamicMock(IInvoker invoker)
			{
				this.invoker = invoker;
			}

			public DynamicMock()
			{
			}

			public string ToString(string format, IFormatProvider formatProvider)
			{
				//return invoker.Invoke(
				//    Reflector<IFormattable>.GetMethod(x => x.ToString(format, formatProvider)),
				//    null);
				Expression<Func<string>> expr = () => "foo".ToUpper();
				return (string)invoker.Invoke(typeof(IFormattable).GetMethod("ToString",
					new Type[] { typeof(string), typeof(IFormatProvider) }),
					format, formatProvider);
			}
		}

		interface IInvoker
		{
			object Invoke(MethodBase member, params object[] args);
		}
	}
}
