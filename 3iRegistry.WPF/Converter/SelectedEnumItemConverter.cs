using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;

namespace _3iRegistry.WPF.Converter
{
    class SelectedEnumItemConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (int)value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int enumValue = (int)value;

            if (enumValue == -1)
                return Enum.ToObject(targetType, System.Convert.ToByte(0));
            else
                return Enum.ToObject(targetType, System.Convert.ToByte(value));
        }
    }
}
