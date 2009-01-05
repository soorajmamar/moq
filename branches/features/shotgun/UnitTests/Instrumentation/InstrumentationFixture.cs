using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using System.IO;
using Mono.Cecil;
using System.Reflection;
using Mono.Cecil.Cil;
using Moq.Instrumentation;

namespace Moq.Tests.Instrumentation
{
	public class InstrumentationFixture
	{
		[Fact]
		public void Misc()
		{
			var asm = AssemblyFactory.GetAssembly(this.GetType().Assembly.Location);
			var method = asm.Modules.Cast<ModuleDefinition>()
				.SelectMany(m => m.Types.Cast<TypeDefinition>())
				.Where(t => t.Name == "InvocationFixture")
				.SelectMany(t => t.Methods.Cast<MethodDefinition>())
				.Where(m => m.Parameters.Count == 0)
				.FirstOrDefault();

			foreach (var ins in method.Body.Instructions.Cast<Instruction>())
			{
				Console.WriteLine(ins);
			}

		}

		[Fact]
		public void InstrumentsClass()
		{
			var asm = AssemblyFactory.GetAssembly(this.GetType().Assembly.Location);
			//var moqAssemblyRef = asm.MainModule.AssemblyReferences.Add(typeof(Mock).Assembly);
			var interceptorRef = asm.MainModule.TypeReferences.Add(typeof(IInterceptor));
			//var typeTypeRef = asm.MainModule.TypeReferences.Add(typeof(Type));
			//var getTypeFromHandle = new MethodReference("GetTypeFromHandle", typeTypeRef, typeTypeRef, false, false, MethodCallingConvention.Default);
			//var methodBaseRef = asm.MainModule.TypeReferences.Add(typeof(MethodBase));
			//var getCurrentMethod = new MethodReference("GetCurrentMethod", methodBaseRef, methodBaseRef, false, false, MethodCallingConvention.Default);
			//var listOfObject = asm.MainModule.TypeReferences.Add(typeof(List<object>));
			//var addToList = new MethodReference("Add", listOfObject, null, true, false, MethodCallingConvention.Default);
			//var invocationTypeRef = asm.MainModule.TypeReferences.Add(typeof(Invocation));
			//var intTypeRef = asm.MainModule.TypeReferences.Add(typeof(int));

			foreach (var type in asm.Modules.Cast<ModuleDefinition>().SelectMany(a => a.Types.Cast<TypeDefinition>())
				// TODO: remove the Where for production.
				.Where(t => t.Name == "InstrumentedClass"))
			{
				// private IInterceptor __Interceptor;
				var interceptorDef = new FieldDefinition("__Interceptor", interceptorRef, Mono.Cecil.FieldAttributes.Private);
				type.Fields.Add(interceptorDef);
				var interceptorField = new FieldReference("__Interceptor", type, interceptorRef);

				// TODO: remove
				//var cw = asm.MainModule.Import(Reflect.GetMethod(() => Console.WriteLine((object)null)));

				foreach (var method in type.Methods.Cast<MethodDefinition>())
				{
					//var argsVar = new VariableDefinition(listOfObject);
					//var invocationVar = new VariableDefinition(invocationTypeRef);
					//// List<object> args;
					//method.Body.Variables.Add(argsVar);
					//// Invocation invocation;
					//method.Body.Variables.Add(invocationVar);

					var il = method.Body.CilWorker;
					var instructions = new List<Instruction>(new[] 
					{
						// if (__Interceptor != null)
						il.Create(OpCodes.Ldarg_0), 
						il.Create(OpCodes.Ldfld, interceptorField),
						il.Create(OpCodes.Ldnull), 
						il.Create(OpCodes.Ceq), 
						il.Create(OpCodes.Brtrue_S, method.Body.Instructions[0]), 
						// args = new List<object>();
						//il.Create(OpCodes.Newobj, listOfObject), 
						//il.Create(OpCodes.Stloc, argsVar)
					});

					//foreach (var paramDef in method.Parameters.Cast<ParameterDefinition>())
					//{
					//    instructions.Add(il.Create(OpCodes.Ldloc, argsVar));
					//    if (paramDef.IsOut)
					//    {
					//        // Out args are added with their default value.
					//        if (paramDef.ParameterType.IsValueType)
					//        {
					//            //args.Add(default(paramType));
					//            instructions.Add(il.Create(OpCodes.Ldc_I4_0));
					//            instructions.Add(il.Create(OpCodes.Box, intTypeRef));
					//        }
					//        else
					//        {
					//            //args.Add(null);
					//            instructions.Add(il.Create(OpCodes.Ldnull));
					//        }
					//    }
					//    else
					//    {
					//        // Regular or ref arguments are copied directly.
					//        //args.Add(param0..n);
					//        instructions.Add(il.Create(OpCodes.Ldarg, paramDef));
					//    }

					//    instructions.Add(il.Create(OpCodes.Callvirt, addToList));
					//}

					//instructions.AddRange(new[] 
					//{
					//    //typeof(TheType)
					//    il.Create(OpCodes.Ldtoken, type.ToReference()),
					//    il.Create(OpCodes.Call, getTypeFromHandle),
					//    //MethodBase.GetCurrentMethod()
					//    il.Create(OpCodes.Call, getCurrentMethod),
					//    //args
					//    il.Create(OpCodes.Ldloc, argsVar),
					//});

					//// default(TReturn)
					//if (method.ReturnType != null && method.ReturnType.ReturnType.IsValueType)
					//{
					//    //default(valuetype) == 0
					//    instructions.Add(il.Create(OpCodes.Ldc_I4_0));
					//    instructions.Add(il.Create(OpCodes.Box, intTypeRef));
					//}
					//else
					//{
					//    //default == null
					//    instructions.Add(il.Create(OpCodes.Ldnull));
					//}

					//instructions.Add(il.Create(OpCodes.Newobj, invocationTypeRef));

					////    il.Create(OpCodes.Ldstr, "intercepted"),
					////    il.Create(OpCodes.Ret)

					////    il.Create(OpCodes.Ldarg_0), 
					////    il.Create(OpCodes.Ldfld, interceptorField),
					////});

					//instructions.Add(il.Create(OpCodes.Ldloc, invocationVar));
					//instructions.Add(il.Create(OpCodes.Call, cw));

					il.InsertRangeBefore(method.Body.Instructions[0], instructions);
				}
			}

			//var cw = asm.MainModule.Import(Reflect.GetMethod(() => MethodBase.GetCurrentMethod()));
			//var cw = asm.MainModule.Import(Reflect.GetMethod(() => Console.WriteLine("foo")));

			//foreach (var typeMethod in definitions)
			//{
			//    var worker = typeMethod.Method.Body.CilWorker;
			//    //Creates the MSIL instruction for inserting the sentence
			//    var ldstr =  worker.Create(OpCodes.Ldstr, "Hello");

			//    //Creates the CIL instruction for calling the 
			//    //Console.WriteLine(string value) method
			//    var callcw = worker.Create(OpCodes.Call, cw);

			//    //Getting the first instruction of the current method
			//    var begin = typeMethod.Method.Body.Instructions[0];

			//    //Inserts the insertSentence instruction before the first //instruction
			//    worker.InsertBefore(begin, ldstr);

			//    //Inserts the callWriteLineMethod after the //insertSentence instruction
			//    worker.InsertAfter(ldstr, callcw);
			//}

			AssemblyFactory.SaveAssembly(asm, "InstrumentedAssembly.dll");
			System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo("InstrumentedAssembly.dll") { UseShellExecute = true });
		}

	}


	public class InstrumentedClass
	{
		public string Echo(string message)
		{
			return message;
		}

		public string Echo2(string message)
		{
			Console.WriteLine("Hello");
			return message;
		}

		public void VoidMethod(int value, out bool value2, ref string value3)
		{
			value2 = true;
		}
	}
}
