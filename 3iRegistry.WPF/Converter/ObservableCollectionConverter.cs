using _3iRegistry.Core;
using _3iRegistry.WPF.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Text;
using System.Windows.Data;

namespace _3iRegistry.WPF.Converter
{
    public class ObservableCollectionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var source = (List<Partner>)value;
            return source.ToObservableCollection();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return new List<Partner>((ObservableCollection<Partner>)value);
        }
    }
}
