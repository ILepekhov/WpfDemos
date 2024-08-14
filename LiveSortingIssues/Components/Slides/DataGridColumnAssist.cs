using System.Windows;
using System.Windows.Controls;

namespace LiveSortingIssues.Components.Slides;

public static class DataGridColumnAssist
{
    #region AtthacedProperty : SlideProperty

    /// <summary>
    /// Provides a way to mark the SlidesDataGrid columns with the respective slide property that
    /// is used to distinguish the columns when sorting and grouping.
    /// </summary>
    public static readonly DependencyProperty SlidePropertyProperty = DependencyProperty.RegisterAttached(
        "SlideProperty", typeof(SlideProperty?), typeof(DataGridColumnAssist));

    /// <summary>
    /// Assigns the specified <paramref name="value"/> of the <see cref="SlideProperty"/> type to the specified <paramref name="column"/>.
    /// </summary>
    public static void SetSlideProperty(DataGridColumn column, SlideProperty? value) => column.SetValue(SlidePropertyProperty, value);

    /// <summary>
    /// Gets the respective <see cref="SlideProperty"/> value of the specified <paramref name="column"/>.
    /// </summary>
    public static SlideProperty? GetSlideProperty(DataGridColumn column) => (SlideProperty?)column.GetValue(SlidePropertyProperty);

    #endregion
}
