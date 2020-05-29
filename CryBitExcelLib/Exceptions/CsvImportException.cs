using System;
using System.Collections.Generic;
using System.Text;

namespace CryBitExcelLib.Exceptions
{
    public class CsvImportException : Exception
    {
        public CsvImportException(string message)
            : base(message)
        {

        }

        public CsvImportException(string message, string cellData)
            : this(message)
        {
            CellData = cellData;
        }

        public CsvImportException(string message, string cellData, Type originator)
            : this(message, cellData)
        {
            ErrorOriginatorType = originator;
        }

        public CsvImportException() { }

        public string Cell { get; set; }
        public string CellData { get; set; }
        public Type ErrorOriginatorType { get; set; }
    }
}
