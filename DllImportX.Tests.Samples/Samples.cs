using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text;

namespace DllImportX.Tests.Samples
{
    public static class Samples
    {
        [DllExport]
        public static void Void() { }

        [DllExport]
        public static int Int() => -1;

        [DllExport]
        public static int IntInt(int i) => ~i;

        [DllExport]
        public static int IntOutInt(out int i)
        {
            var rand = new Random();
            i = rand.Next();
            return ~i;
        }

        [DllExport]
        public static int IntRefInt(ref int i)
        {
            var ret = i;
            i = ~i;
            return ret;
        }

        [DllExport]
        public static IntPtr RefIntInt(int i)
        {

            var ret = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(int)));
            Marshal.WriteInt32(ret, i);
            return ret;
        }

        [DllExport]
        public static int IntAnsiString(string str)
        {
            var md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            return BitConverter.ToInt32(md5.ComputeHash(Encoding.ASCII.GetBytes(str)), 0);
        }

        [DllExport]
        public static int IntUnicodeString([MarshalAs(UnmanagedType.LPWStr)] string str)
        {
            var md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            return BitConverter.ToInt32(md5.ComputeHash(Encoding.UTF8.GetBytes(str)), 0);
        }

        [DllExport]
        public static int IntRefAnsiString(ref string str)
        {
            var md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            var i = BitConverter.ToInt32(md5.ComputeHash(Encoding.ASCII.GetBytes(str)), 0);
            str = i.ToString(CultureInfo.InvariantCulture);
            return i;
        }

        [DllExport]
        public static int IntRefUnicodeString([MarshalAs(UnmanagedType.LPWStr)] ref string str)
        {
            var md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            var i = BitConverter.ToInt32(md5.ComputeHash(Encoding.UTF8.GetBytes(str)), 0);
            str = i.ToString(CultureInfo.InvariantCulture);
            return i;
        }

        [DllExport]
        public static void Free(IntPtr ptr)
        {
            Marshal.FreeHGlobal(ptr);
        }
    }
}
