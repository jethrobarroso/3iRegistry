using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace _3iRegistry.Core.Extensions
{
    public static class CoreExtensions
    {
        /// <summary>
        /// Returns a string representation of the Enum if it has
        /// the DescriptionAttribute applied to its values.
        /// </summary>
        /// <param name="value">Enum type</param>
        /// <returns>Returns the Enum value's attribute string or 
        /// the name value if no attribute was applied</returns>
        public static string GetDescription(this Enum value)
        {
            Type type = value.GetType();
            string name = Enum.GetName(type, value);
            if (name != null)
            {
                FieldInfo field = type.GetField(name);
                if (field != null)
                {
                    DescriptionAttribute attr = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;
                    if (attr != null)
                    {
                        return attr.Description;
                    }
                    else
                        return value.ToString();
                }
            }
            return null;
        }

        /// <summary>
        /// Print a ToString version for a generic list containing a custom entity type
        /// </summary>
        /// <typeparam name="T">Custom entity</typeparam>
        /// <param name="items">List if custom entity</param>
        /// <returns>A human readable version of the beneficiary object</returns>
        public static string GetListString<T>(this List<T> items)
        {
            string listString = String.Empty;

            if (items != null)
            {
                for (int i = 0; i < items.Count; i++)
                {
                    T item = items[i];
                    listString += $"{item}";
                    if (i != items.Count - 1)
                    {
                        listString += "; ";
                    }
                }
            }

            return listString;
        }
    }
}
