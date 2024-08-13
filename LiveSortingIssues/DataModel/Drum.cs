namespace LiveSortingIssues.DataModel;

public sealed class Drum
{
    private readonly Dictionary<MagazineId, Magazine?> _magazines;

    public Drum(int columnsCount, int rowsCount)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(columnsCount);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(rowsCount);

        _magazines = GenerateMagazineIds(columnsCount, rowsCount).ToDictionary(x => x, _ => default(Magazine));
    }

    private static IEnumerable<MagazineId> GenerateMagazineIds(int columnsCount, int rowsCount)
    {
        for (var col = 0; col < columnsCount; col++)
        {
            for (var row = 0; row < rowsCount; row++)
            {
                yield return new MagazineId(
                    row + MagazineId.StartingMagazineRowID,
                    col + MagazineId.StartingMagazineColID);
            }
        }
    }
}
