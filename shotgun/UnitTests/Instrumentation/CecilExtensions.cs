using System;
using System.Linq;
using System.Reflection;
using Mono.Cecil.Cil;
using System.Collections;
using System.Collections.Generic;

namespace Mono.Cecil
{
	/// <summary>
	/// Mono Cecil API extensions.
	/// </summary>
	public static class AssemblyReferenceCollectionExtensions
	{
		/// <summary>
		/// Adds a reference to the given assembly if one does not exist already in the collection.
		/// </summary>
		/// <param name="collection">The collection to add the reference to.</param>
		/// <param name="assembly">The reflection assembly to add as a reference to the collection.</param>
		/// <returns>The <see cref="AssemblyNameReference"/> that was newly added or located (if already existing).</returns>
		public static AssemblyNameReference Add(this AssemblyNameReferenceCollection collection, Assembly assembly)
		{
			var asmName = assembly.GetName();
			var asmRef = collection.Cast<AssemblyNameReference>()
				.Where(r => r.FullName == asmName.FullName)
				.FirstOrDefault();

			if (asmRef == null)
			{
				asmRef = new AssemblyNameReference(
					asmName.Name, asmName.CultureInfo.Name, asmName.Version);
				asmRef.PublicKeyToken = asmName.GetPublicKeyToken();
				asmRef.HashAlgorithm = (AssemblyHashAlgorithm)asmName.HashAlgorithm;
				asmRef.Culture = asmName.CultureInfo.ToString();
				collection.Add(asmRef);
			}

			return asmRef;
		}
	}

	/// <summary>
	/// Mono Cecil API extensions.
	/// </summary>
	public static class TypeReferenceCollectionExtensions
	{
		/// <summary>
		/// Adds the given type as a reference and returns the corresponding 
		/// <see cref="TypeReference"/>.
		/// </summary>
		public static TypeReference Add(this TypeReferenceCollection collection, Type type)
		{
			var asmRef = collection.Container.AssemblyReferences.Add(type.Assembly);
			var typeRef = new TypeReference(type.Name, type.Namespace, asmRef, type.IsValueType);
			collection.Container.TypeReferences.Add(typeRef);
			return typeRef;
		}
	}

	/// <summary>
	/// Mono Cecil API extensions.
	/// </summary>
	public static class CilWorkerExtensions
	{
		/// <summary>
		/// Appends the given instructions at the end of the current instructions.
		/// </summary>
		public static void AppendRange(this CilWorker worker, IEnumerable<Instruction> instructions)
		{
			foreach (var instruction in instructions)
			{
				worker.Append(instruction);
			}
		}

		/// <summary>
		/// Appends the given instructions at the end of the current instructions.
		/// </summary>
		public static void AppendRange(this CilWorker worker, params Instruction[] instructions)
		{
			AppendRange(worker, (IEnumerable<Instruction>)instructions);
		}

		/// <summary>
		/// Inserts the given instructions after the occurrence of <paramref name="target"/>.
		/// </summary>
		public static void InsertRangeAfter(this CilWorker worker, Instruction target, IEnumerable<Instruction> instructions)
		{
			foreach (var instruction in instructions)
			{
				worker.InsertAfter(target, instruction);
				target = instruction;
			}
		}

		/// <summary>
		/// Inserts the given instructions after the occurrence of <paramref name="target"/>.
		/// </summary>
		public static void InsertRangeAfter(this CilWorker worker, Instruction target, params Instruction[] instructions)
		{
			InsertRangeAfter(worker, target, (IEnumerable<Instruction>)instructions);
		}

		/// <summary>
		/// Inserts the given instructions before the occurrence of <paramref name="target"/>.
		/// </summary>
		public static void InsertRangeBefore(this CilWorker worker, Instruction target, IEnumerable<Instruction> instructions)
		{
			foreach (var instruction in instructions.Reverse())
			{
				worker.InsertBefore(target, instruction);
				target = instruction;
			}
		}

		/// <summary>
		/// Inserts the given instructions before the occurrence of <paramref name="target"/>.
		/// </summary>
		public static void InsertRangeBefore(this CilWorker worker, Instruction target, params Instruction[] instructions)
		{
			InsertRangeBefore(worker, target, (IEnumerable<Instruction>)instructions);
		}
	}
}
