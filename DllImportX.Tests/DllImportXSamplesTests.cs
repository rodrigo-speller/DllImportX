using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text;
using Xunit;

namespace DllImportX.Tests.Sample01
{
    [Trait("Category", "DllImportXSamplesInterface (IgnoreAttributes)")]
    public class DllImportXTestsSimple : DllImportXTests
    {
        public DllImportXTestsSimple()
            : base(DllImportXFactory.Build<DllImportXSamplesInterface>(x =>
            {
                x.IgnoreAttributes = true;
                x.DllName = (IntPtr.Size == 8 ? "x64/" : "x86/") + x.DllName;
            }))
        { }
        
        public override void IntUnicodeString()
        {
            Assert.Throws<Xunit.Sdk.EqualException>(
                () => base.IntUnicodeString()
            );
        }

        public override void IntRefUnicodeString()
        {
            Assert.Throws<Xunit.Sdk.EqualException>(
                () => base.IntRefUnicodeString()
            );
        }
    }

    [Trait("Category", "DllImportXSamplesInterface")]
    public class DllImportXTestsStrict : DllImportXTests
    {
        public DllImportXTestsStrict()
            : base(DllImportXFactory.Build<DllImportXSamplesInterface>(x =>
            {
                x.DllName = (IntPtr.Size == 8 ? "x64/" : "x86/") + x.DllName;
            }))
        { }
    }

    public abstract class DllImportXTests
    {
        protected readonly DllImportXSamplesInterface Proxy;

        public DllImportXTests(DllImportXSamplesInterface proxy)
        {
            Proxy = proxy;
        }

        [Fact]
        public virtual void Void()
        {

            Proxy.Void();
        }

        [Fact]
        public virtual void Int()
        {
            Assert.Equal(-1, Proxy.Int());
        }

        [Fact]
        public virtual void IntInt()
        {
            var random = new Random();
            var r = random.Next();
            var i = Proxy.IntInt(r);
            Assert.Equal(~r, i);
        }

        [Fact]
        public virtual void IntOutInt()
        {
            var ret = Proxy.IntOutInt(out var param);
            Assert.Equal(~param, ret);
        }

        [Fact]
        public virtual void IntRefInt()
        {
            var random = new Random();
            var value = random.Next();
            int param = value;

            var ret = Proxy.IntRefInt(ref param);

            Assert.Equal(value, ret);
            Assert.Equal(value, ~param);
        }

        [Fact]
        public virtual void RefIntInt()
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
        public virtual void IntAnsiString()
        {
            var str = "Hello World!";

            var md5 = System.Security.Cryptography.MD5.Create();
            var hash = BitConverter.ToInt32(md5.ComputeHash(Encoding.ASCII.GetBytes(str)), 0);

            var byBuffer = Proxy.IntRefAnsiString(ref str);
            Assert.Equal(hash, byBuffer);
            Assert.Equal(hash.ToString(CultureInfo.InvariantCulture), str);
        }

        [Fact]
        public virtual void IntUnicodeString()
        {
            var str = "Hello World!";

            var md5 = System.Security.Cryptography.MD5.Create();
            var hash = BitConverter.ToInt32(md5.ComputeHash(Encoding.UTF8.GetBytes(str)), 0);

            var byStr = Proxy.IntUnicodeString(str);
            Assert.Equal(hash, byStr);
        }

        [Fact]
        public virtual void IntRefAnsiString()
        {
            var str = "Hello World!";

            var md5 = System.Security.Cryptography.MD5.Create();
            var hash = BitConverter.ToInt32(md5.ComputeHash(Encoding.ASCII.GetBytes(str)), 0);

            var byBuffer = Proxy.IntRefAnsiString(ref str);
            Assert.Equal(hash, byBuffer);
            Assert.Equal(hash.ToString(CultureInfo.InvariantCulture), str);
        }

        [Fact]
        public virtual void IntRefUnicodeString()
        {
            var str = "Hello World!";

            var md5 = System.Security.Cryptography.MD5.Create();
            var hash = BitConverter.ToInt32(md5.ComputeHash(Encoding.UTF8.GetBytes(str)), 0);

            var byBuffer = Proxy.IntRefUnicodeString(ref str);
            Assert.Equal(hash, byBuffer);
            Assert.Equal(hash.ToString(CultureInfo.InvariantCulture), str);
        }
    }
}
