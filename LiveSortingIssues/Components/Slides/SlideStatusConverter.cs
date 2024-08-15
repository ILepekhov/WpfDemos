using System.Globalization;
using System.Windows.Data;
using LiveSortingIssues.DataModel;
using LiveSortingIssues.Resources;

namespace LiveSortingIssues.Components.Slides;

[ValueConversion(typeof(SlideStatus), typeof(string))]
public sealed class SlideStatusConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not SlideStatus slideStatus)
        {
            return Binding.DoNothing;
        }

        return slideStatus switch
        {
            SlideStatus.New => Strings.SlideStatus_New,
            SlideStatus.WaitingForProcessing => Strings.SlideStatus_WaitingForProcessing,
            SlideStatus.Processing => Strings.SlideStatus_Processing,
            SlideStatus.Done => Strings.SlideStatus_Done,
            _ => throw new ArgumentOutOfRangeException(nameof(value), value, null)
        };
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return Binding.DoNothing;
    }
}
