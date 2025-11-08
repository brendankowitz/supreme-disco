namespace AiMergeTool.Models;

/// <summary>
/// Context for a merge operation
/// </summary>
public class MergeContext
{
    public FileContent Base { get; set; } = new();
    public FileContent Ours { get; set; } = new();
    public FileContent Theirs { get; set; } = new();
    public FileContent Merged { get; set; } = new();
    public string OutputPath { get; set; } = string.Empty;
    
    public List<DiffRegion> OursVsBaseDiffs { get; set; } = new();
    public List<DiffRegion> TheirsVsBaseDiffs { get; set; } = new();
}
