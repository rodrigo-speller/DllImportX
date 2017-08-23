namespace System.Runtime.InteropServices
{
    /// <summary>
    /// Indicates that the attributed method is exposed by an unmanaged dynamic-link
    /// library (DLL) as a static entry point.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    [ComVisible(true)]
    public sealed class DllImportXAttribute : Attribute
    {
        /// <summary>
        /// Indicates the name or ordinal of the DLL entry point to be called.
        /// </summary>
        public string EntryPoint;

        /// <summary>
        /// Indicates how to marshal string parameters to the method and controls name mangling.
        /// </summary>
        public CharSet CharSet = CharSet.Ansi;

        /// <summary>
        /// Indicates whether the callee calls the SetLastError Win32 API function before
        /// returning from the attributed method.
        /// </summary>
        public bool SetLastError = false;

        /// <summary>
        /// Controls whether the System.Runtime.InteropServices.DllImportAttribute.CharSet
        /// field causes the common language runtime to search an unmanaged DLL for entry-point
        /// names other than the one specified.
        /// </summary>
        public bool ExactSpelling = false;

        /// <summary>
        /// Indicates whether unmanaged methods that have HRESULT or retval return values
        /// are directly translated or whether HRESULT or retval return values are automatically
        /// converted to exceptions.
        /// </summary>
        public bool PreserveSig = true;

        /// <summary>
        /// Indicates the calling convention of an entry point.
        /// </summary>
        public CallingConvention CallingConvention = CallingConvention.Winapi;

        /// <summary>
        /// Enables or disables best-fit mapping behavior when converting Unicode characters
        /// to ANSI characters.
        /// </summary>
        public bool BestFitMapping = true;

        /// <summary>
        /// Enables or disables the throwing of an exception on an unmappable Unicode character
        /// that is converted to an ANSI "?" character.
        /// </summary>
        public bool ThrowOnUnmappableChar = false;

        /// <summary>
        /// Initializes a new instance of the System.Runtime.InteropServices.DllImportXAttribute
        /// class with the name of the DLL containing the method to import.
        /// </summary>
        public DllImportXAttribute() { }

        /// <summary>
        /// Initializes a new instance of the System.Runtime.InteropServices.DllImportXAttribute
        /// class with the name of the DLL containing the method to import.
        /// </summary>
        /// 
        /// <param name="dllName">
        /// The name of the DLL that contains the unmanaged method. This can include an assembly
        /// display name, if the DLL is included in an assembly.
        /// </param>
        public DllImportXAttribute(string dllName)
        {
            Value = dllName;
        }

        /// <summary>
        /// Gets the name of the DLL file that contains the entry point.
        /// </summary>
        /// <returns>
        /// The name of the DLL file that contains the entry point.
        /// </returns>
        public string Value { get; }
    }
}