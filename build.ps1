# Build and Run Script for AI Merge Tool
# This script builds the project and optionally runs it with test files

param(
    [switch]$Release,
    [switch]$Run,
    [switch]$Test,
    [switch]$Pack,
    [string]$OutputPath = ""
)

$ErrorActionPreference = "Stop"

Write-Host "AI Merge Tool - Build Script" -ForegroundColor Cyan
Write-Host "==============================" -ForegroundColor Cyan
Write-Host ""

# Determine build configuration
$Configuration = if ($Release) { "Release" } else { "Debug" }
Write-Host "Build Configuration: $Configuration" -ForegroundColor Yellow

# Build the solution
Write-Host ""
Write-Host "Building solution..." -ForegroundColor Green
dotnet build AiMergeTool.sln -c $Configuration

if ($LASTEXITCODE -ne 0) {
    Write-Host "Build failed!" -ForegroundColor Red
    exit 1
}

Write-Host "Build succeeded!" -ForegroundColor Green

# Determine executable path
$ExePath = "AiMergeTool\bin\$Configuration\net8.0-windows\AiMergeTool.exe"

if ($OutputPath -ne "") {
    Write-Host ""
    Write-Host "Copying to output path: $OutputPath" -ForegroundColor Yellow
    
    $OutputDir = Split-Path -Parent $OutputPath
    if ($OutputDir -and !(Test-Path $OutputDir)) {
        New-Item -ItemType Directory -Path $OutputDir -Force | Out-Null
    }
    
    Copy-Item -Path $ExePath -Destination $OutputPath -Force
    Copy-Item -Path "AiMergeTool\bin\$Configuration\net8.0-windows\*.dll" -Destination (Split-Path -Parent $OutputPath) -Force
    Copy-Item -Path "AiMergeTool\bin\$Configuration\net8.0-windows\appsettings.json" -Destination (Split-Path -Parent $OutputPath) -Force
    
    Write-Host "Files copied successfully!" -ForegroundColor Green
}

# Run with test files if requested
if ($Test) {
    Write-Host ""
    Write-Host "Running with test files..." -ForegroundColor Green
    Write-Host "Command: $ExePath TestFiles\base.cs TestFiles\ours.cs TestFiles\theirs.cs TestFiles\merged.cs" -ForegroundColor Gray
    
    if (!(Test-Path $ExePath)) {
        Write-Host "Error: Executable not found at $ExePath" -ForegroundColor Red
        exit 1
    }
    
    & $ExePath "TestFiles\base.cs" "TestFiles\ours.cs" "TestFiles\theirs.cs" "TestFiles\merged.cs"
}
elseif ($Run) {
    Write-Host ""
    Write-Host "To run the application, use one of these commands:" -ForegroundColor Yellow
    Write-Host ""
    Write-Host "  $ExePath <base> <local> <remote> <merged>" -ForegroundColor Cyan
    Write-Host ""
    Write-Host "Or run with test files:" -ForegroundColor Yellow
    Write-Host ""
    Write-Host "  .\build.ps1 -Test" -ForegroundColor Cyan
    Write-Host ""
}

# Create NuGet package if requested
if ($Pack) {
    Write-Host ""
    Write-Host "Creating NuGet package..." -ForegroundColor Green
    
    $PackageOutput = ".\nupkgs"
    dotnet pack AiMergeTool\AiMergeTool.csproj -c $Configuration -o $PackageOutput
    
    if ($LASTEXITCODE -ne 0) {
        Write-Host "Pack failed!" -ForegroundColor Red
        exit 1
    }
    
    Write-Host "Package created successfully!" -ForegroundColor Green
    Write-Host "Package location: $PackageOutput" -ForegroundColor Gray
    
    Get-ChildItem $PackageOutput -Filter "*.nupkg" | ForEach-Object {
        Write-Host "  - $($_.Name)" -ForegroundColor Cyan
    }
}

Write-Host ""
Write-Host "Build complete!" -ForegroundColor Green
Write-Host "Executable location: $ExePath" -ForegroundColor Gray
