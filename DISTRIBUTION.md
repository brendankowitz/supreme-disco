# Distribution Guide for AI Merge Tool

## Overview

While WPF applications cannot be distributed as `dotnet tool` packages due to platform-specific requirements, AI Merge Tool can be distributed and installed in several convenient ways.

## Distribution Methods

### 1. NuGet Package (Recommended for Sharing)

The project is configured to be packaged as a NuGet package for easy distribution.

#### Creating the Package

```bash
dotnet pack AiMergeTool/AiMergeTool.csproj -c Release -o ./nupkgs
```

This creates `AiMergeTool.1.0.0.nupkg` which contains:
- Compiled executable and dependencies
- README documentation
- Configuration files

#### Publishing to NuGet.org

```bash
dotnet nuget push nupkgs/AiMergeTool.1.0.0.nupkg --api-key YOUR_API_KEY --source https://api.nuget.org/v3/index.json
```

#### Installing from NuGet

Once published, users can download the package:

```bash
dotnet add package AiMergeTool
```

Or install globally as a NuGet package reference and access the tools folder.

### 2. GitHub Releases (Recommended for End Users)

Create a release on GitHub with pre-built binaries:

1. Build the Release version:
   ```bash
   dotnet publish AiMergeTool/AiMergeTool.csproj -c Release -r win-x64 --self-contained false -o publish/
   ```

2. Create a ZIP file:
   ```bash
   zip -r AiMergeTool-1.0.0-win-x64.zip publish/
   ```

3. Upload to GitHub Releases

Users can then:
- Download the ZIP
- Extract to a folder
- Add to PATH or use directly
- Configure git mergetool to use the executable path

### 3. Chocolatey Package (Windows)

For Windows users, create a Chocolatey package:

1. Create a `aimergetool.nuspec` file
2. Package with `choco pack`
3. Publish to Chocolatey repository

Users install via:
```bash
choco install aimergetool
```

### 4. Manual Distribution

Simply share the build output:

```bash
dotnet build -c Release AiMergeTool/AiMergeTool.csproj
```

Share the contents of `AiMergeTool/bin/Release/net8.0-windows/`

## Why Not `dotnet tool`?

The `dotnet tool` infrastructure does not support:
- Applications targeting `net8.0-windows` (requires platform-specific TFM)
- WPF applications (UseWPF=true)
- Windows Forms applications (UseWindowsForms=true)

These are fundamental limitations of the .NET tool packaging system, which is designed for cross-platform console applications.

## Recommended Installation Instructions for Users

### Option 1: From NuGet Package

```bash
# Download the package
dotnet new console -n temp
cd temp
dotnet add package AiMergeTool
# The executable will be in packages/aimergetool/1.0.0/tools/net8.0-windows/
```

### Option 2: From GitHub Release (Simplest)

1. Go to [Releases](https://github.com/brendankowitz/supreme-disco/releases)
2. Download the latest `AiMergeTool-win-x64.zip`
3. Extract to `C:\Tools\AiMergeTool\`
4. Add to PATH or use full path in git config

### Option 3: Build from Source

```bash
git clone https://github.com/brendankowitz/supreme-disco.git
cd supreme-disco
dotnet build -c Release AiMergeTool.sln
# Executable at: AiMergeTool/bin/Release/net8.0-windows/AiMergeTool.exe
```

## Git Integration After Installation

Regardless of installation method, configure git to use the tool:

```bash
# Using full path
git config --global mergetool.aimergetool.cmd 'C:/Tools/AiMergeTool/AiMergeTool.exe "$BASE" "$LOCAL" "$REMOTE" "$MERGED"'

# Or if in PATH
git config --global mergetool.aimergetool.cmd 'AiMergeTool.exe "$BASE" "$LOCAL" "$REMOTE" "$MERGED"'

git config --global merge.tool aimergetool
```

## Package Metadata

The NuGet package includes:
- **Package ID**: AiMergeTool
- **Version**: 1.0.0
- **Authors**: Brendan Kowitz
- **License**: MIT
- **Tags**: git, merge, diff, ai, openai, azure, wpf, mergetool
- **Repository**: https://github.com/brendankowitz/supreme-disco

## Future Enhancements

Potential distribution improvements:
- Windows Installer (MSI/MSIX)
- Chocolatey package
- WinGet package
- Scoop package
- Automated GitHub Release builds via CI/CD
