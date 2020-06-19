using CryBitMVVMLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace _3iRegistry.Core
{
    public class BuildingSnag : BindableBase, IDataErrorInfo
    {
        private string _department;
        private string _comment;

        public BuildingSnag()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; set; }

        public string Department
        {
            get { return _department; }
            set { SetProperty(ref _department, value); }
        }

        public string Comment
        {
            get { return _comment; }
            set { SetProperty(ref _comment, value); }
        }

        string IDataErrorInfo.Error => throw new NotImplementedException();

        string IDataErrorInfo.this[string propertyName]
        {
            get
            {
                string result = null;

                switch (propertyName)
                {
                    case "Department":
                        if (string.IsNullOrEmpty(_department))
                            result = "Department fields required";
                        break;
                    case "Comment":
                        if (string.IsNullOrEmpty(_comment))
                            result = "Must comment on the snag";
                        break;
                }

                ValidateProperty(propertyName, result);

                return result;
            }
        }
    }
}
