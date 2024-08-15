using LiveSortingIssues.DataModel;
using LiveSortingIssues.Resources;

namespace LiveSortingIssues.Components.Slides;

public sealed record SlideGroupName(string Name, SlideStatusGroup StatusGroup)
{
    public static SlideGroupName GetGroupName(SlideViewModel viewModel, SlideProperty property)
    {
        var statusGroup = GetGroup(viewModel.Status);

        return property switch
        {
            SlideProperty.Position => new SlideGroupName(viewModel.MagazinePosition, statusGroup),
            SlideProperty.Folder => new SlideGroupName(viewModel.Folder ?? "None", statusGroup),
            SlideProperty.Status => new SlideGroupName(GetStatusGroupName(statusGroup), statusGroup),
            _ => throw new ArgumentException($"Unknown Slide Group: {property}", nameof(property))
        };
    }

    public override string ToString() => Name;

    private static SlideStatusGroup GetGroup(SlideStatus status)
    {
        return status switch
        {
            SlideStatus.New => SlideStatusGroup.New,
            SlideStatus.WaitingForProcessing or SlideStatus.Processing => SlideStatusGroup.ProcessingQueue,
            SlideStatus.Done => SlideStatusGroup.Done,
            _ => throw new ArgumentOutOfRangeException(nameof(status), status, null)
        };
    }

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
