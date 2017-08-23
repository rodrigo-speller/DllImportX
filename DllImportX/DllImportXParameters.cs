using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace System.Runtime.InteropServices
{
    public sealed class DllImportXOptions
    {
        /// <summary>
        /// The name of the DLL that contains the unmanaged method. This can include an assembly
        /// display name, if the DLL is included in an assembly.
        /// </summary>
        public string DllName;

        /// <summary>
        /// Indicates the name or ordinal of the DLL entry point to be called.
        /// </summary>
        public string EntryPoint;

        /// <summary>
        /// Indicates how to marshal string parameters to the method and controls name mangling.
        /// </summary>
        public CharSet CharSet;

        /// <summary>
        /// Indicates whether the callee calls the SetLastError Win32 API function before
        /// returning from the attributed method.
        /// </summary>
        public bool SetLastError;

        /// <summary>
        /// Controls whether the System.Runtime.InteropServices.DllImportAttribute.CharSet
        /// field causes the common language runtime to search an unmanaged DLL for entry-point
        /// names other than the one specified.
        /// </summary>
        public bool ExactSpelling;

        /// <summary>
        /// Indicates whether unmanaged methods that have HRESULT or retval return values
        /// are directly translated or whether HRESULT or retval return values are automatically
        /// converted to exceptions.
        /// </summary>
        public bool PreserveSig;

        /// <summary>
        /// Indicates the calling convention of an entry point.
        /// </summary>
        public CallingConvention CallingConvention;

        /// <summary>
        /// Enables or disables best-fit mapping behavior when converting Unicode characters
        /// to ANSI characters.
        /// </summary>
        public bool BestFitMapping;

        /// <summary>
        /// Enables or disables the throwing of an exception on an unmappable Unicode character
        /// that is converted to an ANSI "?" character.
        /// </summary>
        public bool ThrowOnUnmappableChar;

        public bool IgnoreAttributes;

        public readonly MethodInfo Method;

        public DllImportXOptions(MethodInfo method)
        {
            Method = method;

            Initialize(method);
        }

        private void Initialize(MethodInfo method)
        {
            var import = (DllImportXAttribute)method.GetCustomAttributes(typeof(DllImportXAttribute), false).FirstOrDefault();
            if (import != null)
                Initialize(import);

            EntryPoint = EntryPoint ?? method.Name;
        }

        private void Initialize(DllImportXAttribute import)
        {
            DllName = import.Value;
            EntryPoint = import.EntryPoint;
            CharSet = import.CharSet;
            SetLastError = import.SetLastError;
            ExactSpelling = import.ExactSpelling;
            PreserveSig = import.PreserveSig;
            CallingConvention = import.CallingConvention;
            BestFitMapping = import.BestFitMapping;
            ThrowOnUnmappableChar = import.ThrowOnUnmappableChar;
        }
    }
}
