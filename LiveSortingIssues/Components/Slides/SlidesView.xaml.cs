using System.Windows.Controls;
using System.Windows.Input;

namespace LiveSortingIssues.Components.Slides;

public partial class SlidesView : UserControl
{
    public SlidesView()
    {
        InitializeComponent();
    }

    private void OnRowPreviewKeyDown(object sender, KeyEventArgs e) =>
        RowEventsHandler.CustomizeCancelEditing(sender, e);
}
