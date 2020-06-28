using CryBitExcelLib;
using CryBitExcelLib.Exceptions;
using CsvHelper;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace _3iRegistry.Test.CsvLib
{
    public class CustomCsvReaderTest
    {
        private CustomCsvReader _reader;

        [OneTimeSetUp]
        public void InitialSetup()
        {
            _reader = new CustomCsvReader("somepath");
        }

        [Test]
        public void Validate_ThrowHeaderValidationException_NonMatchingImportLists()
        {
            var expectedMsg = $"The following headers are not supported:\nh3";
            List<string> supportedList = new List<string>() { "h1", "h2" };
            List<string> userList = new List<string>() { "h1", "h2", "h3" };

            CsvImportException ex = Assert.Throws<CsvImportException>(() =>
             _reader.ValidateHeaders(supportedList, userList));

            Assert.That(ex.Message, Is.EqualTo(expectedMsg));
        }

        [TestCase("h1,h2,h3", "h1,h2")]
        [TestCase("h1,h2", "h1,h2")]
        public void Validate_ReturnVoid_MatchingLists(
            string supportedString,
            string importListString)
        {
            var supportedList = supportedString.Split(',');
            var userList = importListString.Split(',');

            _reader.ValidateHeaders(supportedList, userList);

            Assert.Pass("List are matching and did not throw any excpetions as expected");
        }
    }
}
