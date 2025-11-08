using System.Windows;
using AiMergeTool.Utilities;
using AiMergeTool.ViewModels;
using ICSharpCode.AvalonEdit.Document;

namespace AiMergeTool.Views;

/// <summary>
/// Main window for the merge tool
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    public void SetViewModel(MainViewModel viewModel)
    {
        DataContext = viewModel;
        
        // Set up AvalonEdit documents with two-way binding
        OursEditor.Document = new TextDocument(viewModel.OursContent);
        MergedEditor.Document = new TextDocument(viewModel.MergedContent);
        TheirsEditor.Document = new TextDocument(viewModel.TheirsContent);
        
        // Update ViewModel when MergedEditor content changes
        MergedEditor.TextChanged += (s, e) =>
        {
            viewModel.MergedContent = MergedEditor.Document.Text;
        };
        
        // Update MergedEditor when ViewModel property changes
        viewModel.PropertyChanged += (s, e) =>
        {
            if (e.PropertyName == nameof(MainViewModel.MergedContent) && 
                MergedEditor.Document.Text != viewModel.MergedContent)
            {
                MergedEditor.Document.Text = viewModel.MergedContent;
            }
        };

        // Apply syntax highlighting based on file extension
        ApplySyntaxHighlighting(viewModel);
    }

    private void ApplySyntaxHighlighting(MainViewModel viewModel)
    {
        // Get file extension from merge context
        var highlightingName = FileTypeDetector.GetSyntaxHighlightingName(
            viewModel.MergeContext.Ours.FilePath);
        
        // Apply highlighting if found
        if (!string.IsNullOrEmpty(highlightingName))
        {
            var highlighting = ICSharpCode.AvalonEdit.Highlighting.HighlightingManager.Instance
                .GetDefinition(highlightingName);
            
            if (highlighting != null)
            {
                OursEditor.SyntaxHighlighting = highlighting;
                MergedEditor.SyntaxHighlighting = highlighting;
                TheirsEditor.SyntaxHighlighting = highlighting;
            }
        }
    }
}
