using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;

#if NETSTANDARD1_5

namespace DllImportX
{
    internal static class ReflectionExtensions
    {
        public static Type CreateType(this TypeBuilder source)
        {
            return source.CreateTypeInfo().AsType();
        }

        public static ConstructorInfo GetConstructor(this Type source, Type[] types)
        {
            return source.GetTypeInfo().GetConstructor(types);
        }

        public static FieldInfo GetField(this Type source, string name)
        {
            return source.GetTypeInfo().GetField(name);
        }

        public static MethodInfo[] GetMethods(this Type source)
        {
            return source.GetTypeInfo().GetMethods();
        }
    }
}

#endif