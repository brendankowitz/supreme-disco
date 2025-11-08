using System.IO;
using System.Windows;
using AiMergeTool.Models;
using AiMergeTool.Services;
using AiMergeTool.ViewModels;
using AiMergeTool.Views;
using CommandLine;

namespace AiMergeTool;

/// <summary>
/// Application entry point
/// </summary>
public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        // Parse command-line arguments
        Parser.Default.ParseArguments<CommandLineOptions>(e.Args)
            .WithParsed(options => RunApplication(options))
            .WithNotParsed(errors => HandleParseErrors(errors));
    }

    private void RunApplication(CommandLineOptions options)
    {
        try
        {
            // Load configuration
            var configService = new ConfigurationService();
            var aiConfig = configService.GetAiConfiguration();

            // Validate paths
            var basePath = options.GetBasePath();
            var localPath = options.GetLocalPath();
            var remotePath = options.GetRemotePath();
            var mergedPath = options.GetMergedPath();

            if (string.IsNullOrEmpty(localPath) || string.IsNullOrEmpty(remotePath))
            {
                MessageBox.Show(
                    "Usage: AiMergeTool.exe <base> <local> <remote> <merged>\n\n" +
                    "Or use named arguments:\n" +
                    "  -b, --base      Base file path\n" +
                    "  -l, --local     Local (ours) file path\n" +
                    "  -r, --remote    Remote (theirs) file path\n" +
                    "  -m, --merged    Merged output file path",
                    "Invalid Arguments",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
                Shutdown(1);
                return;
            }

            // Create merge context
            var context = new MergeContext
            {
                Ours = FileContent.FromFile(localPath),
                Theirs = FileContent.FromFile(remotePath),
                OutputPath = mergedPath
            };

            // Load base if provided
            if (!string.IsNullOrEmpty(basePath) && File.Exists(basePath))
            {
                context.Base = FileContent.FromFile(basePath);
            }
            else
            {
                // If no base, use empty content
                context.Base = new FileContent { FilePath = "base", Content = "", Lines = new List<string>() };
            }

            // Initialize merged content
            if (!string.IsNullOrEmpty(mergedPath) && File.Exists(mergedPath))
            {
                context.Merged = FileContent.FromFile(mergedPath);
            }
            else
            {
                // Start with local content
                context.Merged = new FileContent
                {
                    FilePath = mergedPath,
                    Content = context.Ours.Content,
                    Lines = new List<string>(context.Ours.Lines)
                };
            }

            // Create services
            var diffService = new DiffService();
            var aiService = new AiMergeService(aiConfig, diffService);

            // Calculate diffs
            context.OursVsBaseDiffs = diffService.ExtractDiffRegions(
                diffService.CalculateDiff(context.Base.Content, context.Ours.Content), true);
            context.TheirsVsBaseDiffs = diffService.ExtractDiffRegions(
                diffService.CalculateDiff(context.Base.Content, context.Theirs.Content), true);

            // Create and show main window
            var viewModel = new MainViewModel(diffService, aiService, context);
            var mainWindow = new MainWindow();
            mainWindow.SetViewModel(viewModel);
            mainWindow.Show();
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                $"Failed to initialize application: {ex.Message}\n\n{ex.StackTrace}",
                "Error",
                MessageBoxButton.OK,
                MessageBoxImage.Error);
            Shutdown(1);
        }
    }

    private void HandleParseErrors(IEnumerable<Error> errors)
    {
        var errorList = string.Join("\n", errors.Select(e => e.ToString()));
        MessageBox.Show(
            $"Failed to parse command-line arguments:\n{errorList}",
            "Parse Error",
            MessageBoxButton.OK,
            MessageBoxImage.Error);
        Shutdown(1);
    }
}
