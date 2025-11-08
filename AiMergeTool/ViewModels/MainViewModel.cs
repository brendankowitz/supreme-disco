using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using AiMergeTool.Models;
using AiMergeTool.Services;

namespace AiMergeTool.ViewModels;

/// <summary>
/// ViewModel for the main merge window
/// </summary>
public class MainViewModel : INotifyPropertyChanged
{
    private readonly DiffService _diffService;
    private readonly AiMergeService _aiMergeService;
    private MergeContext _mergeContext;
    private string _oursContent = string.Empty;
    private string _mergedContent = string.Empty;
    private string _theirsContent = string.Empty;
    private string _statusMessage = "Ready";
    private bool _isProcessing;

    public event PropertyChangedEventHandler? PropertyChanged;

    public MainViewModel(DiffService diffService, AiMergeService aiMergeService, MergeContext mergeContext)
    {
        _diffService = diffService;
        _aiMergeService = aiMergeService;
        _mergeContext = mergeContext;

        // Initialize content
        OursContent = mergeContext.Ours.Content;
        TheirsContent = mergeContext.Theirs.Content;
        MergedContent = mergeContext.Merged.Content;

        // Initialize commands
        ResolveWithAiCommand = new RelayCommand(async _ => await ResolveWithAiAsync(), _ => !IsProcessing);
        SaveCommand = new RelayCommand(_ => Save(), _ => !IsProcessing);
        ExitCommand = new RelayCommand(_ => Exit());
    }

    public string OursContent
    {
        get => _oursContent;
        set { _oursContent = value; OnPropertyChanged(); }
    }

    public string MergedContent
    {
        get => _mergedContent;
        set { _mergedContent = value; OnPropertyChanged(); }
    }

    public string TheirsContent
    {
        get => _theirsContent;
        set { _theirsContent = value; OnPropertyChanged(); }
    }

    public string StatusMessage
    {
        get => _statusMessage;
        set { _statusMessage = value; OnPropertyChanged(); }
    }

    public bool IsProcessing
    {
        get => _isProcessing;
        set { _isProcessing = value; OnPropertyChanged(); }
    }

    public ICommand ResolveWithAiCommand { get; }
    public ICommand SaveCommand { get; }
    public ICommand ExitCommand { get; }

    private async Task ResolveWithAiAsync()
    {
        try
        {
            IsProcessing = true;
            StatusMessage = "Resolving with AI...";

            var resolved = await _aiMergeService.SuggestMergeResolutionAsync(_mergeContext);
            MergedContent = resolved;

            StatusMessage = "AI resolution complete";
        }
        catch (Exception ex)
        {
            StatusMessage = $"Error: {ex.Message}";
            MessageBox.Show($"Failed to resolve with AI: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        finally
        {
            IsProcessing = false;
        }
    }

    private void Save()
    {
        try
        {
            var outputPath = _mergeContext.OutputPath;
            if (string.IsNullOrEmpty(outputPath))
            {
                var dialog = new Microsoft.Win32.SaveFileDialog
                {
                    Filter = "All Files (*.*)|*.*",
                    FileName = Path.GetFileName(_mergeContext.Merged.FilePath)
                };

                if (dialog.ShowDialog() == true)
                {
                    outputPath = dialog.FileName;
                }
                else
                {
                    return;
                }
            }

            File.WriteAllText(outputPath, MergedContent);
            StatusMessage = $"Saved to {outputPath}";
            MessageBox.Show($"Successfully saved to {outputPath}", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        catch (Exception ex)
        {
            StatusMessage = $"Error saving: {ex.Message}";
            MessageBox.Show($"Failed to save: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void Exit()
    {
        Application.Current.Shutdown();
    }

    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

/// <summary>
/// Simple relay command implementation
/// </summary>
public class RelayCommand : ICommand
{
    private readonly Action<object?> _execute;
    private readonly Func<object?, bool>? _canExecute;

    public event EventHandler? CanExecuteChanged
    {
        add => CommandManager.RequerySuggested += value;
        remove => CommandManager.RequerySuggested -= value;
    }

    public RelayCommand(Action<object?> execute, Func<object?, bool>? canExecute = null)
    {
        _execute = execute ?? throw new ArgumentNullException(nameof(execute));
        _canExecute = canExecute;
    }

    public bool CanExecute(object? parameter) => _canExecute?.Invoke(parameter) ?? true;

    public void Execute(object? parameter) => _execute(parameter);
}
