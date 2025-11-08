# Quick Start Guide

Get started with AI Merge Tool in 5 minutes!

## 1. Prerequisites

- Windows 10 or later
- .NET 8.0 Runtime ([Download](https://dotnet.microsoft.com/download/dotnet/8.0))
- OpenAI API key or Azure OpenAI access

## 2. Get the Tool

### Option A: Download Release (Recommended)
Download the latest release from the [Releases page](https://github.com/brendankowitz/supreme-disco/releases)

### Option B: Build from Source
```bash
git clone https://github.com/brendankowitz/supreme-disco.git
cd supreme-disco
dotnet build -c Release AiMergeTool.sln
```

## 3. Configure API Key

### Using OpenAI
```cmd
set OPENAI_API_KEY=sk-your-api-key-here
```

### Using Azure OpenAI
```cmd
set OPENAI_ENDPOINT=https://your-resource.openai.azure.com/
set OPENAI_API_KEY=your-azure-api-key
set OPENAI_DEPLOYMENT=gpt-4
set OPENAI_IS_AZURE=true
```

**Tip:** Add these to your system environment variables to make them permanent!

## 4. Test the Tool

Try it with the included test files:

```cmd
cd supreme-disco
AiMergeTool\bin\Release\net8.0-windows\AiMergeTool.exe ^
  TestFiles\base.cs ^
  TestFiles\ours.cs ^
  TestFiles\theirs.cs ^
  TestFiles\merged.cs
```

This will open the UI showing:
- **Left (Blue):** Your local changes
- **Center (Green):** Editable merged result
- **Right (Red):** Incoming changes

Click "🤖 Resolve with AI" to see the magic happen!

## 5. Integrate with Git

Make it your default merge tool:

```bash
# Configure git
git config --global merge.tool aimergetool
git config --global mergetool.aimergetool.cmd 'AiMergeTool.exe "$BASE" "$LOCAL" "$REMOTE" "$MERGED"'

# Use it during a merge
git merge feature-branch
git mergetool
```

## Next Steps

- Read [USAGE.md](USAGE.md) for detailed documentation
- Check [CONTRIBUTING.md](CONTRIBUTING.md) to contribute
- Star the repo if you find it useful! ⭐

## Troubleshooting

### "API key not configured"
Set the `OPENAI_API_KEY` environment variable before running.

### "File not found"
Make sure all file paths are correct and files exist.

### Application doesn't start
Install [.NET 8.0 Runtime](https://dotnet.microsoft.com/download/dotnet/8.0)

## Need Help?

- 📖 Check [USAGE.md](USAGE.md) for detailed instructions
- 🐛 Report issues on [GitHub Issues](https://github.com/brendankowitz/supreme-disco/issues)
- 💬 Ask questions in [Discussions](https://github.com/brendankowitz/supreme-disco/discussions)

---

**Happy merging! 🚀**
