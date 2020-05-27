using _3iRegistry.Core;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;

namespace _3iRegistry.WPF.Converter
{
    public class SpouseConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string output = string.Empty;
            var spouses = (List<Partner>)value;

            if(spouses != null)
            {
                for (int i = 0; i < spouses.Count; i++)
                {
                    var item = spouses[i];
                    output += $"{item.FirstName} {item.LastName}({item.MaritalStatus})";
                    if (i != spouses.Count - 1)
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