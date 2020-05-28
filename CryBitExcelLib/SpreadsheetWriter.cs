using CsvHelper;
using Microsoft.Office.Interop.Excel;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;

namespace CryBitExcelLib
{
    public static class SpreadsheetWriter
    {
        public static void GenerateCSV<T>(IEnumerable<T> data)
        {
            string tempFileName = Path.GetTempPath() + @"\temp.csv";

            using (StreamWriter writer = new StreamWriter(tempFileName, false))
            using (CsvWriter csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.Configuration.RegisterClassMap<BeneficiaryMap>();
                csv.WriteRecords(data);
            }

            SaveFileDialog sfd = new SaveFileDialog()
            {
                Filter = "CSV file (*.csv)|*.csv",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
            };

            if (sfd.ShowDialog() == true)
            {
                File.Move(tempFileName, sfd.FileName, true);
            }
        }

        public static void CreateBlankIfExistsCSV<T>(string filePath)
        {
            var directory = Path.GetDirectoryName(filePath);
            var data = new List<T>();
            if(!File.Exists(filePath))
            {
                if (!Directory.Exists(directory))
                    Directory.CreateDirectory(directory);

                using (StreamWriter writer = new StreamWriter(filePath, false))
                using (CsvWriter csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                {
                    csv.Configuration.RegisterClassMap<BeneficiaryMap>();
                    csv.WriteRecords(data);
                }
            } 
        }
    }
}
