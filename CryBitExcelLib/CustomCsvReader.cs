using _3iRegistry.Core;
using _3iRegistry.Core.Tools;
using CryBitExcelLib.Exceptions;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;

namespace CryBitExcelLib
{
    public class CustomCsvReader
    {
        private readonly string _csvPath;

        public CustomCsvReader(string csvPath)
        {
            _csvPath = csvPath;
        }

        public string[] SupportedHeaders { get; } =
        {
            "ID",
            "SURNAME",
            "NAMES",
            "GENDER",
            "PROJECT",
            "BLOCK",
            "UNIT",
            "ELECTRICITY",
            "WATER (M)",
            "WATER (E)",
            "PRIMARY CONTACT",
            "ALT. CONTACT",
            "EMAIL",
            "TEAM",
            "SETTLEMENT",
            "ADDRESS",
            "FURNITURE",
            "SNAGS",
            "PARTNER(S)",
            "LEARNER(S)",
            "DSTV",
            "HOUSEHOLD MEMBERS COUNT",
            "UNEMPLOYED COUNT",
            "GRANT COUNT",
            "CRHONIC ILLNESS COUNT",
            "ILLNESS DESCRIPTION",
            "GRANT DESCRIPTION",
            "NOTES"
        };

        public IEnumerable<Beneficiary> ReadBeneficiariesFromCSV()
        {
            List<Beneficiary> list = new List<Beneficiary>();

            try
            {
                Read(list);
                ValidateHeaders(SupportedHeaders, GetHeaders());
                return list;
            }
            catch (HeaderValidationException ex)
            {
                string headers = string.Empty;
                string msg = "The following headers are required, but were not found in the CSV:";
                for (int i = 0; i < ex.HeaderNames.Length; i++)
                {
                    headers += (i != ex.HeaderNames.Count() - 1) ? $"{ex.HeaderNames[i]}, " : ex.HeaderNames[i];
                }
                throw new CsvImportException($"{msg}\n{headers}");
            }
            catch (ReaderException ex)
            {
                var innerEx = ex.InnerException as CoreEnumConverterException;
                throw innerEx;
            }
            catch (TypeConverterException ex)
            {
                string tempValue = string.IsNullOrEmpty(ex.Text) ? "an empty value" : $"the value \"{ex.Text}\"";
                string message = $"Could not convert {tempValue}.\n" +
                    $"Please ensure that the data type is correct.\n" +
                    $"\nRow: {ex.ReadingContext.Row}\nColumn: {ex.MemberMapData.Names[0]}";
                throw new CsvImportException(message);
            }
        }

        private IEnumerable<Beneficiary> Read(List<Beneficiary> list)
        {
            using (var reader = new StreamReader(_csvPath))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csv.Configuration.RegisterClassMap<BeneficiaryMap>();
                var result = csv.GetRecords<Beneficiary>();

                foreach (var b in result)
                {
                    list.Add(b);
                }

                return list;
            }
        }

        public void ValidateHeaders(IEnumerable<string> expected, IEnumerable<string> feed)
        {
            if(expected.Count() < feed.Count())
            {
                var msg = "The following headers are not supported:\n";
                var headerDiff = feed.Except(expected).ToList();
                for (int i = 0; i < headerDiff.Count(); i++)
                    msg += (headerDiff.Count() == i - 1) ? $"{headerDiff[i]}, " : headerDiff[i];
                throw new CsvImportException(msg);
            }
        }

        public IEnumerable<string> GetHeaders()
        {
            string[] headerList;
            using (var reader = new StreamReader(_csvPath))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csv.Read();
                csv.ReadHeader();
                headerList = csv.Context.HeaderRecord;
            }

            return headerList;
        }
    }
}
