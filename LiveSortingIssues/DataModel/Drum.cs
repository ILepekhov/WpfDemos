﻿namespace LiveSortingIssues.DataModel;

public sealed class Drum
{
    private readonly Dictionary<MagazineId, Magazine?> _magazines;

    public event Action<Slide>? SlideAdded;

    public event Action<Slide>? SlideRemoved;

    public Drum(int columnsCount, int rowsCount)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(columnsCount);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(rowsCount);

        _magazines = GenerateMagazineIds(columnsCount, rowsCount).ToDictionary(x => x, _ => default(Magazine));
    }

    public Slide[] GetSlides()
    {
        return _magazines.Values
            .Where(m => m is not null)
            .SelectMany(m => m!.GetSlides())
            .OrderBy(s => s.Id)
            .ToArray();
    }

    public MagazineId[] GetAvailableMagazineIds() => _magazines.Keys.ToArray();

    public bool HasMagazine(MagazineId magazineId) => _magazines.TryGetValue(magazineId, out var magazine) &&
                                                      magazine is not null;

    public void AddMagazine(Magazine magazine)
    {
        if (_magazines.TryGetValue(magazine.Id, out var existingMagazine) && existingMagazine is not null)
        {
            throw new InvalidOperationException($"Magazine {magazine.Id} already exists.");
        }

        _magazines[magazine.Id] = magazine;

        foreach (var slide in magazine.GetSlides())
        {
            SlideAdded?.Invoke(slide);
        }
    }

    public void RemoveMagazine(MagazineId magazineId)
    {
        if (_magazines.Remove(magazineId, out var magazine) is false)
        {
            return;
        }

        if (magazine is null)
        {
            return;
        }

        foreach (var slide in magazine.GetSlides())
        {
            SlideRemoved?.Invoke(slide);
        }
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
