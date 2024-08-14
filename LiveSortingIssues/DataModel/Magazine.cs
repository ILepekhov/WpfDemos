namespace LiveSortingIssues.DataModel;

public sealed class Magazine(MagazineId magazineId)
{
    private readonly Dictionary<SlideId, Slide> _slides = [];

    public MagazineId Id { get; } = magazineId;

    public int Count => _slides.Count;

    public Slide[] GetSlides() => _slides.Values.OrderBy(slide => slide.Id).ToArray();

    public void AddSlide(Slide slide)
    {
        if (slide.Id.MagazineId != Id)
        {
            throw new InvalidOperationException();
        }

        if (_slides.TryAdd(slide.Id, slide) is false)
        {
            throw new InvalidOperationException();
        }
    }
}
