using CryBitMVVMLib;
using System;
using System.ComponentModel;

namespace _3iRegistry.Core
{
    public class Furniture : BindableBase, ICloneable, IDataErrorInfo
    {
        private string _name;
        private int _qty = 1;

        public Furniture()
        {
            SuperId = Guid.NewGuid();
        }

        public Guid SuperId { get; set; }
        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }

        public int Qty
        {
            get { return _qty; }
            set { SetProperty(ref _qty, value); }
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        public override string ToString()
        {
            return $"{Qty}x{Name}";
        }

        string IDataErrorInfo.Error => null;

        string IDataErrorInfo.this[string propertyName]
        {
            get
            {
                string result = string.Empty;

                switch (propertyName)
                {
                    case "Name":
                        if (string.IsNullOrEmpty(Name))
                            result = "Cannot be empty";
                        break;
                    case "Qty":
                        if (Qty == 0)
                            result = "Quantity cannot be 0";
                        break;
                }

                ValidateProperty(propertyName, result);
                
                return result;
            }
        }
    }
}