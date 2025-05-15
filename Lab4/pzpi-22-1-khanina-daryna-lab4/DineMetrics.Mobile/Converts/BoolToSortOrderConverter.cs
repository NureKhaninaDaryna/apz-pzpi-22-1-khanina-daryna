using System.Globalization;

namespace DineMetrics.Mobile.Converts
{
    public class BoolToSortOrderConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool ascending)
                return ascending ? "Sort: ↑ Ascending" : "Sort: ↓ Descending";

            return "Sort";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
