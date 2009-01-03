using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using System.IO;
using Mono.Cecil;
using System.Reflection;
using Mono.Cecil.Cil;

namespace Moq.Tests.Instrumentation
{
	public class InstrumentationFixture
	{
		[Fact]
		public void InstrumentsClass()
		{
			var asm = AssemblyFactory.GetAssembly(this.GetType().Assembly.Location);
			foreach (var type in asm.Modules.Cast<ModuleDefinition>().SelectMany(a => a.Types.Cast<TypeDefinition>())
				// TODO: remove the Where for production.
				.Where(t => t.Name == "InstrumentedClass"))
			{


				foreach (var method in type.Methods.Cast<MethodDefinition>())
				{
					
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
	}
}
