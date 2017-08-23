using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text;
using Xunit;

namespace DllImportX.Tests.Sample01
{
    [Trait("Category", "Sample01 > DllImportX")]
    public class DllImportXTests
    {
        DllImportXSamplesInterface Proxy = DllImportXFactory.Build<DllImportXSamplesInterface>(x =>
        {
            x.DllName = (IntPtr.Size == 8 ? "x64/" : "x86/") + x.DllName;
        });

        [Fact]
        public void Void()
        {

            Proxy.Void();
        }

        [Fact]
        public void Int()
        {
            Assert.Equal(-1, Proxy.Int());
        }

        [Fact]
        public void IntInt()
        {
            var random = new Random();
            var r = random.Next();
            var i = Proxy.IntInt(r);
            Assert.Equal(~r, i);
        }

        [Fact]
        public void IntOutInt()
        {
            var ret = Proxy.IntOutInt(out var param);
            Assert.Equal(~param, ret);
        }

        [Fact]
        public void IntRefInt()
        {
            var random = new Random();
            var value = random.Next();
            int param = value;

            var ret = Proxy.IntRefInt(ref param);

            Assert.Equal(value, ret);
            Assert.Equal(value, ~param);
        }

        [Fact]
        public void RefIntInt()
        {
            var random = new Random();
            var value = random.Next();

            var ptr = Proxy.RefIntInt(value);
            int ret;
            try
            {
                ret = Marshal.ReadInt32(ptr);
            }
            finally
            {
                Proxy.Free(ptr);
            }

            Assert.Equal(value, ret);
        }

        [Fact]
        public void IntAnsiString()
        {
            var str = "Hello World!";

            var md5 = System.Security.Cryptography.MD5.Create();
            var hash = BitConverter.ToInt32(md5.ComputeHash(Encoding.ASCII.GetBytes(str)), 0);

            var byStr = Proxy.IntAnsiString(str);
            Assert.Equal(hash, byStr);

            var byBuffer = Proxy.IntRefAnsiString(ref str);
            Assert.Equal(hash, byBuffer);
            Assert.Equal(hash.ToString(CultureInfo.InvariantCulture), str);
        }
    }
}
