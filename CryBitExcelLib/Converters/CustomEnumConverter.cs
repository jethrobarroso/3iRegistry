using _3iRegistry.Core.Tools;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using System;

namespace CryBitExcelLib
{
    public class CustomEnumConverter<T> : ITypeConverter where T : Enum
    {
        public object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
        {
            try
            {
                return EnumToolSet<T>.ConvertToEnumValue(text);
            }
            catch(CoreEnumConverterException ex)
            {
                ex.ErrorOriginatorType = typeof(T);
                throw;
            }
        }

        public string ConvertToString(object value, IWriterRow row, MemberMapData memberMapData)
        {
            return value.ToString();
        }
    }
}
