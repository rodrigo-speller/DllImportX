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
        int IntInt(int i);

        [DllImportX("DllImportX.Tests.Samples.dll", CallingConvention = CallingConvention.Cdecl)]
        int IntOutInt(out int i);

        [DllImportX("DllImportX.Tests.Samples.dll", CallingConvention = CallingConvention.Cdecl)]
        int IntRefInt(ref int i);

        [DllImportX("DllImportX.Tests.Samples.dll", CallingConvention = CallingConvention.Cdecl)]
        IntPtr RefIntInt(int i);

        [DllImportX("DllImportX.Tests.Samples.dll", CallingConvention = CallingConvention.Cdecl)]
        IntPtr Free(IntPtr ptr);

        [DllImportX("DllImportX.Tests.Samples.dll", CallingConvention = CallingConvention.Cdecl)]
        int IntAnsiString(string str);

        [DllImportX("DllImportX.Tests.Samples.dll", CallingConvention = CallingConvention.Cdecl)]
        int IntRefAnsiString(ref string str);
    }
}
