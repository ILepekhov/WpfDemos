using System.Windows.Controls;
using System.Windows.Input;
using LiveSortingIssues.Components.Shared;

namespace LiveSortingIssues.Components.Slides;

public static class RowEventsHandler
{
    public static void CustomizeCancelEditing(object sender, KeyEventArgs e)
    {
        if (sender is not DataGridRow { IsSelected: true, IsEditing: true } row)
        {
            return;
        }

        if (e.Key is Key.Enter)
        {
            DiscardMovingSelectionToTheNextRow(row, e);
        }
        else if (e.Key is Key.Escape)
        {
            CustomizeCancelEditingByEsc(row, e);
        }
    }

    private static void DiscardMovingSelectionToTheNextRow(DataGridRow row, KeyEventArgs e)
    {
        row.CommitEditing();
        e.Handled = true;
    }

    private static void CustomizeCancelEditingByEsc(DataGridRow row, KeyEventArgs e)
    {
        row.CancelEditing();
        e.Handled = true;
    }

    private static void CommitEditing(this DataGridRow row)
    {
        DataGrid? parentDataGrid = row.FindVisualParent<DataGrid>();

        if (parentDataGrid is null)
        {
            return;
        }

        parentDataGrid.CommitEdit(DataGridEditingUnit.Cell, true);
        parentDataGrid.CommitEdit(DataGridEditingUnit.Row, true);
    }

    private static void CancelEditing(this DataGridRow row)
    {
        DataGrid? parentDataGrid = row.FindVisualParent<DataGrid>();

        if (parentDataGrid is null)
        {
            return;
        }

        parentDataGrid.CancelEdit(DataGridEditingUnit.Cell);
        parentDataGrid.CancelEdit(DataGridEditingUnit.Row);
    }
}
