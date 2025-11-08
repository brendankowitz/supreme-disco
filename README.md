# supreme-disco
AI Merge Tool - A WPF application for AI-assisted merge conflict resolution

## Overview

This repository contains an AI-powered merge tool built with WPF that helps developers resolve merge conflicts intelligently using OpenAI/Azure OpenAI. The tool provides a visual three-pane interface similar to popular merge tools like p4merge, semantic merge, or VSCode's git diff tool.

## Features

1. **AI-Powered Merge Resolution** - Connect to OpenAI or Azure OpenAI compatible interfaces to automatically suggest intelligent merge resolutions
2. **Command-Line Compatible** - Can be invoked from the command line with arguments similar to existing diff/merge tools (compatible with git mergetool)
3. **Three-Pane Graphical UI** - Shows ours (local), merged (editable), and theirs (remote) windows with syntax highlighting
4. **Save Functionality** - Saves resolved content to the specified output file
5. **Smart Diff for LLM** - Generates contextual diff information optimized for LLM understanding

## Project Structure

```
supreme-disco/
├── AiMergeTool/              # Main WPF application
│   ├── Models/               # Data models
│   ├── Services/             # Business logic services
│   ├── ViewModels/           # MVVM ViewModels
│   ├── Views/                # WPF Views (XAML)
│   ├── Utilities/            # Helper utilities
│   └── appsettings.json      # Configuration file
├── TestFiles/                # Sample test files
└── README.md                 # This file
```

## Quick Start

### Installation

#### Option 1: Build from Source
```bash
git clone https://github.com/brendankowitz/supreme-disco.git
cd supreme-disco
dotnet build -c Release AiMergeTool.sln
```

#### Option 2: From NuGet Package
```bash
dotnet pack AiMergeTool/AiMergeTool.csproj -c Release -o ./nupkgs
# Package will be available in ./nupkgs/AiMergeTool.1.0.0.nupkg
```

For more installation options, see [DISTRIBUTION.md](DISTRIBUTION.md).

**Note**: This is a WPF application and cannot be distributed as a `dotnet tool` due to platform-specific requirements. See [DISTRIBUTION.md](DISTRIBUTION.md) for alternative distribution methods.

### Prerequisites
- Windows OS (WPF application)
- .NET 8.0 SDK or later
- OpenAI API key or Azure OpenAI endpoint

### Building

```bash
dotnet build AiMergeTool.sln
```

**Windows PowerShell:**
```powershell
.\build.ps1
```

### Configuration

Set up your AI service credentials using environment variables:

**For OpenAI:**
```bash
set OPENAI_API_KEY=your-api-key-here
set OPENAI_MODEL=gpt-4
```

**For Azure OpenAI:**
```bash
set OPENAI_ENDPOINT=https://your-resource.openai.azure.com/
set OPENAI_API_KEY=your-api-key-here
set OPENAI_DEPLOYMENT=your-deployment-name
set OPENAI_IS_AZURE=true
```

See [.env.template](.env.template) for a complete configuration template.

### Testing

Run with sample test files:
```bash
test.bat
```

Or manually:
```bash
AiMergeTool.exe TestFiles\base.cs TestFiles\ours.cs TestFiles\theirs.cs TestFiles\merged.cs
```

### Running

```bash
AiMergeTool.exe <base> <local> <remote> <merged>
```

See [QUICKSTART.md](QUICKSTART.md) for a detailed quick start guide or [AiMergeTool/README.md](AiMergeTool/README.md) for complete usage instructions.

## Testing

Sample test files are provided in the `TestFiles/` directory. See [TestFiles/README.md](TestFiles/README.md) for details on the test scenario.

## Documentation

- **[QUICKSTART.md](QUICKSTART.md)** - Get started in 5 minutes
- **[USAGE.md](USAGE.md)** - Comprehensive usage guide with troubleshooting
- **[DISTRIBUTION.md](DISTRIBUTION.md)** - Distribution and packaging guide (NuGet, GitHub Releases, etc.)
- **[CONTRIBUTING.md](CONTRIBUTING.md)** - Contribution guidelines and coding standards
- **[AiMergeTool/README.md](AiMergeTool/README.md)** - Detailed application documentation
- **[TestFiles/README.md](TestFiles/README.md)** - Test scenario documentation

## Git Integration

Configure the tool as your git mergetool:

```bash
git config --global merge.tool aimergetool
git config --global mergetool.aimergetool.cmd 'AiMergeTool.exe "$BASE" "$LOCAL" "$REMOTE" "$MERGED"'
git config --global mergetool.aimergetool.trustExitCode false
```

## Technology Stack

- **Framework**: .NET 8.0 WPF
- **AI Integration**: Azure.AI.OpenAI
- **Diff Engine**: DiffPlex
- **Text Editor**: AvalonEdit
- **Command-Line Parsing**: CommandLineParser
- **Configuration**: Microsoft.Extensions.Configuration

## Security

- Never commit API keys to version control
- Use environment variables or secure configuration management
- The `.gitignore` file excludes sensitive files like `.env`
- Review AI suggestions before saving - AI can make mistakes

## License

See [LICENSE](LICENSE) file for details.

## Contributing

Contributions are welcome! Please feel free to submit issues or pull requests.
