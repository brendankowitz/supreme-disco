# Usage Guide - AI Merge Tool

This guide provides detailed instructions on how to use the AI Merge Tool.

## Table of Contents
1. [Installation](#installation)
2. [Configuration](#configuration)
3. [Command-Line Usage](#command-line-usage)
4. [Git Integration](#git-integration)
5. [User Interface](#user-interface)
6. [Troubleshooting](#troubleshooting)

## Installation

### Prerequisites
- Windows 10 or later (WPF application)
- .NET 8.0 Runtime or SDK
- OpenAI API key or Azure OpenAI service endpoint

### Building from Source

```bash
git clone https://github.com/brendankowitz/supreme-disco.git
cd supreme-disco
dotnet build AiMergeTool.sln
```

The executable will be in: `AiMergeTool/bin/Debug/net8.0-windows/AiMergeTool.exe`

For release build:
```bash
dotnet build -c Release AiMergeTool.sln
```

The release executable will be in: `AiMergeTool/bin/Release/net8.0-windows/AiMergeTool.exe`

## Configuration

### OpenAI Configuration

Set environment variables before running the application:

```cmd
set OPENAI_API_KEY=sk-your-api-key-here
set OPENAI_MODEL=gpt-4
set OPENAI_IS_AZURE=false
```

### Azure OpenAI Configuration

```cmd
set OPENAI_ENDPOINT=https://your-resource.openai.azure.com/
set OPENAI_API_KEY=your-azure-api-key
set OPENAI_DEPLOYMENT=gpt-4
set OPENAI_IS_AZURE=true
```

### Configuration File (Optional)

Alternatively, edit `appsettings.json` in the application directory:

```json
{
  "AiConfiguration": {
    "IsAzure": true,
    "Endpoint": "https://your-resource.openai.azure.com/",
    "ApiKey": "your-api-key-here",
    "DeploymentName": "gpt-4",
    "Model": "gpt-4",
    "MaxTokens": 4000
  }
}
```

**Note:** Environment variables take precedence over `appsettings.json`.

## Command-Line Usage

### Standard Usage

The tool accepts both positional and named arguments, compatible with most merge tools.

#### Positional Arguments (git mergetool compatible)
```cmd
AiMergeTool.exe <base> <local> <remote> <merged>
```

Example:
```cmd
AiMergeTool.exe base.cs ours.cs theirs.cs merged.cs
```

#### Named Arguments
```cmd
AiMergeTool.exe -b <base> -l <local> -r <remote> -m <merged>
```

Example:
```cmd
AiMergeTool.exe --base base.cs --local ours.cs --remote theirs.cs --merged merged.cs
```

### Arguments

| Argument | Short | Long | Description |
|----------|-------|------|-------------|
| Base | `-b` | `--base` | Common ancestor file (optional) |
| Local | `-l` | `--local` | Your changes (ours) |
| Remote | `-r` | `--remote` | Incoming changes (theirs) |
| Merged | `-m` | `--merged` | Output file path |
| | `-o` | `--output` | Alternative to merged |

**Note:** Local and Remote are required. Base is optional but recommended for better AI resolution.

## Git Integration

### Configure as Git Mergetool

Add the tool as a custom mergetool in git:

```bash
# Set the tool name
git config --global merge.tool aimergetool

# Configure the command
git config --global mergetool.aimergetool.cmd 'AiMergeTool.exe "$BASE" "$LOCAL" "$REMOTE" "$MERGED"'

# Don't trust exit code (WPF apps don't always return proper codes)
git config --global mergetool.aimergetool.trustExitCode false

# Optionally, don't prompt before each invocation
git config --global mergetool.prompt false
```

### Using During a Merge

When you encounter a merge conflict:

```bash
# Start merge
git merge feature-branch

# If conflicts occur, launch the merge tool
git mergetool

# After resolving, complete the merge
git commit
```

### Alternative: Direct Invocation

For a specific file:
```bash
git mergetool --tool=aimergetool path/to/conflicted-file.cs
```

## User Interface

### Layout

The UI consists of three main panes arranged horizontally:

```
┌─────────────┬──────────────┬──────────────┐
│   OURS      │   MERGED     │   THEIRS     │
│  (Local)    │  (Editable)  │  (Remote)    │
│   Blue      │   Green      │    Red       │
│  Read-Only  │   Editable   │  Read-Only   │
└─────────────┴──────────────┴──────────────┘
```

#### Left Pane (Blue) - OURS
- Shows your local changes
- Read-only view
- Syntax highlighted

#### Center Pane (Green) - MERGED
- Shows the merged result
- **Fully editable** - you can manually edit the merge
- Initially contains your local version
- Updated when you click "Resolve with AI"

#### Right Pane (Red) - THEIRS
- Shows incoming changes
- Read-only view
- Syntax highlighted

### Toolbar Actions

#### 🤖 Resolve with AI
- Sends the conflict context to the configured AI service
- AI analyzes both changes and suggests an intelligent merge
- Merged pane is updated with the AI suggestion
- You can edit the suggestion before saving

#### 💾 Save
- Saves the content of the MERGED pane to the output file
- If no output path was specified, prompts for save location
- Creates a backup of existing file (if any)

#### ✖ Exit
- Closes the application
- Does not automatically save changes
- Prompts if unsaved changes exist (future enhancement)

### Status Bar

Shows current operation status:
- "Ready" - Waiting for user action
- "Resolving with AI..." - AI is processing
- "AI resolution complete" - AI has provided a suggestion
- "Saved to <path>" - File was successfully saved
- "Error: <message>" - An error occurred

## Troubleshooting

### Common Issues

#### "Failed to initialize application: File not found"
- **Cause:** One or more input files don't exist
- **Solution:** Verify all file paths are correct and files exist

#### "Failed to get AI merge suggestion: Invalid API key"
- **Cause:** API key is not configured or is invalid
- **Solution:** 
  - Check environment variables are set correctly
  - Verify API key is valid
  - For Azure: Ensure endpoint URL is correct

#### "Failed to get AI merge suggestion: Quota exceeded"
- **Cause:** OpenAI API quota limit reached
- **Solution:** 
  - Check your OpenAI usage limits
  - Wait for quota reset
  - Upgrade your OpenAI plan

#### Application doesn't start
- **Cause:** .NET runtime not installed
- **Solution:** Install .NET 8.0 Runtime from https://dotnet.microsoft.com/download

#### Syntax highlighting not working
- **Cause:** File extension not recognized
- **Solution:** File type detection is automatic. Supported extensions include .cs, .js, .py, .java, etc.

### Getting Help

For issues, questions, or contributions:
- GitHub Issues: https://github.com/brendankowitz/supreme-disco/issues
- Documentation: See README.md files in repository

### Security Considerations

⚠️ **Important Security Notes:**

1. **Never commit API keys** to source control
2. **Use environment variables** for sensitive configuration
3. **Review AI suggestions** before saving - AI can make mistakes
4. **Keep your API keys secure** - rotate them regularly
5. **Use separate API keys** for development and production

### Best Practices

1. **Always review AI suggestions** - The AI is a tool to assist, not replace your judgment
2. **Test the merged result** - Compile and test after merging
3. **Use base file when available** - Provides better context for AI
4. **Start with simple conflicts** - Get familiar with the tool on simple merges first
5. **Keep backups** - Original files are not modified, but keep backups just in case

## Advanced Usage

### Batch Processing (Future Enhancement)

Currently, the tool processes one file at a time. For batch processing:

```bash
for %%f in (*.conflict) do (
    AiMergeTool.exe base-%%f ours-%%f theirs-%%f merged-%%f
)
```

### Custom Prompts (Future Enhancement)

Future versions may support custom AI prompts via configuration.

### Integration with Other Tools

The tool is designed to be compatible with:
- Git mergetool
- P4Merge (command-line compatible)
- KDiff3 (command-line compatible)
- Any tool that supports custom merge tools

## Performance Tips

1. **Use GPT-4** for complex merges, GPT-3.5 for simple ones
2. **Adjust MaxTokens** in configuration based on file size
3. **Pre-resolve simple conflicts** manually to reduce AI overhead
4. **Use base file** to improve AI accuracy and reduce token usage

## License & Credits

See LICENSE file for license information.

This tool uses:
- DiffPlex for diff computation
- AvalonEdit for code editing
- Azure.AI.OpenAI for AI integration
- CommandLineParser for argument parsing
