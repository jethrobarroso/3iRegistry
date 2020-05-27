using _3iRegistry.Core;
using CsvHelper;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace _3iRegistry.CSV
{
    public static class CSVWriter
    {
        public static void GenerateCSV(IEnumerable<Beneficiary> data)
        {
            string tempFileName = Path.GetTempPath() + @"\temp.csv";

            using (StreamWriter writer = new StreamWriter(tempFileName, false))
            using (CsvWriter csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteRecords(data);
            }

            SaveFileDialog sfd = new SaveFileDialog()
            {
                Filter = "CSV file (*.csv)|*.csv",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
            };

            if (sfd.ShowDialog() == true)
            {
                if (File.Exists(sfd.FileName))
                {
                    File.Delete(sfd.FileName);
                    File.Move(tempFileName, sfd.FileName);
                }
                else
                    File.Move(tempFileName, sfd.FileName);
            }
        }

        public static bool Save()
        {
            return false;
        }
    }
}
