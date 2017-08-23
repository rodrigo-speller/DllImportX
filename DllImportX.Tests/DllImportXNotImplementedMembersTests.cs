using System;
using System.Runtime.InteropServices;
using Xunit;

namespace DllImportX.Tests
{
    [Trait("Category", "NotImplementedMembers > DllImportX")]
    public class DllImportXNotImplementedMembersTests
    {
        private static DllImportXNotImplementedMembersInterface Proxy
            = DllImportXFactory.Build<DllImportXNotImplementedMembersInterface>();

        [Fact]
        public void NotImplementedVoid()
        {
            Assert.Throws<NotImplementedException>(() => Proxy.NotImplementedVoid());
        }

        [Fact]
        public void NotImplementedPropertyIntGet()
        {
            Assert.Throws<NotImplementedException>(() =>
            {
                var i = Proxy.NotImplementedPropertyInt;
            });
        }

        [Fact]
        public void NotImplementedPropertyIntSet()
        {
            Assert.Throws<NotImplementedException>(() => Proxy.NotImplementedPropertyInt = 1);
        }
    }
}
