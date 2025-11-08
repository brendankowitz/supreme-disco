using System.IO;

namespace AiMergeTool.Utilities;

/// <summary>
/// Utility for detecting file types and extensions
/// </summary>
public static class FileTypeDetector
{
    public static string? GetSyntaxHighlightingName(string filePath)
    {
        var extension = Path.GetExtension(filePath).ToLowerInvariant();
        
        return extension switch
        {
            ".cs" => "C#",
            ".vb" => "VB",
            ".cpp" or ".cc" or ".cxx" or ".h" or ".hpp" => "C++",
            ".c" => "C",
            ".java" => "Java",
            ".py" => "Python",
            ".js" => "JavaScript",
            ".ts" => "TypeScript",
            ".xml" or ".xaml" or ".csproj" or ".vbproj" => "XML",
            ".html" or ".htm" => "HTML",
            ".css" => "CSS",
            ".json" => "JavaScript",
            ".sql" => "SQL",
            ".php" => "PHP",
            ".rb" => "Ruby",
            ".go" => "Go",
            ".rs" => "Rust",
            ".swift" => "Swift",
            ".kt" => "Java", // Close enough for Kotlin
            ".md" => "MarkDown",
            _ => null
        };
    }
}
