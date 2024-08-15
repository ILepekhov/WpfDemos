using System.ComponentModel;
using System.Windows.Data;

namespace LiveSortingIssues.Components.Slides.Grouping;

public static class GroupingHelper
{
    public static GroupDescription GetGroupDescription(SlideProperty sortByProperty, ListSortDirection sortDirection)
    {
        return new PropertyGroupDescription(GetPropertyName(sortByProperty))
        {
            StringComparison = StringComparison.OrdinalIgnoreCase,
            Converter = GetPropertyValueConverter(sortByProperty),
            CustomSort = sortDirection is ListSortDirection.Ascending
                ? PropertyGroupDescription.CompareNameAscending
                : PropertyGroupDescription.CompareNameDescending
        };
    }

    public static string GetPropertyName(SlideProperty slideProperty)
    {
        return slideProperty switch
        {
            SlideProperty.Position => nameof(SlideViewModel.MagazinePosition),
            SlideProperty.Folder => nameof(SlideViewModel.Folder),
            SlideProperty.Status => nameof(SlideViewModel.Status),
            _ => throw new ArgumentOutOfRangeException(nameof(slideProperty), slideProperty, null)
        };
    }

    private static IValueConverter? GetPropertyValueConverter(SlideProperty slideProperty)
    {
        return slideProperty is SlideProperty.Status
            ? new StatusGroupConverter()
            : null;
    }
}
