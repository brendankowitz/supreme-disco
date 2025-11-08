using AiMergeTool.Models;
using Azure;
using Azure.AI.OpenAI;
using OpenAI.Chat;

namespace AiMergeTool.Services;

/// <summary>
/// Service for interacting with OpenAI/Azure OpenAI to resolve merge conflicts
/// </summary>
public class AiMergeService
{
    private readonly AiConfiguration _config;
    private readonly DiffService _diffService;

    public AiMergeService(AiConfiguration config, DiffService diffService)
    {
        _config = config;
        _diffService = diffService;
    }

    /// <summary>
    /// Use AI to suggest a merge resolution
    /// </summary>
    public async Task<string> SuggestMergeResolutionAsync(MergeContext context, CancellationToken cancellationToken = default)
    {
        var smartDiff = _diffService.CreateSmartDiffForLLM(context.Base, context.Ours, context.Theirs);
        
        var prompt = $@"{smartDiff}

Please analyze the merge conflict above and provide a resolved version of the file that:
1. Preserves the intent of both changes where possible
2. Resolves conflicts intelligently based on the context
3. Maintains code quality and consistency
4. Provides ONLY the final merged content without any explanations or markdown formatting

Return just the final merged file content.";

        try
        {
            ChatClient? client = null;
            
            if (_config.IsAzure)
            {
                if (string.IsNullOrEmpty(_config.Endpoint) || string.IsNullOrEmpty(_config.ApiKey))
                    throw new InvalidOperationException("Azure OpenAI endpoint and API key must be configured");
                
                var azureClient = new AzureOpenAIClient(
                    new Uri(_config.Endpoint),
                    new AzureKeyCredential(_config.ApiKey));
                
                client = azureClient.GetChatClient(_config.DeploymentName ?? "gpt-4");
            }
            else
            {
                if (string.IsNullOrEmpty(_config.ApiKey))
                    throw new InvalidOperationException("OpenAI API key must be configured");
                
                client = new ChatClient(_config.Model, _config.ApiKey);
            }

            var messages = new List<ChatMessage>
            {
                new SystemChatMessage("You are an expert code merge resolution assistant. Your task is to intelligently merge code changes while preserving the intent of both versions."),
                new UserChatMessage(prompt)
            };

            var response = await client.CompleteChatAsync(
                messages,
                new ChatCompletionOptions
                {
                    MaxOutputTokenCount = _config.MaxTokens,
                    Temperature = 0.3f // Lower temperature for more deterministic output
                },
                cancellationToken);

            return response.Value.Content[0].Text;
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Failed to get AI merge suggestion: {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Validate the AI configuration
    /// </summary>
    public bool ValidateConfiguration()
    {
        if (_config.IsAzure)
        {
            return !string.IsNullOrEmpty(_config.Endpoint) && 
                   !string.IsNullOrEmpty(_config.ApiKey) &&
                   !string.IsNullOrEmpty(_config.DeploymentName);
        }
        else
        {
            return !string.IsNullOrEmpty(_config.ApiKey);
        }
    }
}
