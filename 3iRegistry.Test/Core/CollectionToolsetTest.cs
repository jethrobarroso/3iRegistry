using _3iRegistry.Core;
using _3iRegistry.Core.Tools;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace _3iRegistry.Core.Test
{
    [TestFixture]
    public class CollectionToolsetTest
    {
        [Test]
        public void StringToSnag_ReturnEmptyList_StringEmpty()
        {
            List<BuildingSnag> snags = CollectionToolset.StringToSnags(string.Empty);
            Assert.IsEmpty(snags);
        }

        [TestCase("D 1,C 1", "D 1", "C 1")]
        [TestCase("D1,C1", "D1", "C1")]
        [TestCase("D1,C1,with,commas", "D1", "C1,with,commas")]
        public void StringToSnag_SingleItemList_SingleItemString(
            string csvInputString,
            string expectedDepartment,
            string expectedComment)
        {
            var list = CollectionToolset.StringToSnags(csvInputString);
            var snag = list.FirstOrDefault();

            Assert.That(list.Count, Is.EqualTo(1));
            Assert.That(snag.Department, Is.EqualTo(expectedDepartment));
            Assert.That(snag.Comment, Is.EqualTo(expectedComment));
        }

        [Test]
        public void StringToSnag_ContainsDepartments_ValidInput()
        {
            string snagCsvString = "Department1,Fault1;Department2,Fault2;Department3,Fault3";

            var snagList = CollectionToolset.StringToSnags(snagCsvString);

            Assert.AreEqual("Department1", snagList[0].Department);
            Assert.AreEqual("Department2", snagList[1].Department);
            Assert.AreEqual("Department3", snagList[2].Department);
        }

        [Test]
        public void StringToSnag_ContainsComment_ValidInput()
        {
            string snagCsvString = "Department1,Fault1;Department2,Fault2;Department3,Fault3";

            var snagList = CollectionToolset.StringToSnags(snagCsvString);

            Assert.AreEqual("Fault1", snagList[0].Comment);
            Assert.AreEqual("Fault2", snagList[1].Comment);
            Assert.AreEqual("Fault3", snagList[2].Comment);
        }

        [Test]
        public void StringToSnag_ThrowArgumentException_OneSubItemPerItem()
        {
            string expectedMessage = "There was an issue parsing the input data\nInput data: Item1";

            var ex = Assert.Throws<ArgumentException>(() => CollectionToolset.StringToSnags("Item1;Item3,Item4"));

            Assert.That(ex.Message, Is.EqualTo(expectedMessage));
        }
    }
}