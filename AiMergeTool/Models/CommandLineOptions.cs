using CommandLine;

namespace AiMergeTool.Models;

/// <summary>
/// Command-line arguments compatible with standard diff/merge tools
/// </summary>
public class CommandLineOptions
{
    [Option('b', "base", Required = false, HelpText = "Base (common ancestor) file path")]
    public string? BasePath { get; set; }

    [Option('l', "local", Required = false, HelpText = "Local (ours) file path")]
    public string? LocalPath { get; set; }

    [Option('r', "remote", Required = false, HelpText = "Remote (theirs) file path")]
    public string? RemotePath { get; set; }

    [Option('m', "merged", Required = false, HelpText = "Merged output file path")]
    public string? MergedPath { get; set; }

    [Option('o', "output", Required = false, HelpText = "Output file path (alternative to merged)")]
    public string? OutputPath { get; set; }

    // Alternative argument styles for compatibility with different merge tools
    [Value(0, MetaName = "base", Required = false, HelpText = "Base file (positional)")]
    public string? PositionalBase { get; set; }

    [Value(1, MetaName = "local", Required = false, HelpText = "Local file (positional)")]
    public string? PositionalLocal { get; set; }

    [Value(2, MetaName = "remote", Required = false, HelpText = "Remote file (positional)")]
    public string? PositionalRemote { get; set; }

    [Value(3, MetaName = "merged", Required = false, HelpText = "Merged file (positional)")]
    public string? PositionalMerged { get; set; }

    public string GetBasePath() => BasePath ?? PositionalBase ?? string.Empty;
    public string GetLocalPath() => LocalPath ?? PositionalLocal ?? string.Empty;
    public string GetRemotePath() => RemotePath ?? PositionalRemote ?? string.Empty;
    public string GetMergedPath() => MergedPath ?? OutputPath ?? PositionalMerged ?? string.Empty;
}
