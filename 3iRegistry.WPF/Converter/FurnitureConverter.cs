using _3iRegistry.Core;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;

namespace _3iRegistry.WPF.Converter
{
    public class FurnitureConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string output = string.Empty;
            var furniture = (List<Furniture>)value;

            if(furniture != null)
            {
                for (int i = 0; i < furniture.Count; i++)
                {

                    var item = furniture[i];
                    output += $"{item.Name}({item.Qty})";
                    if (i != furniture.Count - 1)
                        output += ", ";
                }
            }
            
            return output;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
