using System.Windows.Controls;
using LiveSortingIssues.Components.Root;
using LiveSortingIssues.Components.Slides;

namespace LiveSortingIssues;

public partial class RootWindow
{
    public RootWindow()
    {
        InitializeComponent();
    }

    public RootWindow(RootViewModel viewModel, SlidesView slidesView) : this()
    {
        DataContext = viewModel;

        Grid.SetColumn(slidesView, 0);
        RootGrid.Children.Add(slidesView);
    }
}
