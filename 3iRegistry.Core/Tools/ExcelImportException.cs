using System;
using System.Collections.Generic;
using System.Text;

namespace _3iRegistry.Core.Tools
{
    public class ExcelImportException : Exception
    {
        public string Cell { get; set; }
        public string CellData { get; set; }
        public string ErrorInfo { get; set; }
    }
}
