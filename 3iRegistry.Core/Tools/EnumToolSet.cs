using _3iRegistry.Core.Extensions;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Text;

namespace _3iRegistry.Core.Tools
{
    public static class EnumToolSet<TEnum> where TEnum : Enum
    {
        public static TEnum ConvertToEnumValue(string value)
        {
            TEnum convertedVal = default;
            bool found = false;

            foreach (TEnum item in (TEnum[])Enum.GetValues(typeof(TEnum)))
            {
                if (value.ToLower() == item.GetDescription().ToLower())
                {
                    convertedVal = item;
                    found = true;
                }
            }

            if (!found)
            {
                var exception = new ExcelImportException()
                {
                    ErrorInfo = $"No matches on any of the accepted items:\n" +
                    $"Cohabiting\nEngaged\nMarried in community of property\n" +
                    $"Married out of community of property",
                    CellData = value
                };
            }
            return convertedVal;
        }

        public static List<string> ConvertToList()
        {
            List<string> list = new List<string>();

            foreach (TEnum item in (TEnum[])Enum.GetValues(typeof(TEnum)))
            {
                list.Add(item.GetDescription());
            }

            return list;
        }
    }
}
