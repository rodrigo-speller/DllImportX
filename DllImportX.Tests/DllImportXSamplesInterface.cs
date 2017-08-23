using System;
using System.Runtime.InteropServices;

namespace DllImportX.Tests.Sample01
{
    public interface DllImportXSamplesInterface
    {
        [DllImportX("DllImportX.Tests.Samples.dll")]
        void Void();

        [DllImportX("DllImportX.Tests.Samples.dll")]
        int Int();

        [DllImportX("DllImportX.Tests.Samples.dll", CallingConvention = CallingConvention.Cdecl)]
        int IntInt([In] int i);

        [DllImportX("DllImportX.Tests.Samples.dll", CallingConvention = CallingConvention.Cdecl)]
        int IntOutInt([Out] out int i);

        [DllImportX("DllImportX.Tests.Samples.dll", CallingConvention = CallingConvention.Cdecl)]
        int IntRefInt([In][Out] ref int i);

        [DllImportX("DllImportX.Tests.Samples.dll", CallingConvention = CallingConvention.Cdecl)]
        IntPtr RefIntInt([In] int i);

        [DllImportX("DllImportX.Tests.Samples.dll", CallingConvention = CallingConvention.Cdecl)]
        IntPtr Free([In] IntPtr ptr);

        [DllImportX("DllImportX.Tests.Samples.dll", CallingConvention = CallingConvention.Cdecl, ThrowOnUnmappableChar = true)]
        int IntAnsiString([In][MarshalAs(UnmanagedType.LPStr)] string str);

        [DllImportX("DllImportX.Tests.Samples.dll", CallingConvention = CallingConvention.Cdecl, ThrowOnUnmappableChar = true)]
        int IntUnicodeString([In][MarshalAs(UnmanagedType.LPWStr)] string str);

        [DllImportX("DllImportX.Tests.Samples.dll", CallingConvention = CallingConvention.Cdecl, ThrowOnUnmappableChar = true)]
        [return: MarshalAs(UnmanagedType.U4)]
        int IntRefAnsiString([In][Out][MarshalAs(UnmanagedType.LPStr)] ref string str);

        [DllImportX("DllImportX.Tests.Samples.dll", CallingConvention = CallingConvention.Cdecl, ThrowOnUnmappableChar = true)]
        [return: MarshalAs(UnmanagedType.U4)]
        int IntRefUnicodeString([In][Out][MarshalAs(UnmanagedType.LPWStr)] ref string str);
    }
}
