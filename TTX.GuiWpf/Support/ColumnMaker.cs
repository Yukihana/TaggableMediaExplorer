using System;
using System.Globalization;
using System.Windows.Data;

namespace TTX.GuiWpf.Support;

internal class ColumnMaker : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        double n = (double)value / 600;
        if (n == 0)
            n = 1;
        return n;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => throw new NotImplementedException();
}