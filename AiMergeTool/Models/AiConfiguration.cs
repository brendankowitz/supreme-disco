namespace AiMergeTool.Models;

/// <summary>
/// Configuration for OpenAI/Azure OpenAI connection
/// </summary>
public class AiConfiguration
{
    public string? Endpoint { get; set; }
    public string? ApiKey { get; set; }
    public string? DeploymentName { get; set; }
    public bool IsAzure { get; set; }
    public string Model { get; set; } = "gpt-4";
    public int MaxTokens { get; set; } = 4000;
}
