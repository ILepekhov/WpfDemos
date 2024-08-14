using LiveSortingIssues.Components.Shared;
using LiveSortingIssues.DataModel;

namespace LiveSortingIssues.Components.Slides;

public sealed class SlideViewModel(Slide slide) : ViewModelBase
{
    private readonly Slide _slide = slide;
    private bool _isSelected;

    public string Position => _slide.Id.ToString();

    public string MagazinePosition => _slide.Id.MagazineId.ToString();

    public string? Name
    {
        get => _slide.Name;
        set
        {
            _slide.Name = value;
            OnPropertyChanged();
        }
    }

    public SlideStatus Status
    {
        get => _slide.Status;
        set
        {
            _slide.Status = value;
            OnPropertyChanged();
        }
    }

    public string? Folder
    {
        get => _slide.Folder;
        set
        {
            _slide.Folder = value;
            OnPropertyChanged();
        }
    }

    public bool IsSelected
    {
        get => _isSelected;
        set => SetField(ref _isSelected, value);
    }

    public Slide GetModel() => _slide;
}
