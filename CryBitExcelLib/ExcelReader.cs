using _3iRegistry.Core;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Office.Interop.Excel;
using Excel = Microsoft.Office.Interop.Excel;
using System.Reflection;
using System.ComponentModel;
using _3iRegistry.Core.Tools;
using System.IO;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows;

namespace CryBitExcelLib
{
    public class ExcelReader : IDisposable
    {
        string _dataDir = Directory.GetCurrentDirectory() + @"\Data";
        string _path = Directory.GetCurrentDirectory() + @"\Data\DataSheet.xlsx";

        _Application _application = new Excel.Application();
        Workbooks _workBooks;
        Workbook _wb;
        Worksheet _ws;
        Excel.Range _range;

        public ExcelReader(int sheet)
        {
            if (!Directory.Exists(_dataDir))
                Directory.CreateDirectory(_dataDir);

            _workBooks = _application.Workbooks;
            _wb = _workBooks.Open(_path);
            _ws = _wb.Worksheets[sheet] as Worksheet;
            _range = _ws.UsedRange;
        }

        public List<Beneficiary> ReadBeneficiaries()
        {
            List<Beneficiary> list = new List<Beneficiary>();
            Beneficiary beneficiary;
            int countRow = 2;
            int countCol = 1;

            try
            {
                string cellData;


                for (countRow = 2; countRow <= _range.Rows.Count; countRow++)
                {
                    beneficiary = new Beneficiary();

                    for (countCol = 1; countCol <= _range.Columns.Count; countCol++)
                    {
                        cellData = ((Excel.Range)_range.Cells[countRow, countCol]).Value2?.ToString();
                        ConvertCellToPersonProperty(beneficiary, countCol, cellData);
                    }
                    list.Add(beneficiary);
                }
            }
            catch (ExcelImportException ex)
            {
                string message = ex.ErrorInfo + $" @ row{countRow},col{countCol}" +
                    $"\n\nCell Data: {ex.CellData}";
                MessageBox.Show(message, "Import Error");
            }

            return list;
        }

        private void ConvertCellToPersonProperty(Beneficiary beneficiary, int column, string cellData)
        {
            switch (column)
            {
                case 1:
                    beneficiary.PersonId = cellData;
                    break;
                case 2:
                    beneficiary.FirstName = cellData;
                    break;
                case 3:
                    beneficiary.LastName = cellData;
                    break;
                case 4:
                    if (cellData == "Male")
                        beneficiary.Gender = Gender.Male;
                    else
                        beneficiary.Gender = Gender.Female;
                    break;
                case 5:
                    if (beneficiary.Hop == null)
                        beneficiary.Hop = new HOP() { Project = cellData };
                    else
                        beneficiary.Hop.Project = cellData;
                    break;
                case 6:
                    if (beneficiary.Hop == null)
                        beneficiary.Hop = new HOP() { Block = cellData };
                    else
                        beneficiary.Hop.Block = cellData;
                    break;
                case 7:
                    if (beneficiary.Hop == null)
                        beneficiary.Hop = new HOP() { Unit = cellData };
                    else
                        beneficiary.Hop.Unit = cellData;
                    break;
                case 8:
                    if (beneficiary.Hop == null)
                        beneficiary.Hop = new HOP() { Elec = cellData };
                    else
                        beneficiary.Hop.Elec = cellData;
                    break;
                case 9:
                    if (beneficiary.Hop == null)
                        beneficiary.Hop = new HOP() { WaterM = cellData };
                    else
                        beneficiary.Hop.WaterM = cellData;
                    break;
                case 10:
                    if (beneficiary.Hop == null)
                        beneficiary.Hop = new HOP() { WaterE = cellData };
                    else
                        beneficiary.Hop.WaterE = cellData;
                    break;
                case 11:
                    beneficiary.Contact = cellData;
                    break;
                case 12:
                    beneficiary.AltContact = cellData;
                    break;
                case 13:
                    beneficiary.Email = cellData;
                    break;
                case 14:
                    beneficiary.Team = cellData;
                    break;
                case 15:
                    beneficiary.Settlement = cellData;
                    break;
                case 16:
                    beneficiary.Address = cellData;
                    break;
                case 17:
                    beneficiary.Furniture = FurnitureToolSet.StringToFurniture(cellData);
                    break;
                case 18:
                    beneficiary.Partners = SpouseToolSet.StringToSpouse(cellData);
                    break;
                case 19:
                    beneficiary.Learners = LearnerToolSet.StringToLearners(cellData);
                    break;
                case 20:
                    beneficiary.DSTV = EnumToolSet<DSTVState>.ConvertToEnumValue(cellData);
                    break;
                case 21:
                    beneficiary.HouseholdMemberCount = int.Parse(cellData);
                    break;
                case 22:
                    beneficiary.UnemployedCount = int.Parse(cellData);
                    break;
                case 23:
                    beneficiary.GrantCount = int.Parse(cellData);
                    break;
                case 24:
                    beneficiary.IllnessCount = int.Parse(cellData);
                    break;
                case 25:
                    beneficiary.IllnessDescription = cellData;
                    break;
                case 26:
                    beneficiary.GrantDescription = cellData;
                    break;
                case 27:
                    beneficiary.Notes = cellData;
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Closes all components of the excel application and then
        /// references to each excel component
        /// </summary>
        public void Dispose()
        {
            _wb.Close(false, null, null);
            _workBooks.Close();
            _application.Quit();

            //Marshal.ReleaseComObject(_wb);
            //Marshal.ReleaseComObject(_range);
            //Marshal.ReleaseComObject(_workBooks);
            //Marshal.ReleaseComObject(_ws);
            //Marshal.ReleaseComObject(_application);

            _application = null;
            _workBooks = null;
            _wb = null;
            _ws = null;
            _range = null;

            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }
    }
}
