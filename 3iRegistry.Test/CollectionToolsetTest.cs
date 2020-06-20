using _3iRegistry.Core;
using _3iRegistry.Core.Tools;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using NUnit.Framework;
using System.Collections.Generic;

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
        public void StringToSnag_OneItemList_ValidString()
        {
            string inputString = ""
            //List<BuildingSnag> snags = CollectionToolset
        }
    }
}