using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using System.IO;
using Mono.Cecil;
using SR = System.Reflection;
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
		public void CecilRepro()
		{
			var asm = AssemblyFactory.GetAssembly(this.GetType().Assembly.Location);
			var listOfObject = asm.MainModule.Import(typeof(List<object>));
			var method = asm.MainModule.Types.Cast<TypeDefinition>()
				.Where(t => t.FullName == typeof(InvocationFixture).FullName)
				.Single();

			foreach (var prop in method.Properties.Cast<PropertyDefinition>())
			{
				Console.WriteLine(prop.Attributes);
			}
		}

		[Fact]
		public void InstrumentsClass()
		{
			var asm = AssemblyFactory.GetAssembly(this.GetType().Assembly.Location);
			var interceptorRef = asm.MainModule.Import(typeof(IInterceptor));
			var typeTypeRef = asm.MainModule.Import(typeof(Type));
			var getTypeFromHandle = asm.MainModule.Import(SR.Reflect.GetMethod(() => Type.GetTypeFromHandle(new RuntimeTypeHandle())));
			var methodBaseRef = asm.MainModule.Import(typeof(SR.MethodBase));
			var getCurrentMethod = asm.MainModule.Import(SR.Reflect.GetMethod(() => SR.MethodBase.GetCurrentMethod()));
			var listOfObject = asm.MainModule.Import(typeof(List<object>));
			var listOfObjectCtor = asm.MainModule.Import(SR.Reflect.GetConstructor(() => new List<object>()));
			var addToList = asm.MainModule.Import(SR.Reflect<List<object>>.GetMethod(l => l.Add(new object())));
			var invocationTypeRef = asm.MainModule.Import(typeof(Invocation));
			var invocationCtor = asm.MainModule.Import(SR.Reflect.GetConstructor(() => new Invocation(null, null, null, null, null)));
			var iinstrumentedRef = asm.MainModule.Import(typeof(IInstrumented));

			foreach (var type in asm.Modules.Cast<ModuleDefinition>().SelectMany(a => a.Types.Cast<TypeDefinition>())
				// TODO: remove the Where for production.
				.Where(t => t.Name == "InstrumentedClass"))
			{
				// IInstrumented interface
				type.Interfaces.Add(iinstrumentedRef);

				// public IInterceptor Interceptor
				var interceptorProp = new PropertyDefinition(SR.Reflect<IInstrumented>.GetProperty(r => r.Interceptor).Name,
					interceptorRef, (PropertyAttributes)0);
				// private IInterceptor __Interceptor;  /* Backing field */
				var interceptorDef = new FieldDefinition("__" + interceptorProp.Name, interceptorRef, 
					FieldAttributes.Public);
				type.Fields.Add(interceptorDef);
				// { get { return __interceptor; }
				interceptorProp.GetMethod = new MethodDefinition("get_" + interceptorProp.Name,
					MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.Virtual,
					interceptorRef);
				interceptorProp.GetMethod.Body.CilWorker.AppendRange(
						interceptorProp.GetMethod.Body.CilWorker.Create(OpCodes.Ldarg_0), 
						interceptorProp.GetMethod.Body.CilWorker.Create(OpCodes.Ldfld, interceptorDef),
						interceptorProp.GetMethod.Body.CilWorker.Create(OpCodes.Ret)
				);
				// { set { __interceptor = value; }
				interceptorProp.SetMethod = new MethodDefinition("set_" + interceptorProp.Name,
					MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.Virtual,
					interceptorRef);
				interceptorProp.GetMethod.Body.CilWorker.AppendRange(
						interceptorProp.GetMethod.Body.CilWorker.Create(OpCodes.Ldarg_0),
						interceptorProp.GetMethod.Body.CilWorker.Create(OpCodes.Ldarg_1),
						interceptorProp.GetMethod.Body.CilWorker.Create(OpCodes.Stfld, interceptorDef),
						interceptorProp.GetMethod.Body.CilWorker.Create(OpCodes.Ret)
				);

				type.Properties.Add(interceptorProp);

				foreach (var method in type.Methods.Cast<MethodDefinition>())
				{
					method.Body.InitLocals = true;
					var argsVar = new VariableDefinition(listOfObject);
					var invocationVar = new VariableDefinition(invocationTypeRef);
					// List<object> args;
					method.Body.Variables.Add(argsVar);
					// Invocation invocation;
					method.Body.Variables.Add(invocationVar);

					var il = method.Body.CilWorker;
					var instructions = new List<Instruction>(new[] 
					{
						// if (__Interceptor != null)
						il.Create(OpCodes.Ldarg_0), 
						il.Create(OpCodes.Ldfld, interceptorDef),
						il.Create(OpCodes.Ldnull), 
						il.Create(OpCodes.Ceq), 
						il.Create(OpCodes.Brtrue_S, method.Body.Instructions[0]), 
						// args = new List<object>();
						il.Create(OpCodes.Newobj, listOfObjectCtor), 
						il.Create(OpCodes.Stloc, argsVar)
					});

					foreach (var paramDef in method.Parameters.Cast<ParameterDefinition>())
					{
						instructions.Add(il.Create(OpCodes.Ldloc, argsVar));
						if (paramDef.IsOut)
						{
							// Out args are added with their default value.
							if (paramDef.ParameterType.IsValueType)
							{
								//args.Add(default(paramType));
								instructions.Add(il.Create(OpCodes.Ldc_I4_0));
								instructions.Add(il.Create(OpCodes.Box, paramDef.ParameterType));
							}
							else
							{
								//args.Add(null);
								instructions.Add(il.Create(OpCodes.Ldnull));
							}
						}
						else
						{
							if (paramDef.ParameterType.IsValueType)
							{
								// This is what the C# compiler does: just passes a default value :|
								if (paramDef.ParameterType.Name.EndsWith("&"))
								{
									// Load default value.
									instructions.Add(il.Create(OpCodes.Ldc_I4_0));
								}
								else
								{
									instructions.Add(il.Create(OpCodes.Ldarg, paramDef));
								}

								instructions.Add(il.Create(OpCodes.Box, paramDef.ParameterType));
							}
							else
							{
								instructions.Add(il.Create(OpCodes.Ldarg, paramDef));

								// TODO is this "&" lookup the right way to do this??
								if (paramDef.ParameterType.Name.EndsWith("&"))
									// Dereference byref addresses
									instructions.Add(il.Create(OpCodes.Ldind_Ref));
							}
						}

						instructions.Add(il.Create(OpCodes.Callvirt, addToList));
					}

					instructions.AddRange(new[] 
					{
						//this
						il.Create(OpCodes.Ldloc_0),
					    //typeof(TheType)
					    il.Create(OpCodes.Ldtoken, type),
					    il.Create(OpCodes.Call, getTypeFromHandle),
					    //MethodBase.GetCurrentMethod()
					    il.Create(OpCodes.Call, getCurrentMethod),
					    //args
					    il.Create(OpCodes.Ldloc, argsVar),
					});

					// default(TReturn)
					if (method.ReturnType.ReturnType.FullName != typeof(void).FullName && 
						method.ReturnType.ReturnType.IsValueType)
					{
						//default(valuetype) == 0
						instructions.Add(il.Create(OpCodes.Ldc_I4_0));
						instructions.Add(il.Create(OpCodes.Box, method.ReturnType.ReturnType));
					}
					else
					{
						//default == null
						instructions.Add(il.Create(OpCodes.Ldnull));
					}

					instructions.Add(il.Create(OpCodes.Newobj, invocationCtor));

					// TODO: remove
					instructions.Add(il.Create(OpCodes.Ldloc, invocationVar));
					var cw = asm.MainModule.Import(SR.Reflect.GetMethod(() => Console.WriteLine((object)null)));

					instructions.Add(il.Create(OpCodes.Call, cw));

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

		public void VoidMethod(int value, out bool value2, ref string value3, ref int value4)
		{
			value2 = true;
		}
	}
}
