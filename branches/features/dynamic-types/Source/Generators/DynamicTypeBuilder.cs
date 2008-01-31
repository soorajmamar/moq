using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection.Emit;
using System.Reflection;

namespace Moq.Generators
{
    internal class DynamicTypeBuilder
    {
        static ModuleBuilder moduleBuilder;
        static Dictionary<Type, Type> generatedTypes;

        static DynamicTypeBuilder()
        {
            var assemblyName = new AssemblyName("Moq.GeneratedTypes");
            var assemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);
            moduleBuilder = assemblyBuilder.DefineDynamicModule(assemblyName.Name, true);
            generatedTypes = new Dictionary<Type, Type>();
        }

        public static Type BuildType(Type _interface)
        {
            Type result;
            if (!generatedTypes.TryGetValue(_interface, out result))
            {
                result = InternalBuildType(_interface);
                generatedTypes.Add(_interface, result);
            }
            return result;
        }

        public static Type InternalBuildType(Type _interface)
        {
            var type = moduleBuilder.DefineType(
                GetMockTypeName(_interface),
                TypeAttributes.Public,
                typeof(object),
                new Type[] { _interface });
            return type;
        }

        private static string GetMockTypeName(Type type)
        {
            return "Mock_" + type.FullName;
        }
    }
}
