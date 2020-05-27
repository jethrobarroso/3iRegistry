using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace _3iRegistry.Core.Tools
{
    public class FurnitureToolSet
    {
        public static List<Furniture> StringToFurniture(string value)
        {
            List<Furniture> list = new List<Furniture>();
            if (string.IsNullOrEmpty(value))
                return list;

            var splitEntities = value.Split(';');
            Regex regexName = new Regex(@"(\d *)x(.*)", RegexOptions.IgnoreCase);

            foreach (var item in splitEntities)
            {
                var match = regexName.Match(item);
                list.Add(new Furniture()
                {
                    Qty = int.TryParse(match.Groups[1].Value, out int val) ? val : 0,
                    Name = match.Groups[2].Value
                });
            }
            return list;
        }

        //public static List<Furniture> StringToFurniture(string value)
        //{
        //    List<Furniture> list = new List<Furniture>();
        //    var splitEntities = value.Split(';');
        //    Match match;

        //    foreach (var item in splitEntities)
        //    {
        //        match = Regex.Match(item, @"(\d)\s*([a-zA-Z]*)");

        //        list.Add(new Furniture()
        //        {
        //            Qty = int.Parse(match.Groups[1].Value),
        //            Name = match.Groups[2].Value
        //        });
        //    }
        //    return list;
        //}
    }
}
