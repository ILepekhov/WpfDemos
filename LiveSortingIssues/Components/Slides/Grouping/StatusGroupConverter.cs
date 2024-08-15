using System.Globalization;
using System.Windows.Data;
using LiveSortingIssues.DataModel;

namespace LiveSortingIssues.Components.Slides.Grouping;

[ValueConversion(typeof(SlideStatus), typeof(StatusGroupName))]
public class StatusGroupConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not SlideStatus slideStatus)
        {
            return new ArgumentNullException(nameof(value));
        }

        var statusGroup = slideStatus switch
        {
            SlideStatus.New => SlideStatusGroup.New,
            SlideStatus.WaitingForProcessing or SlideStatus.Processing => SlideStatusGroup.ProcessingQueue,
            SlideStatus.Done => SlideStatusGroup.Done,
            _ => throw new ArgumentOutOfRangeException(nameof(value), value, null)
        };

        return new StatusGroupName(statusGroup);
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return Binding.DoNothing;
    }
}
