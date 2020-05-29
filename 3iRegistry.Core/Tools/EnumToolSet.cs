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
            string errorList = string.Empty;
            string enumDescription = string.Empty;

            foreach (TEnum item in (TEnum[])Enum.GetValues(typeof(TEnum)))
            {
                enumDescription = item.GetDescription();
                if ((value.ToLower() == item.GetDescription().ToLower()) ||
                    (value.ToLower() == item.ToString().ToLower()))
                {
                    convertedVal = item;
                    found = true;
                }
                errorList += $"- {enumDescription}\n";
            }

            if (!found)
            {
                string tempValue = string.IsNullOrEmpty(value) ? "<EMPTY>" : value;
                string message = $"The input \"{tempValue}\" " +
                    $"does not match any of the following values:\n{errorList.Trim()}";
                throw new CoreEnumConverterException(message)
                {
                    EnumValue = value
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
