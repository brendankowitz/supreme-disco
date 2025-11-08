@echo off
REM Quick test script for AI Merge Tool
REM This script runs the tool with the sample test files

setlocal

echo =======================================
echo AI Merge Tool - Quick Test
echo =======================================
echo.

REM Check if executable exists
set EXE_PATH=AiMergeTool\bin\Debug\net8.0-windows\AiMergeTool.exe
if not exist "%EXE_PATH%" (
    set EXE_PATH=AiMergeTool\bin\Release\net8.0-windows\AiMergeTool.exe
)

if not exist "%EXE_PATH%" (
    echo Error: AiMergeTool.exe not found!
    echo Please build the project first:
    echo   dotnet build AiMergeTool.sln
    echo.
    pause
    exit /b 1
)

echo Found executable: %EXE_PATH%
echo.

REM Check for API key
if "%OPENAI_API_KEY%"=="" (
    echo WARNING: OPENAI_API_KEY environment variable is not set!
    echo The AI resolution feature will not work.
    echo.
    echo To set it, run:
    echo   set OPENAI_API_KEY=your-api-key-here
    echo.
    echo You can still use the tool to view and manually edit the merge.
    echo.
    pause
)

echo Running with test files...
echo Command: %EXE_PATH% TestFiles\base.cs TestFiles\ours.cs TestFiles\theirs.cs TestFiles\merged.cs
echo.

"%EXE_PATH%" TestFiles\base.cs TestFiles\ours.cs TestFiles\theirs.cs TestFiles\merged.cs

echo.
echo Test completed!
pause
