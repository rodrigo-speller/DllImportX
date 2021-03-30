#!/usr/bin/env pwsh

using namespace System
using namespace System.IO
using namespace System.Runtime.InteropServices

param (
    [ValidateSet('Windows', 'Linux')]
    [String] $TargetPlatform = $null,
    
    [ValidateSet('x86', 'x64')]
    [String] $TargetArchitecture
)

$ErrorActionPreference = "Stop"

$scriptDirectory        = [Path]::GetDirectoryName($script:MyInvocation.MyCommand.Path)
$projectDirectory       = [Path]::GetDirectoryName($scriptDirectory)
$sourceFile             = [Path]::Combine($projectDirectory, 'Sample.cpp')
$binDir                 = [Path]::Combine($projectDirectory, 'obj', 'sample-lib')
$win32Dir               = [Path]::Combine($binDir, 'win-x86')
$win64Dir               = [Path]::Combine($binDir, 'win-x64')
$linux32Dir             = [Path]::Combine($binDir, 'linux-x86')
$linux64Dir             = [Path]::Combine($binDir, 'linux-x64')

function main() {
    if ([String]::IsNullOrEmpty($TargetPlatform)) {
        $TargetPlatform = getCurrentOS
    }

    switch ($TargetPlatform) {
        'Windows' { buildWindows }
        'Linux' { buildLinux }
        default { throw "Operational system not supported ($TargetPlatform)!" }
    }
}

function buildLinux() {
    $targetArch     = $TargetArchitecture
    $gcc            = getGccCommand

    # target achitecture

    if ([String]::IsNullOrEmpty($targetArch)) {
        $targetArch = getCurrentOSArchitecture
    }

    switch ($targetArch) {
        'x86' { $outputDir = $linux32Dir; $m = '-m32' }
        'x64' { $outputDir = $linux64Dir; $m = '-m64' }
        Default { throw "Target architecture not supported ('$targetArch')." }
    }

    # gcc

    if ($null -eq $gcc) {
        throw 'GNU Compiler Collection (gcc or g++) not found! Install these tools to build.'
    }

    # build

    Write-Output "Building..."
    Write-Output "Platform: Linux ($targetArch)"

    mkdir $binDir
    mkdir $outputDir

    # gcc -Wall -c $sourceFile -o "$binDir/Sample.o"
    & $gcc -shared $m -fpermissive -o "$outputDir/DllImportX.Sample.so" $sourceFile
}

function buildWindows() {
    $vcexec         = [Path]::Combine($scriptDirectory, 'vcexec.bat')
    $targetArch     = $TargetArchitecture
    $vcvarsall      = getVCVarsAllPath

    # target achitecture

    if ([String]::IsNullOrEmpty($targetArch)) {
        # VS Command Prompt target architecture
        $targetArch = $env:VSCMD_ARG_TGT_ARCH

        if ([String]::IsNullOrEmpty($targetArch)) {
            $targetArch = getCurrentOSArchitecture
        }
    }

    switch ($targetArch) {
        'x86' { $outputDir = $win32Dir }
        'x64' { $outputDir = $win64Dir }
        Default { throw "Target architecture not supported ('$targetArch')." }
    }

    # vcvarsall

    if ([String]::IsNullOrEmpty($vcvarsall)) {
        throw 'Microsoft C++ toolset (vcvarsall.bat) not found! Install these tools to build. Read more on: https://docs.microsoft.com/pt-br/cpp/build/building-on-the-command-line?view=msvc-160.'
    }

    # build

    Write-Output "Building..."
    Write-Output "Platform: Windows ($targetArch)"

    mkdir $binDir
    mkdir $outputDir

    # injects vcvarsall path as environment variable to vcexec
    $env:__DLLIMPORTX_VCVARSALL__ = $vcvarsall

    & $vcexec $targetArch cl /EHsc /c $sourceFile /permissive /Fo"$outputDir/DllImportX.Sample.obj"
    & $vcexec $targetArch link /DLL "/OUT:$outputDir/DllImportX.Sample.dll" "$outputDir/DllImportX.Sample.obj" ole32.lib
}

function mkdir([String] $path) {
    if (![Directory]::Exists($path))
    {
        [void][Directory]::CreateDirectory($path)
    }
}

function getGccCommand() {
    $gcc = Get-Command 'gcc' -CommandType Application -ErrorAction Ignore | Select-Object -First 1

    if ($null -eq $gcc) {
        $gcc = Get-Command 'g++' -CommandType Application -ErrorAction Ignore | Select-Object -First 1
    }

    return $gcc;
}

function getVCVarsAllPath() {
    $programFilesX86 = [Environment]::GetFolderPath('ProgramFilesX86')
    $vswhere = [Path]::Combine($programFilesX86, 'Microsoft Visual Studio', 'Installer', 'vswhere.exe')

    if (![File]::Exists($vswhere)) {
        return $null;
    }

    $data = (& $vswhere -format json | ConvertFrom-Json) |
        Sort-Object { [Version]$_.installationVersion } -Descending |
        ForEach-Object {
            $vcvarsall = [Path]::Combine($_.installationPath, 'VC', 'Auxiliary', 'Build', 'vcvarsall.bat');
            if ([File]::Exists($vcvarsall)) {
                return $vcvarsall;
            }
            return $null;
        }

    $data
}

function getCurrentOS() {
    $runtimeInfo = [System.Runtime.InteropServices.RuntimeInformation, mscorlib];
    $osPlatform = [System.Runtime.InteropServices.OSPlatform, mscorlib];

    if ($runtimeInfo::IsOSPlatform($osPlatform::Linux)) {
        return "Linux"
    }
    
    if ($runtimeInfo::IsOSPlatform($osPlatform::Windows)) {
        return "Windows"
    }
    
    if ($runtimeInfo::IsOSPlatform($osPlatform::OSX)) {
        return "MacOS"
    }
    
    if ($PSVersion -lt '6.0') {
        return 'Windows'
    }

    return 'NotDetected'
}

function getCurrentOSArchitecture() {
    if ([Environment]::Is64BitOperatingSystem) {
        return 'x64'
    } else {
        return 'x86'
    }

    return 'NotDetected'
}

main