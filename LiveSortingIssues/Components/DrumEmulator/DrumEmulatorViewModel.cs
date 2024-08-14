using System.Collections.ObjectModel;
using System.Windows.Input;
using LiveSortingIssues.Components.Shared;
using LiveSortingIssues.DataModel;

namespace LiveSortingIssues.Components.DrumEmulator;

public sealed class DrumEmulatorViewModel : ViewModelBase
{
    private const int MagazineCapacity = 12;

    private readonly Drum _drum;
    private MagazineId _selectedMagazine;

    public DrumEmulatorViewModel(Drum drum)
    {
        _drum = drum;

        Magazines = new ReadOnlyObservableCollection<MagazineId>(
            new ObservableCollection<MagazineId>(drum.GetAvailableMagazineIds()));

        SelectedMagazine = Magazines.FirstOrDefault();

        AddMagazineCommand = new DelegateCommand(_ => AddMagazine());
        RemoveMagazineCommand = new DelegateCommand(_ => RemoveMagazine());
    }

    public ReadOnlyObservableCollection<MagazineId> Magazines { get; }

    public MagazineId SelectedMagazine
    {
        get => _selectedMagazine;
        set => SetField(ref _selectedMagazine, value);
    }

    public ICommand AddMagazineCommand { get; }

    public ICommand RemoveMagazineCommand { get; }

    private void AddMagazine()
    {
        var magazineId = SelectedMagazine;

        if (magazineId == default)
        {
            return;
        }

        Task.Run(() =>
        {
            if (_drum.HasMagazine(magazineId))
            {
                _drum.RemoveMagazine(magazineId);
            }

            _drum.AddMagazine(GenerateMagazine(magazineId));
        });
    }

    private static Magazine GenerateMagazine(MagazineId magazineId)
    {
        Magazine magazine = new(magazineId);

        for (var i = 0; i <= Random.Shared.Next(1, MagazineCapacity); i++)
        {
            magazine.AddSlide(new Slide(new SlideId(magazineId.Row, magazineId.Column, i + 1)));
        }

        return magazine;
    }

    private void RemoveMagazine()
    {
        var magazineId = SelectedMagazine;

        if (magazineId == default)
        {
            return;
        }

        Task.Run(() =>
        {
            _drum.RemoveMagazine(magazineId);
        });
    }
}
