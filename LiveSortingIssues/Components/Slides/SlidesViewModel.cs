using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Channels;
using System.Windows.Data;
using LiveSortingIssues.Components.Shared;
using LiveSortingIssues.DataModel;

namespace LiveSortingIssues.Components.Slides;

public sealed class SlidesViewModel : ViewModelBase
{
    private readonly SlidePropertyGroupDescription _groupDescription;

    public SlidesViewModel(Drum drum)
    {
        ObservableCollection<SlideViewModel> slideViewModels = new(drum.GetSlides().Select(s => new SlideViewModel(s)));

        _groupDescription = new SlidePropertyGroupDescription(SlideProperty.Position);

        SlidesView = ConfigureSlidesView(slideViewModels, _groupDescription, SlideProperty.Position, ListSortDirection.Ascending);

        InitiateSlidePresenceUpdatesHandling(slideViewModels, out var slidePresenceUpdatesInput);

        SubscribeToSlidePresenceUpdates(drum, slidePresenceUpdatesInput);
    }

    public ICollectionView SlidesView { get; }

    public void UpdateDisplayingParameters(ListSortDirection sortDirection, SlideProperty sortByProperty)
    {
        _groupDescription.Property = sortByProperty;
        _groupDescription.SetSortDirection(sortDirection);

        var slidesView = (SlidesView as ListCollectionView)!;

        slidesView.LiveGroupingProperties.Clear();
        slidesView.LiveSortingProperties.Clear();

        slidesView.CustomSort = SlideComparer.GetComparer(sortByProperty, sortDirection);

        var propertyName = GetPropertyName(sortByProperty);

        slidesView.LiveSortingProperties.Add(propertyName);
        slidesView.LiveGroupingProperties.Add(propertyName);
    }

    private static ListCollectionView ConfigureSlidesView(ObservableCollection<SlideViewModel> slideViewModels,
        GroupDescription groupDescription,
        SlideProperty sortByProperty,
        ListSortDirection sortDirection)
    {
        var slidesView = (CollectionViewSource.GetDefaultView(slideViewModels) as ListCollectionView)!;

        var propertyName = GetPropertyName(sortByProperty);

        slidesView.GroupDescriptions!.Clear();
        slidesView.GroupDescriptions.Add(groupDescription);
        slidesView.LiveGroupingProperties.Add(propertyName);
        slidesView.IsLiveGrouping = true;

        slidesView.LiveSortingProperties.Clear();
        slidesView.LiveSortingProperties.Add(propertyName);
        slidesView.IsLiveSorting = true;
        slidesView.CustomSort = SlideComparer.GetComparer(sortByProperty, sortDirection);

        return slidesView;
    }

    private static string GetPropertyName(SlideProperty slideProperty)
    {
        return slideProperty switch
        {
            SlideProperty.Position => nameof(SlideViewModel.MagazinePosition),
            SlideProperty.Folder => nameof(SlideViewModel.Folder),
            SlideProperty.Status => nameof(SlideViewModel.Status),
            _ => throw new ArgumentOutOfRangeException(nameof(slideProperty), slideProperty, null)
        };
    }

    private static void InitiateSlidePresenceUpdatesHandling(ObservableCollection<SlideViewModel> slideViewModels,
        out ChannelWriter<SlidePresenceUpdate> channelWriter)
    {
        var channel = Channel.CreateUnbounded<SlidePresenceUpdate>(new UnboundedChannelOptions
        {
            SingleReader = true,
            SingleWriter = false,
            AllowSynchronousContinuations = true
        });

        channelWriter = channel.Writer;

        _ = HandleSlidePresenceUpdates(slideViewModels, channel.Reader);
    }

    private static async Task HandleSlidePresenceUpdates(ObservableCollection<SlideViewModel> slideViewModels,
        ChannelReader<SlidePresenceUpdate> channelReader)
    {
        await foreach (var (slide, updateType) in channelReader.ReadAllAsync())
        {
            try
            {
                if (updateType is SlidePresenceUpdateType.Added)
                {
                    slideViewModels.Add(new SlideViewModel(slide));
                }
                else
                {
                    var toRemove = slideViewModels.FirstOrDefault(s => s.GetModel().Id == slide.Id);
                    if (toRemove is null)
                    {
                        continue;
                    }

                    toRemove.IsSelected = false;
                    slideViewModels.Remove(toRemove);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to handle slide presence update: {slide.Id} {updateType}");
                Console.WriteLine(ex);
            }
        }
    }

    private static void SubscribeToSlidePresenceUpdates(Drum drum, ChannelWriter<SlidePresenceUpdate> updatesChannelWriter)
    {
        drum.SlideAdded += slide =>
            updatesChannelWriter.TryWrite(new SlidePresenceUpdate(slide, SlidePresenceUpdateType.Added));

        drum.SlideRemoved += slide =>
            updatesChannelWriter.TryWrite(new SlidePresenceUpdate(slide, SlidePresenceUpdateType.Removed));
    }
}
