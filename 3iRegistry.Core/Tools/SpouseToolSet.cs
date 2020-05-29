using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace _3iRegistry.Core.Tools
{
    public class SpouseToolSet
    {
        public static List<Partner> StringToSpouse(string value)
        {
            List<Partner> list = new List<Partner>();
            if (string.IsNullOrEmpty(value))
                return list;

            var splitEntities = value.Split(';');

            //Match match;
            //string expression = @"^\s*?(?<id>\d*),(?<fname>[a-z]*),(?<lname>[a-z]*),(?<gender>(Male|Female)),(?<dob>\d*[\/\\-]\d*[\/\\-]\d*),(?<status>.[a-z]*)\s*$";

            Partner partner;

            foreach (var entity in splitEntities)
            {
                var entityValues = entity.Split(',');
                partner = new Partner();

                partner.PersonId = entityValues[0].Trim();
                partner.FirstName = entityValues[1].Trim();
                partner.LastName = entityValues[2].Trim();
                partner.Gender = EnumToolSet<Gender>.ConvertToEnumValue(entityValues[3]);
                partner.MaritalStatus = EnumToolSet<MaritalStatus>.ConvertToEnumValue(entityValues[5]);

                if (DateTime.TryParse(entityValues[4].Trim(), out DateTime dob))
                    partner.DOB = dob;
                else
                    throw new CoreEnumConverterException($"Not a correct date format")
                    {
                        EnumValue = entityValues[4].Trim()
                    };

                list.Add(partner);
            }

            return list;
        }
    }
}
