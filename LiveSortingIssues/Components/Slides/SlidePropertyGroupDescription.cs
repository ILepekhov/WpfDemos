using System.ComponentModel;
using System.Globalization;

namespace LiveSortingIssues.Components.Slides;

public sealed class SlidePropertyGroupDescription(SlideProperty property) : GroupDescription
{
    private SlideProperty _property = property;

    public SlideProperty Property
    {
        get => _property;
        set
        {
            _property = value;
            OnPropertyChanged(new PropertyChangedEventArgs(nameof(Property)));
        }
    }

    public override object GroupNameFromItem(object item, int level, CultureInfo culture)
    {
        if (item is not SlideViewModel slideViewModel)
        {
            throw new InvalidOperationException();
        }

        return SlideGroupName.GetGroupName(slideViewModel, Property);
    }

    public override bool NamesMatch(object groupName, object itemName)
    {
        if (groupName is SlideGroupName gName && itemName is SlideGroupName iName)
        {
            return string.Equals(gName.Name, iName.Name, StringComparison.OrdinalIgnoreCase);
        }

        return Equals(groupName, itemName);
    }

    public void SetSortDirection(ListSortDirection sortDirection)
    {
        CustomSort = SlideGroupComparer.GetComparer(Property, sortDirection);
    }
}
