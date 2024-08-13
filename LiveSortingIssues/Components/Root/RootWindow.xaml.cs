using LiveSortingIssues.Components.Root;

namespace LiveSortingIssues;

public partial class RootWindow
{
    private RootViewModel? _viewModel;

    public RootWindow()
    {
        InitializeComponent();
    }

    public RootViewModel? ViewModel
    {
        get => _viewModel;
        set
        {
            _viewModel = value;
            DataContext = value;
        }
    }
}
