using _3iRegistry.Core;
using CsvHelper;
using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text;

namespace CryBitExcelLib
{
    public class SpreadsheetReader
    {
        private readonly string _csvPath;

        public SpreadsheetReader(string csvPath)
        {
            if (!File.Exists(csvPath))
                throw new FileNotFoundException();

            _csvPath = csvPath;
        }

        public IEnumerable<Beneficiary> ReadBeneficiariesFromCSV()
        {
            List<Beneficiary> list = new List<Beneficiary>();
            using (var reader = new StreamReader(_csvPath))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csv.Configuration.RegisterClassMap<BeneficiaryMap>();
                var result = csv.GetRecords<Beneficiary>();

                foreach(var b in result)
                {
                    list.Add(b);
                }

                return list;
            }
        }
    }

    
}
