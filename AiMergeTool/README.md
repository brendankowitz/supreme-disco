# AI Merge Tool

A stand-alone WPF application that uses OpenAI/Azure OpenAI to intelligently resolve merge conflicts with a graphical interface.

## Features

1. **AI-Powered Merge Resolution**: Connect to OpenAI or Azure OpenAI compatible interfaces to automatically suggest merge resolutions
2. **Command-Line Compatible**: Can be invoked from the command line with arguments similar to existing diff tools (p4merge, kdiff3, etc.)
3. **Three-Pane Graphical UI**: Shows ours/merged/theirs windows with syntax highlighting and diff visualization
4. **Save Functionality**: Saves resolved content to the specified output file
5. **Smart Diff for LLM**: Generates intelligent context for the AI to understand the merge conflict

## Requirements

- .NET 8.0 or later
- Windows OS (WPF application)
- OpenAI API key or Azure OpenAI endpoint

## Configuration

Configure the AI service using one of these methods:

### Option 1: Environment Variables

```bash
set OPENAI_API_KEY=your-api-key-here
set OPENAI_MODEL=gpt-4
set OPENAI_IS_AZURE=false
```

For Azure OpenAI:
```bash
set OPENAI_ENDPOINT=https://your-resource.openai.azure.com/
set OPENAI_API_KEY=your-api-key-here
set OPENAI_DEPLOYMENT=your-deployment-name
set OPENAI_IS_AZURE=true
```

### Option 2: appsettings.json

Edit `appsettings.json` in the application directory:

```json
{
  "AiConfiguration": {
    "IsAzure": false,
    "Endpoint": "https://your-resource.openai.azure.com/",
    "ApiKey": "your-api-key-here",
    "DeploymentName": "gpt-4",
    "Model": "gpt-4",
    "MaxTokens": 4000
  }
}
```

## Usage

### Command-Line

The tool can be invoked with positional arguments (compatible with git mergetool):

```bash
AiMergeTool.exe <base> <local> <remote> <merged>
```

Or with named arguments:

```bash
AiMergeTool.exe -b base.txt -l local.txt -r remote.txt -m merged.txt
```

Arguments:
- `base` / `-b` / `--base`: Base (common ancestor) file path
- `local` / `-l` / `--local`: Local (ours) file path
- `remote` / `-r` / `--remote`: Remote (theirs) file path  
- `merged` / `-m` / `--merged`: Merged output file path

### Git Integration

Configure as a git mergetool:

```bash
git config --global merge.tool aimergetool
git config --global mergetool.aimergetool.cmd 'AiMergeTool.exe "$BASE" "$LOCAL" "$REMOTE" "$MERGED"'
git config --global mergetool.aimergetool.trustExitCode false
```

Then use it during merges:

```bash
git mergetool
```

## Building

```bash
dotnet build AiMergeTool.sln
```

## Running

```bash
dotnet run --project AiMergeTool/AiMergeTool.csproj -- -l local.txt -r remote.txt -m merged.txt
```

## UI Overview

The application displays three editor panes:

- **Left (Blue)**: OURS (Local) - Your changes (read-only)
- **Center (Green)**: MERGED - Editable merge result
- **Right (Red)**: THEIRS (Remote) - Incoming changes (read-only)

### Toolbar Actions

- **🤖 Resolve with AI**: Uses AI to automatically suggest a merge resolution
- **💾 Save**: Saves the merged content to the output file
- **✖ Exit**: Closes the application

## Open Source Components

This project uses the following open-source libraries:

- **DiffPlex**: For computing text differences
- **AvalonEdit**: For syntax-highlighted code editing
- **CommandLineParser**: For parsing command-line arguments
- **Azure.AI.OpenAI**: For OpenAI/Azure OpenAI integration
- **Microsoft.Extensions.Configuration**: For configuration management

## License

See LICENSE file for details.

## Security Notes

- **Never commit API keys** to version control
- Use environment variables or secure configuration management for sensitive data
- The `.env` file is ignored by git (see `.gitignore`)
- Review AI suggestions before saving - AI can make mistakes

## Contributing

Contributions are welcome! Please feel free to submit issues or pull requests.
