using System.IO;

namespace AiMergeTool.Models;

/// <summary>
/// Represents the content of a file involved in a merge
/// </summary>
public class FileContent
{
    public string FilePath { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public List<string> Lines { get; set; } = new();

    public static FileContent FromFile(string filePath)
    {
        var content = File.ReadAllText(filePath);
        var lines = File.ReadAllLines(filePath).ToList();
        
        return new FileContent
        {
            FilePath = filePath,
            Content = content,
            Lines = lines
        };
    }
}
