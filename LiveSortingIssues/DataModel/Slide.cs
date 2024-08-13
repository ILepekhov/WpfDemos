namespace LiveSortingIssues.DataModel;

public sealed class Slide(SlideId slideId)
{
    public SlideId Id { get; } = slideId;

    public string? Name { get; set; }

    public string? Folder { get; set; }

    public SlideStatus Status { get; set; }
}
