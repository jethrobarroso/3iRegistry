using CryBitMVVMLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace _3iRegistry.Core
{
    public class BuildingSnag : BindableBase
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

        public override string ToString()
        {
            return $"{_department},{_comment}";
        }
    }
}
