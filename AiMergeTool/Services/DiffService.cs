using AiMergeTool.Models;
using DiffPlex;
using DiffPlex.DiffBuilder;
using DiffPlex.DiffBuilder.Model;

namespace AiMergeTool.Services;

/// <summary>
/// Service for computing differences between files
/// </summary>
public class DiffService
{
    private readonly ISideBySideDiffBuilder _diffBuilder;

    public DiffService()
    {
        _diffBuilder = new SideBySideDiffBuilder(new Differ());
    }

    /// <summary>
    /// Calculate differences between two files
    /// </summary>
    public SideBySideDiffModel CalculateDiff(string oldText, string newText)
    {
        return _diffBuilder.BuildDiffModel(oldText, newText);
    }

    /// <summary>
    /// Extract diff regions from the diff model
    /// </summary>
    public List<DiffRegion> ExtractDiffRegions(SideBySideDiffModel diffModel, bool useNewSide = true)
    {
        var regions = new List<DiffRegion>();
        var lines = useNewSide ? diffModel.NewText.Lines : diffModel.OldText.Lines;
        
        DiffRegion? currentRegion = null;
        
        for (int i = 0; i < lines.Count; i++)
        {
            var line = lines[i];
            var diffType = ConvertChangeType(line.Type);
            
            if (currentRegion == null || currentRegion.Type != diffType)
            {
                if (currentRegion != null)
                {
                    regions.Add(currentRegion);
                }
                
                currentRegion = new DiffRegion
                {
                    StartLine = i,
                    Type = diffType,
                    Lines = new List<string>()
                };
            }
            
            currentRegion.Lines.Add(line.Text ?? string.Empty);
            currentRegion.EndLine = i;
        }
        
        if (currentRegion != null)
        {
            regions.Add(currentRegion);
        }
        
        return regions;
    }

    private DiffType ConvertChangeType(ChangeType changeType)
    {
        return changeType switch
        {
            ChangeType.Unchanged => DiffType.Unchanged,
            ChangeType.Deleted => DiffType.Deleted,
            ChangeType.Inserted => DiffType.Added,
            ChangeType.Modified => DiffType.Modified,
            ChangeType.Imaginary => DiffType.Unchanged,
            _ => DiffType.Unchanged
        };
    }

    /// <summary>
    /// Create a smart diff summary for LLM processing
    /// </summary>
    public string CreateSmartDiffForLLM(FileContent baseFile, FileContent oursFile, FileContent theirsFile)
    {
        var oursDiff = CalculateDiff(baseFile.Content, oursFile.Content);
        var theirsDiff = CalculateDiff(baseFile.Content, theirsFile.Content);
        
        var summary = new System.Text.StringBuilder();
        summary.AppendLine("# Merge Conflict Context");
        summary.AppendLine();
        summary.AppendLine($"## Base File: {baseFile.FilePath}");
        summary.AppendLine();
        
        summary.AppendLine("## Changes in OURS (local):");
        AppendDiffSummary(summary, oursDiff.NewText.Lines);
        summary.AppendLine();
        
        summary.AppendLine("## Changes in THEIRS (remote):");
        AppendDiffSummary(summary, theirsDiff.NewText.Lines);
        summary.AppendLine();
        
        summary.AppendLine("## Full Context:");
        summary.AppendLine("### BASE:");
        summary.AppendLine("```");
        summary.AppendLine(baseFile.Content);
        summary.AppendLine("```");
        summary.AppendLine();
        
        summary.AppendLine("### OURS:");
        summary.AppendLine("```");
        summary.AppendLine(oursFile.Content);
        summary.AppendLine("```");
        summary.AppendLine();
        
        summary.AppendLine("### THEIRS:");
        summary.AppendLine("```");
        summary.AppendLine(theirsFile.Content);
        summary.AppendLine("```");
        
        return summary.ToString();
    }

    private void AppendDiffSummary(System.Text.StringBuilder summary, List<DiffPiece> lines)
    {
        var changeCount = new Dictionary<ChangeType, int>();
        
        foreach (var line in lines)
        {
            if (line.Type != ChangeType.Unchanged && line.Type != ChangeType.Imaginary)
            {
                if (!changeCount.ContainsKey(line.Type))
                    changeCount[line.Type] = 0;
                changeCount[line.Type]++;
                
                var prefix = line.Type switch
                {
                    ChangeType.Inserted => "+ ",
                    ChangeType.Deleted => "- ",
                    ChangeType.Modified => "~ ",
                    _ => "  "
                };
                
                summary.AppendLine($"{prefix}{line.Text}");
            }
        }
        
        if (changeCount.Count == 0)
        {
            summary.AppendLine("(No changes)");
        }
    }
}
