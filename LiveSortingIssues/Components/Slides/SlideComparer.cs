using System.ComponentModel;
using LiveSortingIssues.Components.Shared;

namespace LiveSortingIssues.Components.Slides;

public abstract class SlideComparer : AbstractComparer<SlideViewModel>
{
    private static readonly ChainedComparer ByPosition = new(new ByPositionComparer(), new ByStatusComparer());
    private static readonly ChainedComparer ByFolder = new(new ByFolderComparer(), new ByPositionComparer());
    private static readonly ChainedComparer ByStatus = new(new ByStatusComparer(), new ByPositionComparer());

    public static SlideComparer GetComparer(SlideProperty sortByProperty, ListSortDirection sortDirection)
    {
        SlideComparer comparer = sortByProperty switch
        {
            SlideProperty.Position => ByPosition,
            SlideProperty.Folder => ByFolder,
            SlideProperty.Status => ByStatus,
            _ => throw new ArgumentOutOfRangeException(nameof(sortByProperty), sortByProperty, null)
        };

        comparer.SortDirection = sortDirection;

        return comparer;
    }

    private sealed class ChainedComparer : SlideComparer
    {
        private readonly SlideComparer _firstComparer;
        private readonly SlideComparer _secondComparer;

        public ChainedComparer(SlideComparer firstComparer, SlideComparer secondComparer)
        {
            _firstComparer = firstComparer;
            _secondComparer = secondComparer;
        }

        protected override int CompareByAscending(SlideViewModel first, SlideViewModel second)
        {
            var firstComparerResult = _firstComparer.CompareByAscending(first, second);

            return firstComparerResult != 0
                ? firstComparerResult
                : _secondComparer.CompareByAscending(first, second);
        }
    }

    private sealed class ByPositionComparer : SlideComparer
    {
        protected override int CompareByAscending(SlideViewModel first, SlideViewModel second)
        {
            return string.Compare(first.Position, second.Position, StringComparison.OrdinalIgnoreCase);
        }
    }

    private sealed class ByFolderComparer : SlideComparer
    {
        protected override int CompareByAscending(SlideViewModel first, SlideViewModel second)
        {
            return string.Compare(first.Folder, second.Folder, StringComparison.OrdinalIgnoreCase);
        }
    }

    private sealed class ByStatusComparer : SlideComparer
    {
        protected override int CompareByAscending(SlideViewModel first, SlideViewModel second)
        {
            return first.Status - second.Status;
        }
    }
}
