using System;
using System.IO;
using System.Runtime.InteropServices;

namespace DllImportX.Sample
{
    public static class DllImportXSample
    {
        public static IDllImportXSample Create()
        {
            return DllImportXFactory.Build<IDllImportXSample>(x =>
            {
                x.DllName = ResolveLibraryPath(x.DllName);
            });
        }

        public static IDllImportXSample CreateWithIgnoreAttributes()
        {
            return DllImportXFactory.Build<IDllImportXSample>(x =>
            {
                x.IgnoreAttributes = true;
                x.DllName = ResolveLibraryPath(x.DllName);
            });
        }

        private static string ResolveLibraryPath(string name)
        {
            var thisDllPath = typeof(DllImportXSample).Assembly.Location;
            var binDirectory = Path.GetDirectoryName(thisDllPath);

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                if (Environment.Is64BitProcess)
                {
                    return Path.Combine(binDirectory, "sample-lib", "linux-x64", $"{name}.so");
                }
                else
                {
                    return Path.Combine(binDirectory, "sample-lib", "linux-x86", $"{name}.so");
                }
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                if (Environment.Is64BitProcess)
                {
                    return Path.Combine(binDirectory, "sample-lib", "win-x64", $"{name}.dll");
                }
                else
                {
                    return Path.Combine(binDirectory, "sample-lib", "win-86", $"{name}.dll");
                }
            }

            throw new InvalidOperationException();
        }

    }
}
