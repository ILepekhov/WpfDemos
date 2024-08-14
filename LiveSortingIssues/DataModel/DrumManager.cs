namespace LiveSortingIssues.DataModel;

public interface IDrumManager
{
    Slide[] GetSlides();
}

public sealed class DrumManager : IDrumManager
{
    private readonly Drum _drum;

    public DrumManager(Drum drum)
    {
        _drum = drum;
    }

    public Slide[] GetSlides() => _drum.GetSlides();
}
