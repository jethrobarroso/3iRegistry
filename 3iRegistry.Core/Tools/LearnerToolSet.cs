using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace _3iRegistry.Core.Tools
{
    public class LearnerToolSet
    {
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
                    throw new ExcelImportException()
                    {
                        ErrorInfo = $"Not a correct date format",
                        CellData = entityValues[4].Trim()
                    };

                list.Add(learner);
            }

            return list;
        }

        private static List<string> LoadSchools()
        {
            return new List<string>()
            {
                "Relebogile Secondary",
                "Wildfontein Primary Farm",
                "Lemao Primary Farm",
                "Dinglo Primary Farm",
                "Hlangabeza Primary",
                "Phororong Primary",
                "Nayaboswa Primary",
                "Kamohelo Primary",
                "Tswasongu Secondary",
                "Badirile Secondary",
                "Mbulelo Primary",
                "Hlanganani Primary",
                "Khutsong South Primary",
                "Itumeleng Primary",
                "Denzel Primary",
            };
        }

        //public static List<Learner> StringToLearners(string value)
        //{
        //    List<Learner> list = new List<Learner>();
        //    var splitItems = value.Split(',');
        //    var fullNameSplit = splitItems[0].Split(' ');

        //    string firstName = fullNameSplit[0];
        //    string lastName = fullNameSplit[1];

        //    string school = splitItems[1];

        //    list.Add(new Learner()
        //    {
        //        FirstName = firstName,
        //        LastName = lastName,
        //        School = school
        //    });

        //    return list;
        //}
    }
}
