using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace _3iRegistry.Core.Tools
{
    public class CollectionToolset
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

        public static List<Learner> StringToLearners(string value)
        {
            List<Learner> list = new List<Learner>();

            if (string.IsNullOrEmpty(value))
                return list;

            var splitEntities = value.Split(';');

            Learner learner;

            foreach (var entity in splitEntities)
            {
                var entityValues = entity.Split(',');

                if (entityValues.Count() == 1)
                    break;
                learner = new Learner();

                learner.FirstName = entityValues[0].Trim();
                learner.LastName = entityValues[1].Trim();
                learner.Gender = EnumToolSet<Gender>.ConvertToEnumValue(entityValues[2]);
                learner.School = entityValues[4].Trim();
                learner.Grade = entityValues[5].Trim();

                if (DateTime.TryParse(entityValues[3].Trim(), out DateTime dob))
                    learner.DOB = dob;
                else
                    throw new CoreEnumConverterException($"Not a correct date format")
                    {
                        EnumValue = entityValues[4].Trim()
                    };

                list.Add(learner);
            }

            return list;
        }

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

        public static List<BuildingSnag> StringToSnags(string snagsString)
        {
            List<BuildingSnag> list = new List<BuildingSnag>();

            if (string.IsNullOrEmpty(snagsString))
                return list;

            return list;
        }
    }
}
