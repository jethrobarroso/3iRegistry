using _3iRegistry.Core;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;

namespace _3iRegistry.WPF.Converter
{
    public class LearnerConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string output = string.Empty;
            var learners = (List<Learner>)value;

            if(learners != null)
            {
                for (int i = 0; i < learners.Count; i++)
                {
                    var item = learners[i];
                    output += $"{item.FirstName} {item.LastName}";
                    if (i != learners.Count - 1)
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
