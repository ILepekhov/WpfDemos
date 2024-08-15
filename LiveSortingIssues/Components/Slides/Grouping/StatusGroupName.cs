using LiveSortingIssues.Resources;

namespace LiveSortingIssues.Components.Slides.Grouping;

public sealed record StatusGroupName(SlideStatusGroup StatusGroup) : IComparable<StatusGroupName>, IComparable
{
    public int CompareTo(StatusGroupName? other)
    {
        if (ReferenceEquals(this, other)) return 0;
        if (ReferenceEquals(null, other)) return 1;

        return StatusGroup - other.StatusGroup;
    }

    public int CompareTo(object? other) => CompareTo(other as StatusGroupName);

    public override string ToString() => GetStatusGroupName(StatusGroup);

    private static string GetStatusGroupName(SlideStatusGroup group)
    {
        return group switch
        {
            SlideStatusGroup.New => Strings.SlideStatusGroup_New,
            SlideStatusGroup.ProcessingQueue => Strings.SlideStatusGroup_ProcessingQueue,
            SlideStatusGroup.Done => Strings.SlideStatusGroup_Done,
            _ => throw new ArgumentOutOfRangeException(nameof(group), group, null)
        };
    }
}
