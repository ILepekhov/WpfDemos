using System.Collections;
using System.ComponentModel;

namespace LiveSortingIssues.Components.Shared;

/// <summary>
/// Provides a foundation for creating comparers supporting changing a sorting direction.
/// The cases where we need to compare two items not by themselves but by one of their properties are supported.
/// All you need is to implement the two methods: <see cref="CompareByAscending"/> and <see cref="GetCompareByMember"/>.
/// </summary>
/// <typeparam name="TItem">A type of the initial items to compare.</typeparam>
/// <typeparam name="TCompareBy">The type of values the <typeparamref name="TItem"/>'s should provide for performing comparison by.</typeparam>
public abstract class AbstractComparer<TItem, TCompareBy> : IComparer<TItem>, IComparer where TItem : class
{
    protected const int AreEqual = 0;
    protected const int IsLess = -1;
    protected const int IsGreater = 1;

    public ListSortDirection SortDirection { get; protected set; } = ListSortDirection.Ascending;

    public int Compare(object? x, object? y) => Compare(x as TItem, y as TItem);

    public int Compare(TItem? x, TItem? y)
    {
        var first = GetCompareByMember(x);
        var second = GetCompareByMember(y);

        if (first is null || second is null)
        {
            return CompareByAscendingIfAnyIsNull(first, second) * GetSortingDirectionMultiplier();
        }

        return CompareByAscending(first, second) * GetSortingDirectionMultiplier();
    }

    protected abstract int CompareByAscending(TCompareBy first, TCompareBy second);

    protected abstract TCompareBy? GetCompareByMember(TItem? item);

    private static int CompareByAscendingIfAnyIsNull(TCompareBy? first, TCompareBy? second)
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

    private int GetSortingDirectionMultiplier()
    {
        return SortDirection is ListSortDirection.Ascending ? 1 : -1;
    }
}

/// <summary>
/// A special case of <see cref="AbstractComparer{TItem,TCompareBy}"/> where <typeparamref name="TItem"/>'s are compared by themselves.
/// </summary>
/// <typeparam name="TItem">The type of items to compare.</typeparam>
public abstract class AbstractComparer<TItem> : AbstractComparer<TItem, TItem> where TItem : class
{
    protected sealed override TItem? GetCompareByMember(TItem? item) => item;
}
