using _3iRegistry.Core;
using CryBitExcelLib;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _3iRegistry.Test.CsvLib
{
    public class ListConverterTest
    {
        [Test]
        public void ConvertFromString_ReturnListOfSnag()
        {
            var converter = new ListConverter<BuildingSnag>();

            var actualObject = converter.ConvertFromString("D1,C1", null, null);

            Assert.IsAssignableFrom(typeof(List<BuildingSnag>), actualObject);
        }

        [TestCase("D1,C1", "D1", "C1")]
        public void ConvertFromString_ValidSnagList_ValidData(
            string csvInput,
            string expectedDepartment,
            string exptectedComment)
        {
            var converter = new ListConverter<BuildingSnag>();

            var list = (List<BuildingSnag>)converter.ConvertFromString(csvInput, null, null);
            var snag = list.FirstOrDefault();

            Assert.That(snag.Department, Is.EqualTo(expectedDepartment));
            Assert.That(snag.Comment, Is.EqualTo(exptectedComment));
        }

        [Test]
        public void ConvertToString_ReturnValidList_ValidCases()
        {
            var listTwoItems = new List<BuildingSnag>()
            {
                new BuildingSnag() { Department = "D1", Comment = "C1" },
                new BuildingSnag() { Department = "D2", Comment = "C2" }
            };
            var converter = new ListConverter<BuildingSnag>();

            string actualTwoItemString = converter.ConvertToString(listTwoItems, null, null);

            Assert.AreEqual("D1,C1;D2,C2", actualTwoItemString);
        }

        
    }
}
