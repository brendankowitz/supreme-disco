using AiMergeTool.Models;
using Microsoft.Extensions.Configuration;

namespace AiMergeTool.Services;

/// <summary>
/// Service for loading and managing configuration
/// </summary>
public class ConfigurationService
{
    private readonly IConfiguration _configuration;

    public ConfigurationService()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables();

        _configuration = builder.Build();
    }

    public AiConfiguration GetAiConfiguration()
    {
        var config = new AiConfiguration();
        
        // Try to load from configuration
        _configuration.GetSection("AiConfiguration").Bind(config);
        
        // Environment variables override configuration file
        config.Endpoint = Environment.GetEnvironmentVariable("OPENAI_ENDPOINT") ?? config.Endpoint;
        config.ApiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY") ?? config.ApiKey;
        config.DeploymentName = Environment.GetEnvironmentVariable("OPENAI_DEPLOYMENT") ?? config.DeploymentName;
        
        var isAzureStr = Environment.GetEnvironmentVariable("OPENAI_IS_AZURE");
        if (!string.IsNullOrEmpty(isAzureStr))
        {
            config.IsAzure = bool.Parse(isAzureStr);
        }

        var modelStr = Environment.GetEnvironmentVariable("OPENAI_MODEL");
        if (!string.IsNullOrEmpty(modelStr))
        {
            config.Model = modelStr;
        }

        return config;
    }
}
