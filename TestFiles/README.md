# Test Files for AI Merge Tool

This directory contains sample files for testing the AI Merge Tool.

## Test Scenario

The test files represent a common merge conflict scenario:

### Base File (`base.cs`)
The original file with a simple Calculator class containing `Add` and `Multiply` methods.

### Ours File (`ours.cs`)
Our local changes added a `Subtract` method at the beginning of the class.

### Theirs File (`theirs.cs`)
Incoming changes added a `Divide` method at the end of the class.

### Merged File (`merged.cs`)
The current merged state (initially same as base).

## Expected Resolution

The AI should intelligently merge both changes, resulting in a Calculator class with all four methods:
- Subtract (from ours)
- Add (from base)
- Multiply (from base)
- Divide (from theirs)

## How to Test

On Windows, run:
```bash
cd TestFiles
..\AiMergeTool\bin\Debug\net8.0-windows\AiMergeTool.exe base.cs ours.cs theirs.cs merged.cs
```

Or with named arguments:
```bash
..\AiMergeTool\bin\Debug\net8.0-windows\AiMergeTool.exe -b base.cs -l ours.cs -r theirs.cs -m merged.cs
```

The tool will open showing:
- **Left pane (Blue)**: Your changes with the Subtract method
- **Center pane (Green)**: Editable merged result
- **Right pane (Red)**: Incoming changes with the Divide method

Click "Resolve with AI" to let the AI suggest an intelligent merge that includes both methods.
