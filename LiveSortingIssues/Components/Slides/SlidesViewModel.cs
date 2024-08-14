using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;
using LiveSortingIssues.Components.Shared;
using LiveSortingIssues.DataModel;

namespace LiveSortingIssues.Components.Slides;

public sealed class SlidesViewModel : ViewModelBase
{
    private readonly IDrumManager _drumManager;
    private readonly ObservableCollection<SlideViewModel> _slideViewModels;

    public SlidesViewModel(IDrumManager drumManager)
    {
        _drumManager = drumManager;
        _slideViewModels =
            new ObservableCollection<SlideViewModel>(drumManager.GetSlides().Select(s => new SlideViewModel(s)));

        SlidesView = ConfigureSlidesView(_slideViewModels, SlideProperty.Position, ListSortDirection.Ascending);
    }

    public ICollectionView SlidesView { get; }

    private static ListCollectionView ConfigureSlidesView(ObservableCollection<SlideViewModel> slideViewModels,
        SlideProperty sortByProperty,
        ListSortDirection sortDirection)
    {
        var slidesView = (CollectionViewSource.GetDefaultView(slideViewModels) as ListCollectionView)!;

        slidesView.GroupDescriptions!.Clear();
        slidesView.GroupDescriptions.Add(new PropertyGroupDescription(GetPropertyName(sortByProperty)));
        slidesView.IsLiveGrouping = true;

        slidesView.LiveSortingProperties.Clear();
        slidesView.LiveSortingProperties.Add(GetPropertyName(sortByProperty));
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
}
