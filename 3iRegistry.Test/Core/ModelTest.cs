using _3iRegistry.Core;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace _3iRegistry.Test.Core
{
    public class ModelTest
    {
        [TestCase("D1", "C1", ExpectedResult ="D1,C1")]
        [TestCase("", "C1", ExpectedResult = ",C1")]
        [TestCase(null, "C1", ExpectedResult = ",C1")]
        [TestCase(null, "", ExpectedResult = ",")]
        public string BuildingSnag_ToString_Variations(string department, string comment)
        {
            var snag = new BuildingSnag() { Department = department, Comment = comment };

            return snag.ToString();
        }
    }
}
