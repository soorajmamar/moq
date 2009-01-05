using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace Moq.Tests.Instrumentation
{
	public class CecilExtensionsFixture
	{
		[Fact]
		public void CilWorkerInsertsRangeBefore()
		{
			var method = new MethodDefinition("foo", MethodAttributes.Public, null);
			var worker = method.Body.CilWorker;

			var first = worker.Create(OpCodes.Ret);
			worker.Append(first);

			worker.InsertRangeBefore(first,
				worker.Create(OpCodes.Ldloc_0),
				worker.Create(OpCodes.Nop)
			);

			Assert.True(method.Body.Instructions[0].OpCode == OpCodes.Ldloc_0);
			Assert.True(method.Body.Instructions[1].OpCode == OpCodes.Nop);
			Assert.True(method.Body.Instructions[2].OpCode == OpCodes.Ret);
		}

		[Fact]
		public void CilWorkerInsertsRangeAfter()
		{
			var method = new MethodDefinition("foo", MethodAttributes.Public, null);
			var worker = method.Body.CilWorker;

			var first = worker.Create(OpCodes.Ret);
			worker.Append(first);

			worker.InsertRangeAfter(first,
				worker.Create(OpCodes.Ldloc_0),
				worker.Create(OpCodes.Nop)
			);

			Assert.True(method.Body.Instructions[0].OpCode == OpCodes.Ret);
			Assert.True(method.Body.Instructions[1].OpCode == OpCodes.Ldloc_0);
			Assert.True(method.Body.Instructions[2].OpCode == OpCodes.Nop);
		}

		[Fact]
		public void CilWorkerAppendsRange()
		{
			var method = new MethodDefinition("foo", MethodAttributes.Public, null);
			var worker = method.Body.CilWorker;

			var first = worker.Create(OpCodes.Ret);
			worker.Append(first);

			worker.AppendRange(
				worker.Create(OpCodes.Ldloc_0),
				worker.Create(OpCodes.Nop)
			);

			Assert.True(method.Body.Instructions[0].OpCode == OpCodes.Ret);
			Assert.True(method.Body.Instructions[1].OpCode == OpCodes.Ldloc_0);
			Assert.True(method.Body.Instructions[2].OpCode == OpCodes.Nop);
		}
	}
}
