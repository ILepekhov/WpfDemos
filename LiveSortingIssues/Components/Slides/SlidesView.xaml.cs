using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Input;

namespace LiveSortingIssues.Components.Slides;

public partial class SlidesView
{
    public SlidesView()
    {
        InitializeComponent();
    }

    private void OnRowPreviewKeyDown(object sender, KeyEventArgs e) =>
        RowEventsHandler.CustomizeCancelEditing(sender, e);

    private void SlideDataGridOnSorting(object sender, DataGridSortingEventArgs e)
    {
        e.Handled = true;

        var sortDirection = e.Column.SortDirection != ListSortDirection.Ascending
            ? ListSortDirection.Ascending
            : ListSortDirection.Descending;
        var sortByProperty = DataGridColumnAssist.GetSlideProperty(e.Column);

        if (sortByProperty is null)
        {
            throw new InvalidCastException();
        }

        e.Column.SortDirection = sortDirection;

        (DataContext as SlidesViewModel)?.UpdateDisplayingParameters(sortByProperty.Value, sortDirection);
    }
}
