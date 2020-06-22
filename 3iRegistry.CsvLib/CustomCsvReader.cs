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
using System.Text;
using System.Windows;

namespace CryBitExcelLib
{
    public class CustomCsvReader
    {
        private readonly string _csvPath;

        public CustomCsvReader(string csvPath)
        {
            if (!File.Exists(csvPath))
                throw new FileNotFoundException();

            _csvPath = csvPath;
        }

        public IEnumerable<Beneficiary> ReadBeneficiariesFromCSV()
        {
            List<Beneficiary> list = new List<Beneficiary>();

            try
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
            catch (HeaderValidationException ex)
            {
                throw new CsvImportException(ex.Message);
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

        public static IEnumerable<Beneficiary> ImportFromCSV(string filePath)
        {
            List<Beneficiary> list = new List<Beneficiary>();

            try
            {
                using (var reader = new StreamReader(filePath))
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
            catch(ReaderException ex)
            {
                var innerEx = ex.InnerException as CoreEnumConverterException;
                throw innerEx;
            }
            catch(TypeConverterException ex)
            {
                string tempValue = string.IsNullOrEmpty(ex.Text) ? "an empty value" : $"the value \"{ex.Text}\"";
                string message = $"Could not convert {tempValue}.\n" +
                    $"Please ensure that the data type is correct.\n" +
                    $"\nRow: {ex.ReadingContext.Row}\nColumn: {ex.MemberMapData.Names[0]}";
                throw new CsvImportException(message);
            }
            catch (HeaderValidationException ex)
            {
                throw new CsvImportException($"Header with the name {ex.HeaderNames[0]} was not found.");
            }
        }
    }
}
