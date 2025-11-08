namespace AiMergeTool.Models;

/// <summary>
/// Represents a region of differences in a file
/// </summary>
public class DiffRegion
{
    public int StartLine { get; set; }
    public int EndLine { get; set; }
    public DiffType Type { get; set; }
    public List<string> Lines { get; set; } = new();
}

public enum DiffType
{
    Unchanged,
    Added,
    Deleted,
    Modified
}
