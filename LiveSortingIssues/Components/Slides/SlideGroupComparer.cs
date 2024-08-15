using System.ComponentModel;
using System.Windows.Data;
using LiveSortingIssues.Components.Shared;

namespace LiveSortingIssues.Components.Slides;

public abstract class SlideGroupComparer : AbstractComparer<CollectionViewGroup, SlideGroupName>
{
    private static readonly ByNameComparer ByName = new();
    private static readonly ByStatusComparer ByStatus = new();

    public static SlideGroupComparer GetComparer(SlideProperty slideProperty, ListSortDirection sortDirection)
    {
        SlideGroupComparer comparer = slideProperty switch
        {
            SlideProperty.Status => ByStatus,
            _ => ByName
        };

        comparer.SortDirection = sortDirection;

        return comparer;
    }

    protected sealed override SlideGroupName? GetCompareByMember(CollectionViewGroup? item)
    {
        return item?.Name as SlideGroupName;
    }

    private sealed class ByNameComparer : SlideGroupComparer
    {
        protected override int CompareByAscending(SlideGroupName first, SlideGroupName second)
        {
            return string.Compare(first.Name, second.Name, StringComparison.OrdinalIgnoreCase);
        }
    }

    private sealed class ByStatusComparer : SlideGroupComparer
    {
        protected override int CompareByAscending(SlideGroupName first, SlideGroupName second)
        {
            return first.StatusGroup - second.StatusGroup;
        }
    }
}
