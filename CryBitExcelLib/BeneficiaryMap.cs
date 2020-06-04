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
    public class BeneficiaryMap : ClassMap<Beneficiary>
    {
        public BeneficiaryMap()
        {
            Map(m => m.PersonId).Name("ID").TypeConverter<ApostropheStringConverter<string>>().Index(0);
            Map(m => m.LastName).Name("SURNAME").Index(1);
            Map(m => m.FirstName).Name("NAMES").Index(2);
            Map(m => m.Gender).Name("GENDER").TypeConverter<CustomEnumConverter<Gender>>().Index(3);
            Map(m => m.Hop.Project).Name("PROJECT").Index(4);
            Map(m => m.Hop.Block).Name("BLOCK").Index(5);
            Map(m => m.Hop.Unit).Name("UNIT").Index(6);
            Map(m => m.Hop.Elec).Name("ELECTRICITY").TypeConverter<ApostropheStringConverter<string>>().Index(7);
            Map(m => m.Hop.WaterM).Name("WATER (M)").Index(8);
            Map(m => m.Hop.WaterE).Name("WATER (E)").TypeConverter<ApostropheStringConverter<string>>().Index(9);
            Map(m => m.Contact).Name("PRIMARY CONTACT").TypeConverter<ApostropheStringConverter<string>>().Index(10);
            Map(m => m.AltContact).Name("ALT. CONTACT").TypeConverter<ApostropheStringConverter<string>>().Index(11);
            Map(m => m.Email).Name("EMAIL").Index(12);
            Map(m => m.Team).Name("TEAM").Index(13);
            Map(m => m.Settlement).Name("SETTLEMENT").Index(14);
            Map(m => m.Address).Name("ADDRESS").Index(15);
            Map(m => m.Furniture).Name("FURNITURE").TypeConverter<ListConverter<Furniture>>().Index(16);
            Map(m => m.Partners).Name("PARTNER(S)").TypeConverter<ListConverter<Partner>>().Index(17);
            Map(m => m.Learners).Name("LEARNER(S)").TypeConverter<ListConverter<Learner>>().Index(18);
            Map(m => m.DSTV).Name("DSTV").TypeConverter<CustomEnumConverter<DSTVState>>().Index(19);
            Map(m => m.HouseholdMemberCount).Name("HOUSEHOLD MEMBERS COUNT").Default(0).Index(20);
            Map(m => m.UnemployedCount).Name("UNEMPLOYED COUNT").Default(0).Index(21);
            Map(m => m.GrantCount).Name("GRANT COUNT").Default(0).Index(22);
            Map(m => m.IllnessCount).Name("CRHONIC ILLNESS COUNT").Default(0).Index(23);
            Map(m => m.IllnessDescription).Name("ILLNESS DESCRIPTION").Index(24);
            Map(m => m.GrantDescription).Name("GRANT DESCRIPTION").Index(25);
            Map(m => m.Notes).Name("NOTES").Index(26);
        }
    }
}
