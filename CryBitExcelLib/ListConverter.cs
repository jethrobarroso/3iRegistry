using _3iRegistry.Core;
using _3iRegistry.Core.Extensions;
using _3iRegistry.Core.Tools;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using System;
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

            return text;
        }

        public string ConvertToString(object value, IWriterRow row, MemberMapData memberMapData) 
        {
            var list = value as List<T>;

            return list.GetListString();
        }
    }

    public class ActualString<T> : ITypeConverter
    {
        public object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
        {
            if (text[0] == '\'')
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

    public class BeneficiaryMap : ClassMap<Beneficiary>
    {
        public BeneficiaryMap()
        {
            Map(m => m.PersonId).Name("ID").TypeConverter<ActualString<string>>().Index(0);
            Map(m => m.LastName).Name("SURNAME").Index(1);
            Map(m => m.FirstName).Name("NAMES").Index(2);
            Map(m => m.Gender).Name("GENDER").Index(3);
            Map(m => m.Hop.Project).Name("PROJECT").Index(4);
            Map(m => m.Hop.Block).Name("BLOCK").Index(5);
            Map(m => m.Hop.Unit).Name("UNIT").Index(6);
            Map(m => m.Hop.Elec).Name("ELECTRICITY").Index(7);
            Map(m => m.Hop.WaterM).Name("WATER (M)").Index(8);
            Map(m => m.Hop.WaterE).Name("WATER (E)").TypeConverter<ActualString<string>>().Index(9);
            Map(m => m.Contact).Name("PRIMARY CONTACT").TypeConverter<ActualString<string>>().Index(10);
            Map(m => m.AltContact).Name("ALT. CONTACT").TypeConverter<ActualString<string>>().Index(11);
            Map(m => m.Email).Name("EMAIL").Index(12);
            Map(m => m.Team).Name("TEAM").Index(13);
            Map(m => m.Settlement).Name("SETTLEMENT").Index(14);
            Map(m => m.Address).Name("ADDRESS").Index(15);
            Map(m => m.Furniture).Name("FURNITURE").TypeConverter<ListConverter<Furniture>>().Index(16);
            Map(m => m.Partners).Name("PARTNER(S)").TypeConverter<ListConverter<Partner>>().Index(17);
            Map(m => m.Learners).Name("LEARNER(S)").TypeConverter<ListConverter<Learner>>().Index(18);
            Map(m => m.DSTV).Name("DSTV").Index(19);
            Map(m => m.HouseholdMemberCount).Name("HOUSEHOLD MEMBERS").Index(20);
            Map(m => m.UnemployedCount).Name("UNEMPLOYED").Index(21);
            Map(m => m.GrantCount).Name("GRANT COUNT").Index(22);
            Map(m => m.IllnessCount).Name("CRHONIC ILLNESSES").Index(23);
            Map(m => m.IllnessDescription).Name("ILLNESS DESCRIPTION").Index(24);
            Map(m => m.GrantDescription).Name("GRANT DESCRIPTION").Index(25);
            Map(m => m.Notes).Name("NOTES").Index(26);
        }
    }
}
