namespace LiveSortingIssues.DataModel;

/// <summary>
/// Defines a unique identifier (position) of a magazine in the drum.
/// </summary>
/// <param name="Row">The row the magazine is in. 1-based</param>
/// <param name="Column">The column the magazine is in. 1-based</param>
public readonly record struct MagazineId(int Row, int Column) : IComparable<MagazineId>, IComparable
{
    public const byte StartingMagazineRowID = 1;
    public const byte StartingMagazineColID = 1;

    public const char MinColumnLetter = 'A';

    public override string ToString() => $"{GetColumnLetter(Column)}{Row}";

    public int CompareTo(object? other)
    {
        if (ReferenceEquals(null, other))
        {
            return 1;
        }

        return other is MagazineId otherId
            ? CompareTo(otherId)
            : throw new ArgumentException($"Object must be of type {nameof(MagazineId)}");
    }

    private static char GetColumnLetter(int column) => (char)(MinColumnLetter + column - StartingMagazineColID);

    public int CompareTo(MagazineId other) => GetComparisonScore(this) - GetComparisonScore(other);

    private static int GetComparisonScore(MagazineId magazineId) => magazineId.Column * 10_000 + magazineId.Row * 100;
}
