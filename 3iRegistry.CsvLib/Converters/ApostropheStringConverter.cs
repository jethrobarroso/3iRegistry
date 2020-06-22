using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;

namespace CryBitExcelLib
{
    public class ApostropheStringConverter<T> : ITypeConverter
    {
        public object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
        {
            if (!string.IsNullOrEmpty(text) && text[0] == '\'')
            {
                var newString = text.Substring(1);
                return newString;
            }
                
            return text;
        }

        public string ConvertToString(object value, IWriterRow row, MemberMapData memberMapData)
        {
            return "'" + value;
        }
    }
}
