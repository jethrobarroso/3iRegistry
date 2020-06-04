using CsvHelper;
using CsvHelper.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;

namespace CryBitExcelLib
{
    public class CSVBackupSystem
    {
        static int _initialCount = 0;

        public static void Backup<T>(IEnumerable<T> data)
        {
            string activeDir = Directory.GetCurrentDirectory() + @"\Data";
            string backupDir = Directory.GetCurrentDirectory() + @"\Backup";
            string fileSuffix = DateTime.Now.ToString("yyMMdd-HHmmss");
            string backupFile = backupDir + @$"\BackupSheet-{fileSuffix}.csv";
            string activeFile = activeDir + @"\DataStore.csv";

            if (!Directory.Exists(backupDir))
                Directory.CreateDirectory(backupDir);

            try
            {
                using (StreamWriter writer = new StreamWriter(backupFile, false))
                using (CsvWriter csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                {
                    csv.Configuration.RegisterClassMap<BeneficiaryMap>();
                    csv.WriteRecords(data);
                }
            }
            catch
            {
                throw;
            }

            //RotateBackup(backupDir, 10);
            

            if (_initialCount == 0)
            {
                FileInfo[] backupDirFiles = new DirectoryInfo(backupDir).GetFiles();
                string archiveDate = GetOldestFileDateFromName(backupDir);
                bool isSameDate = string.Equals(archiveDate, DateTime.Now.ToString("yyMMdd"));

                if (backupDirFiles.Length != 0 && 
                    !string.IsNullOrEmpty(archiveDate) &&
                    !isSameDate)
                {
                    string archiveFullPath = Directory.GetCurrentDirectory() + @"\Archive";
                    if (!Directory.Exists(archiveFullPath))
                        Directory.CreateDirectory(archiveFullPath);
                    ArchiveBackup(Directory.GetCurrentDirectory() + $@"\Archive\Archive-{archiveDate}.zip");
                    foreach (var f in backupDirFiles)
                        f.Delete();
                } 
            }
                
            _initialCount++;

            if(File.Exists(backupFile))
                RestoreFileFromBackup(backupFile, activeFile);
        }

        public static void RotateBackup(string directory, int fileCount)
        {
            var dirInfo = new DirectoryInfo(directory);
            var fileInfos = dirInfo.GetFileSystemInfos();

            try
            {
                while (fileInfos.Length > fileCount)
                {
                    File.Delete(fileInfos.OrderBy(fi => fi.LastWriteTime).First().FullName);
                    fileInfos = dirInfo.GetFileSystemInfos();
                }
            }
            catch
            {
                throw;
            }
        }

        public static void ArchiveBackup(string archivePath, string backupDir=null)
        {
            if (backupDir == null)
                backupDir = Directory.GetCurrentDirectory() + @"\Backup";
            if (archivePath == null)
                archivePath = Directory.GetCurrentDirectory() + @"\Archive";

            if (!Directory.Exists(backupDir))
            {
                string message = $"The following backup directory does not exist:\n {backupDir}";
                throw new DirectoryNotFoundException(message);
            }   

            if (Directory.GetFiles(backupDir).Length == 0)
            {
                string message = $"Nothing to archive. The following directory is empty:\n" +
                    $"{backupDir}";
                throw new ArgumentException(message, "backupDir");
            }

            if (File.Exists(archivePath))
                File.Delete(archivePath);
            
            ZipFile.CreateFromDirectory(backupDir, archivePath);
        }

        public static void RestoreFileFromBackup(string sourceFile, string targetFile)
        {
            if (!File.Exists(sourceFile))
            {
                var message = "Unable to restore from backup. File not found.";
                throw new FileNotFoundException(message, sourceFile);
            }

            if (File.Exists(targetFile))
                File.Delete(targetFile);

            File.Copy(sourceFile, targetFile);
        }

        public static string GetOldestFileDateFromName(string directory)
        {
            string pattern = @"^BackupSheet-(\d{6})-.*$";
            var regex = new Regex(pattern);

            var dirInfo = new DirectoryInfo(directory);
            var fileInfos = dirInfo.GetFileSystemInfos();
            var fileName = fileInfos.OrderBy(fi => regex.Match(pattern).Groups[1].Value)
                .FirstOrDefault().Name;

            var match = Regex.Match(fileName, pattern, RegexOptions.IgnoreCase);
            return match.Groups[1].Value;
        }
    }
}
