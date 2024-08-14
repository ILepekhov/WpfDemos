using System.Collections;
using System.ComponentModel;

namespace LiveSortingIssues.Components.Slides;

public abstract class SlideComparer : IComparer<SlideViewModel>, IComparer
{
    private const int AreEqual = 0;
    private const int IsLess = -1;
    private const int IsGreater = 1;

    private static readonly ChainedComparer ByPosition = new(new ByPositionComparer(), new ByStatusComparer());
    private static readonly ChainedComparer ByFolder = new(new ByFolderComparer(), new ByPositionComparer());
    private static readonly ChainedComparer ByStatus = new(new ByStatusComparer(), new ByPositionComparer());

    public ListSortDirection SortDirection { get; protected set; } = ListSortDirection.Ascending;

    public int Compare(object? x, object? y) => Compare(x as SlideViewModel, y as SlideViewModel);

    public int Compare(SlideViewModel? first, SlideViewModel? second)
    {
        if (first is null || second is null)
        {
            return CompareByAscendingIfAnyIsNull(first, second) * GetSortingDirectionMultiplier();
        }

        return CompareByAscending(first, second) * GetSortingDirectionMultiplier();
    }

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

    protected abstract int CompareByAscending(SlideViewModel first, SlideViewModel second);

    private int GetSortingDirectionMultiplier()
    {
        return SortDirection is ListSortDirection.Ascending ? 1 : -1;
    }

    private static int CompareByAscendingIfAnyIsNull(SlideViewModel? first, SlideViewModel? second)
    {
        if (first is null && second is null)
        {
            return AreEqual;
        }

        if (first is null)
        {
            return IsLess;
        }

        if (second is null)
        {
            return IsGreater;
        }

        throw new InvalidOperationException();
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
            return first.Position.CompareTo(second.Position);
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
