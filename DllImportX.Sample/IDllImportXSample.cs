using System;
using System.Runtime.InteropServices;

namespace DllImportX.Sample
{
    public interface IDllImportXSample
    {
        [DllImportX("DllImportX.Sample")]
        void Void();

        [DllImportX("DllImportX.Sample")]
        int Int();

        [DllImportX("DllImportX.Sample", CallingConvention = CallingConvention.Cdecl)]
        int IntInt([In] int i);

        [DllImportX("DllImportX.Sample", CallingConvention = CallingConvention.Cdecl)]
        int IntOutInt([Out] out int i);

        [DllImportX("DllImportX.Sample", CallingConvention = CallingConvention.Cdecl)]
        int IntRefInt([In][Out] ref int i);

        [DllImportX("DllImportX.Sample", CallingConvention = CallingConvention.Cdecl)]
        IntPtr RefIntInt([In] int i);

        [DllImportX("DllImportX.Sample", CallingConvention = CallingConvention.Cdecl)]
        IntPtr Free([In] IntPtr ptr);

        [DllImportX("DllImportX.Sample", CallingConvention = CallingConvention.Cdecl, ThrowOnUnmappableChar = true)]
        int IntAnsiString([In][MarshalAs(UnmanagedType.LPStr)] string str);

        [DllImportX("DllImportX.Sample", CallingConvention = CallingConvention.Cdecl, ThrowOnUnmappableChar = true)]
        int IntUnicodeString([In][MarshalAs(UnmanagedType.LPWStr)] string str);

        [DllImportX("DllImportX.Sample", CallingConvention = CallingConvention.Cdecl, ThrowOnUnmappableChar = true)]
        [return: MarshalAs(UnmanagedType.U4)]
        int IntRefAnsiString([In][Out][MarshalAs(UnmanagedType.LPStr)] ref string str);

        [DllImportX("DllImportX.Sample", CallingConvention = CallingConvention.Cdecl, ThrowOnUnmappableChar = true)]
        [return: MarshalAs(UnmanagedType.U4)]
        int IntRefUnicodeString([In][Out][MarshalAs(UnmanagedType.LPWStr)] ref string str);
    }
}
