using System;
using System.Collections.Generic;
using System.Text;

namespace _3iRegistry.Core.Tools
{
    public class CoreEnumConverterException : Exception
    {
        public CoreEnumConverterException(string message) 
            : base(message)
        {
            
        }
        
        public CoreEnumConverterException(string message, string cellData)
            : this(message)
        {
            EnumValue = cellData;
        }

        public CoreEnumConverterException(string message, string cellData, Type originator)
            : this(message, cellData)
        {
            ErrorOriginatorType = originator;
        }

        public CoreEnumConverterException() { }

        public string EnumValue { get; set; }
        public Type ErrorOriginatorType { get; set; }
    }
}
