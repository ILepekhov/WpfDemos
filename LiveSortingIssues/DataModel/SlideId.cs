namespace LiveSortingIssues.DataModel;

/// <summary>
/// Defines a unique identifier (position) of a slide in the drum.
/// </summary>
/// <param name="Row">The row the magazine is in. 1-based</param>
/// <param name="Column">The column the magazine is in. 1-based</param>
/// <param name="Slot">Defines the position of the slide in the magazine. 1-based</param>
public readonly record struct SlideId(int Row, int Column, int Slot) : IComparable<SlideId>, IComparable
{
    public const byte StartingSlideID = 1;

    /// <summary>
    /// Gets the value describing the identifier of the parent magazine.
    /// </summary>
    public MagazineId MagazineId => new(Row, Column);

    public int CompareTo(SlideId other)
    {
        var magazinesScore = MagazineId.CompareTo(other.MagazineId);

        return magazinesScore != 0
            ? magazinesScore
            : Slot - other.Slot;
    }

    public int CompareTo(object? other)
    {
        if (ReferenceEquals(null, other))
        {
            return 1;
        }

        return other is SlideId otherId
            ? CompareTo(otherId)
            : throw new ArgumentException($"Object must be of type {nameof(SlideId)}");
    }

    public override string ToString() => $"{MagazineId.ToString()}-{Slot}";
}
