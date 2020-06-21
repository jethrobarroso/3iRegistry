using _3iRegistry.Core;
using _3iRegistry.Core.Tools;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace _3iRegistry.Test
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

        [Test]
        public void StringToSnag_SingleItemList_SingleItemString()
        {
            string input = "Department1,Fault1";

            var list = CollectionToolset.StringToSnags(input);
            var snag = list.FirstOrDefault();

            Assert.That(list.Count, Is.EqualTo(1));
            Assert.That(snag.Department, Is.EqualTo("Department1"));
            Assert.That(snag.Comment, Is.EqualTo("Fault1"));
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

        [Test]
        public void StringToSnag_ValidList_SnagCommentContainsCommas()
        {
            string input = "Department1,Comment,with,commas";
            string expectedSnagComment = "Comment,with,commas";

            var snag = CollectionToolset.StringToSnags(input).FirstOrDefault();

            Assert.AreEqual(expectedSnagComment, snag.Comment);
        }
    }
}