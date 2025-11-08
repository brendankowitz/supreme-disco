# Contributing to AI Merge Tool

Thank you for your interest in contributing to the AI Merge Tool! This document provides guidelines for contributing to the project.

## Code of Conduct

Please be respectful and constructive in all interactions. We're here to build great software together.

## How to Contribute

### Reporting Bugs

If you find a bug, please create an issue with:
- Clear description of the problem
- Steps to reproduce
- Expected vs. actual behavior
- Environment details (Windows version, .NET version, etc.)
- Error messages or screenshots if applicable

### Suggesting Enhancements

For feature requests or enhancements:
- Describe the feature and its benefits
- Explain the use case
- Consider how it fits with existing features
- Be open to discussion and alternative approaches

### Pull Requests

1. **Fork the repository** and create a branch from `main`
2. **Make your changes** following the coding standards below
3. **Test your changes** thoroughly
4. **Update documentation** if needed
5. **Create a pull request** with a clear description

## Development Setup

### Prerequisites
- Windows 10 or later
- Visual Studio 2022 or VS Code with C# extension
- .NET 8.0 SDK

### Building

```bash
dotnet build AiMergeTool.sln
```

### Running

```bash
dotnet run --project AiMergeTool/AiMergeTool.csproj -- -l local.txt -r remote.txt -m merged.txt
```

## Coding Standards

### C# Style
- Follow [Microsoft C# Coding Conventions](https://docs.microsoft.com/en-us/dotnet/csharp/fundamentals/coding-style/coding-conventions)
- Use meaningful variable and method names
- Add XML documentation comments for public APIs
- Keep methods focused and concise

### Project Structure
```
AiMergeTool/
├── Models/          # Data models and DTOs
├── Services/        # Business logic and external services
├── ViewModels/      # MVVM ViewModels
├── Views/           # WPF Views (XAML)
├── Utilities/       # Helper classes and utilities
└── appsettings.json # Configuration
```

### MVVM Pattern
- Follow MVVM pattern strictly
- Keep business logic out of code-behind
- Use commands for user actions
- Implement INotifyPropertyChanged for data binding

### Naming Conventions
- Classes: PascalCase
- Methods: PascalCase
- Properties: PascalCase
- Private fields: _camelCase
- Parameters: camelCase
- Constants: PascalCase

## Testing

Currently, the project focuses on manual testing. Contributions to add automated tests are welcome!

### Manual Testing
1. Build the project
2. Run with test files in `TestFiles/` directory
3. Verify UI renders correctly
4. Test AI resolution with valid API key
5. Test save functionality

## Areas for Contribution

We welcome contributions in these areas:

### High Priority
- [ ] Unit tests for services
- [ ] UI tests
- [ ] Improved error handling and validation
- [ ] Better diff visualization
- [ ] Configuration UI

### Medium Priority
- [ ] Support for more file types
- [ ] Customizable AI prompts
- [ ] Diff conflict markers parsing
- [ ] Undo/redo functionality
- [ ] Search and replace in editors

### Low Priority
- [ ] Themes and customization
- [ ] Keyboard shortcuts
- [ ] Plugin system
- [ ] Integration with more version control systems

## Documentation

When adding features:
- Update README.md if it affects usage
- Update USAGE.md for user-facing changes
- Add XML comments for new public APIs
- Consider adding examples

## Commit Messages

Use clear, descriptive commit messages:
- Use present tense ("Add feature" not "Added feature")
- Keep first line under 72 characters
- Add detailed description if needed

Examples:
```
Add syntax highlighting for Python files

Implement FileTypeDetector for Python .py files and update
MainWindow to use the new detection.
```

## Pull Request Process

1. Update the README.md or USAGE.md with details of changes if applicable
2. Ensure all builds pass
3. Request review from maintainers
4. Address review feedback
5. Squash commits if requested
6. Wait for approval and merge

## Code Review

All submissions require review. We're looking for:
- Code quality and maintainability
- Adherence to coding standards
- Appropriate test coverage
- Clear documentation
- No security vulnerabilities

## Security

- Never commit API keys or secrets
- Use environment variables for sensitive data
- Report security vulnerabilities privately
- Follow secure coding practices

## License

By contributing, you agree that your contributions will be licensed under the same license as the project (see LICENSE file).

## Questions?

Feel free to create an issue with the "question" label if you need help getting started or have questions about contributing.

## Recognition

Contributors will be recognized in the project documentation. Thank you for helping make AI Merge Tool better!
