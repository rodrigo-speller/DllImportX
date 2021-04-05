# DllImportX Sample - Native Libraries

*DllImportX Sample - Native Libraries* is a pre-compiled sample library collection to be consumed in the tests.

To see the list of exported functions, see [Samples.h](Samples.h) file.

The pre-compiled files are in `assets/sample-lib` directory.

## Pre-requisites to build on Windows

Required tools to build on Windows:

* Microsft Visual Studio 2017 (or above) with Microsoft C++ toolset

## Pre-requisites to build on Linux

Required tools to build on Linux:

* PowerShell (pwsh)
* GNU Compiler Collection (gcc)
* GNU C compiler (multilib files) (gcc-multilib)
* GNU C++ compiler (multilib files) (g++-multilib)

## How to build

To build the native library execute the `build/build.ps1` script.
```
build/build.ps1 [ -TargetPlatform ( Windows | Linux ) ] [ -TargetArchitecture ( x86 | x64 ) ]
```
