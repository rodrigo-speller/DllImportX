using DllImportX;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace System.Runtime.InteropServices
{
    public class DllImportXFactory
    {
        public const string InvalidTypeExceptionMessage = "";

        public static T Build<T>() where T : class => Build<T>(null);

        public static T Build<T>(DllImportXFilter filter) where T : class
        {
            var type = typeof(T);

            var concreteType = ImplementInterface(type, filter);
            return (T)Activator.CreateInstance(concreteType);
        }

        public static Type ImplementInterface(Type iface, DllImportXFilter filter)
        {
            var assemblyName = new AssemblyName("DllImportX");
#if NET35
            var assemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);
#else
            var assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);
#endif
            var moduleBuilder = assemblyBuilder.DefineDynamicModule(assemblyName.Name);
            var typeBuilder = DeclareType(iface, moduleBuilder);
            var methods = GetMethods(iface, filter, out var otherMethods);

            DefineImportMethods(typeBuilder, methods);
            DefineNotImplementedMethods(typeBuilder, otherMethods);

            return typeBuilder.CreateType();
        }

        public static TypeBuilder DeclareType(Type iface, ModuleBuilder moduleBuilder)
        {
            ValidateInterface(iface);

            var typeBuilder = moduleBuilder.DefineType(iface.Name + " (Implementation)", TypeAttributes.Public);
            typeBuilder.AddInterfaceImplementation(iface);

            return typeBuilder;
        }

        private static void ValidateInterface(Type type)
        {
#if NET35
            if (!type.IsInterface)
#else
            if (!type.GetTypeInfo().IsInterface)
#endif
            throw new InvalidOperationException(InvalidTypeExceptionMessage);
        }

        private static IEnumerable<DllImportXOptions> GetMethods(Type iface, DllImportXFilter filter, out IEnumerable<MethodInfo> others)
        {
            var methods = new { Imports = new List<DllImportXOptions>(), Others = new List<MethodInfo>() };

            foreach(var x in iface.GetMethods())
            {
                if (x.GetCustomAttributes(typeof(DllImportXAttribute), false).FirstOrDefault() == null)
                    methods.Others.Add(x);
                else
                    methods.Imports.Add(new DllImportXOptions(x));
            }

            if (filter != null)
            {
                foreach(var import in methods.Imports)
                    filter(import);
            }

            others = methods.Others;
            return methods.Imports;
        }

        private static void DefineImportMethods(TypeBuilder typeBuilder, IEnumerable<DllImportXOptions> imports)
        {
            foreach(var options in imports)
                ImplementMethod(typeBuilder, options);
        }

        private static void ImplementMethod(TypeBuilder typeBuilder, DllImportXOptions options)
        {
            var method = options.Method;
            var parameters = method.GetParameters().Select(x => x.ParameterType).ToArray();
            var callee = DefinePInvokeMethod(typeBuilder, options);
            var caller = typeBuilder.DefineMethod(method.Name, MethodAttributes.Virtual, method.ReturnType, parameters);
            var il = caller.GetILGenerator();

            for (int i = 1, c = parameters.Length; i <= c; i++)
            {
                il.Emit(OpCodes.Ldarg, i);
            }
            il.Emit(OpCodes.Call, callee);
            il.Emit(OpCodes.Ret);

            typeBuilder.DefineMethodOverride(caller, options.Method);
        }

        public static MethodInfo DefinePInvokeMethod(TypeBuilder typeBuilder, DllImportXOptions options)
        {
            var clrImportType = typeof(DllImportAttribute);
            var ctor = clrImportType.GetConstructor(new[] { typeof(string) });
            
            var fields = new[] {
                clrImportType.GetField("EntryPoint"),
                clrImportType.GetField("ExactSpelling"),
                clrImportType.GetField("PreserveSig"),
                clrImportType.GetField("SetLastError"),
                clrImportType.GetField("CallingConvention"),
                clrImportType.GetField("CharSet"),
                clrImportType.GetField("BestFitMapping"),
                clrImportType.GetField("ThrowOnUnmappableChar")
            };

            var fieldValues = new object[] {
                options.EntryPoint,
                options.ExactSpelling,
                options.PreserveSig,
                options.SetLastError,
                options.CallingConvention,
                options.CharSet,
                options.BestFitMapping,
                options.ThrowOnUnmappableChar
            };

            var clrImport = new CustomAttributeBuilder(ctor, new[] { options.DllName }, fields, fieldValues);

            var ifaceMethod = options.Method;
            var parameters = ifaceMethod.GetParameters();
            var method = typeBuilder.DefineMethod(
                ifaceMethod.Name,
                MethodAttributes.Private | MethodAttributes.Static,
                ifaceMethod.ReturnType,
                ifaceMethod.GetParameters().Select(x => x.ParameterType).ToArray()
            );

            if(!options.IgnoreAttributes)
                DefineAttributes(ifaceMethod, method);

            method.SetCustomAttribute(clrImport);
            return method;
        }

        private static void DefineAttributes(MethodInfo ifaceMethod, MethodBuilder method)
        {
            var parameters = ifaceMethod.GetParameters();

            CopyAttributes<MarshalAsAttribute>(ifaceMethod.ReturnParameter, method.DefineParameter(0, ParameterAttributes.None, null), (x, build) => build(new[] { typeof(UnmanagedType) }, new object[] { x.Value }));
            for (int i = 0, c = parameters.Length; i < c;)
            {
                var ifaceParam = parameters[i++];
                var paramBuilder = method.DefineParameter(i, ParameterAttributes.None, ifaceParam.Name);

                CopyAttributes<InAttribute>(ifaceParam, paramBuilder,
                    (x, build) => build(
                        Type.EmptyTypes,
                        new object[0]
                    )
                );
                CopyAttributes<OutAttribute>(ifaceParam, paramBuilder,
                    (x, build) => build(
                        Type.EmptyTypes,
                        new object[0]
                    )
                );
                CopyAttributes<MarshalAsAttribute>(ifaceParam, paramBuilder,
                    (x, build) => build(
                        new[] { typeof(UnmanagedType) },
                        new object[] { x.Value }
                    )
                );
            }
        }

        private static void CopyAttributes<TAttributes>(
                ParameterInfo ifaceParam,
                ParameterBuilder parameter,
                Action<TAttributes, Action<Type[], object[]>> builder)
                where TAttributes : Attribute
        {
#if NET35
                var ifaceAttr = (TAttributes)ifaceParam
                    .GetCustomAttributes(typeof(TAttributes), false)
                    .FirstOrDefault();
#else
            var ifaceAttr = ifaceParam.GetCustomAttribute<TAttributes>();
#endif
            if (ifaceAttr == null)
                return;

            builder(ifaceAttr, (types, values) =>
            {
                if (builder == null)
                    return;

                builder = null;

                var parameterAttrCtor = typeof(TAttributes).GetConstructor(types);
                var attributeBuilder = new CustomAttributeBuilder(parameterAttrCtor, values);
                parameter.SetCustomAttribute(attributeBuilder);
            });
        }

        private static void DefineNotImplementedMethods(TypeBuilder typeBuilder, IEnumerable<MethodInfo> otherMethods)
        {
            var clrExceptionType = typeof(NotImplementedException);
            var ctor = clrExceptionType.GetConstructor(Type.EmptyTypes);

            foreach(var method in otherMethods)
            {
                var caller = typeBuilder
                    .DefineMethod(
                        method.Name,
                        MethodAttributes.Virtual,
                        method.ReturnType,
                        method.GetParameters().Select(x => x.ParameterType).ToArray()
                    );

                var il = caller.GetILGenerator();
                il.Emit(OpCodes.Newobj, ctor);
                il.Emit(OpCodes.Throw);

                typeBuilder.DefineMethodOverride(caller, method);
            }
        }
    }
}
