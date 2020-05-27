using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace _3iRegistry.WPF.Extensions
{
    public static class ListExtensions
    {
        public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> collection)
        {
            var c = new ObservableCollection<T>();
            foreach(var item in collection)
                c.Add(item);
            return c;
        }
    }
}
