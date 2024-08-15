using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Channels;
using System.Windows.Data;
using LiveSortingIssues.Components.Shared;
using LiveSortingIssues.Components.Slides.Grouping;
using LiveSortingIssues.DataModel;

namespace LiveSortingIssues.Components.Slides;

public sealed class SlidesViewModel : ViewModelBase
{
    private int _slideCount;

    public SlidesViewModel(Drum drum)
    {
        ObservableCollection<SlideViewModel> slideViewModels = new(drum.GetSlides().Select(s => new SlideViewModel(s)));

        SlidesView = ConfigureSlidesView(slideViewModels, SlideProperty.Position, ListSortDirection.Ascending);
        SlideCount = slideViewModels.Count;

        InitiateSlidePresenceUpdatesHandling(slideViewModels, out var slidePresenceUpdatesInput);

        SubscribeToSlidePresenceUpdates(drum, slidePresenceUpdatesInput);

        slideViewModels.CollectionChanged += (_, _) => SlideCount = slideViewModels.Count;
    }

    public ICollectionView SlidesView { get; }

    public int SlideCount
    {
        get => _slideCount;
        private set => SetField(ref _slideCount, value);
    }

    public void UpdateDisplayingParameters(SlideProperty sortByProperty, ListSortDirection sortDirection)
    {
        var slidesView = (SlidesView as ListCollectionView)!;

        using var deferredRefresh = slidesView.DeferRefresh();

        slidesView.GroupDescriptions!.Clear();
        slidesView.LiveGroupingProperties.Clear();
        slidesView.LiveSortingProperties.Clear();

        slidesView.CustomSort = SlideComparer.GetComparer(sortByProperty, sortDirection);

        var propertyName = GroupingHelper.GetPropertyName(sortByProperty);

        slidesView.LiveSortingProperties.Add(propertyName);
        slidesView.LiveGroupingProperties.Add(propertyName);
        slidesView.GroupDescriptions.Add(GroupingHelper.GetGroupDescription(sortByProperty, sortDirection));
    }

    private static ListCollectionView ConfigureSlidesView(ObservableCollection<SlideViewModel> slideViewModels,
        SlideProperty sortByProperty,
        ListSortDirection sortDirection)
    {
        var slidesView = (CollectionViewSource.GetDefaultView(slideViewModels) as ListCollectionView)!;

        var propertyName = GroupingHelper.GetPropertyName(sortByProperty);

        slidesView.GroupDescriptions!.Clear();
        slidesView.GroupDescriptions.Add(GroupingHelper.GetGroupDescription(sortByProperty, sortDirection));
        slidesView.LiveGroupingProperties.Add(propertyName);
        slidesView.IsLiveGrouping = true;

        slidesView.LiveSortingProperties.Clear();
        slidesView.LiveSortingProperties.Add(propertyName);
        slidesView.IsLiveSorting = true;
        slidesView.CustomSort = SlideComparer.GetComparer(sortByProperty, sortDirection);

        return slidesView;
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
