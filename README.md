# DllImportX

DllImportX is an attribute to work around the classic DllImport's limitations.

The .Net platform does not allow to define dynamic values in the attributes
assignments to perform environment based unmanaged library linking.

With DllImporX, you can dynamically set the DllImport attribute at runtime
changing everyone attributes as necessary!

DllImportX's attribute supports all the classic DllImport parameters.
All you need is add the 'X' in the DllImport attribute and build your instance.

# Get DllImportX

The quickest way to get the latest release of DllImportX is to add it to your
project using NuGet (https://www.nuget.org/packages/DllImportX)

# How to use

If you have an unmanaged static-link library with this functions:

```c
// sample.h
// Unmanaged C sample functions

void uVoidSample();
int cIntSample(int intParam);
void cOutIntSample(int* outIntParam);
void cRefIntSample(int* refIntParam);
void cAnsiStringSample(const char* str);
void cPtrSample(void* ptr);
```

You must only define an interface in your project:

```csharp
// ISample.cs

using System;
using System.Runtime.InteropServices;

namespace DllImportXSample
{
    public interface ISample
    {
        [DllImportX("Sample.dll")]
        void cVoidSample();

        [DllImportX("Sample.dll")]
        int cIntSample(int intParam);

        [DllImportX("Sample.dll", EntryPoint = "cOutIntSample")]
        void OutIntSample(out int outIntParam);

        [DllImportX("Sample.dll", EntryPoint = "cRefIntSample"))]
        void cRefIntSample(ref int refIntParam);

        [DllImportX("Sample.dll", EntryPoint = "cAnsiString"))]
        void AnsiStringSample([In][MarshalAs(UnmanagedType.LPStr)] string str);

        [DllImportX("Sample.dll")]
        void cPtrSample(IntPtr ptr);
    }
}
```

And build your instance!

```csharp
using System.Runtime.InteropServices;
using DllImportXSample;

// ...
    
var sampleInstance = DllImportXFactory
    .Build<ISample>(entry => {
        // Change the path to the unmanged static-link library
        // based in the runtime enviroment architeture
        entry.DllName = (IntPtr.Size == 8 ? "x64/" : "x86/") + entry.DllName;
    });

// ready

sampleInstance.AnsiStringSample("Hello world.");

// ...
```
# Bug Reports
If you find any bugs, please report them using the GitHub issue tracker.

# License
This software is distributed under the terms of the MIT license
(see [LICENSE.txt](LICENSE.txt)).