using _3iRegistry.Core;
using _3iRegistry.Core.Extensions;
using _3iRegistry.Core.Tools;
using CryBitExcelLib.Exceptions;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using System.CodeDom;
using System.Collections.Generic;
using System.Text;

namespace CryBitExcelLib
{
    public class ListConverter<T> : ITypeConverter
    {
        public object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
        {
            if (typeof(Partner).IsAssignableFrom(typeof(T)))
            {
                return SpouseToolSet.StringToSpouse(text);
            }

            if (typeof(Furniture).IsAssignableFrom(typeof(T)))
            {
                return FurnitureToolSet.StringToFurniture(text);
            }

            if (typeof(Learner).IsAssignableFrom(typeof(T)))
            {
                return LearnerToolSet.StringToLearners(text);
            }

            string message = $"The value could not be parsed.\n";
            throw new CsvImportException(message)
            {
                CellData = text,
                Cell = $"Column: {memberMapData.Index}",
                ErrorOriginatorType = typeof(T)
            };
        }

        public string ConvertToString(object value, IWriterRow row, MemberMapData memberMapData)
        {
            var list = value as List<T>;

            return list.GetListString();
        }
    }
}
