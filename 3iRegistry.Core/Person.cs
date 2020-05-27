using _3iRegistry;
using _3iRegistry.Core.Validation;
using CryBitMVVMLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace _3iRegistry.Core
{
    public abstract class Person : BindableBase
    {
        #region Fields
        private string _fName;
        private string _lName;
        private string _fullName;
        private Gender _gender;
        #endregion

        public Person()
        {
            Id = Guid.NewGuid();
        }

        #region Entity properties

        public Guid Id { get; set; }

        public string FirstName
        {
            get { return _fName; }
            set 
            {
                SetProperty(ref _fName, value);
                FullName = $"{_fName} {_lName}";
            }
        }

        public string LastName
        {
            get { return _lName; }
            set
            {
                SetProperty(ref _lName, value);
                FullName = $"{_fName} {_lName}";
            }
        }

        public string FullName
        {
            get { return $"{_fName} {_lName}"; }
            private set { SetProperty(ref _fullName, value); }
        }

        public Gender Gender
        {
            get { return _gender; }
            set { SetProperty(ref _gender, value); }
        }
        #endregion

        public abstract override string ToString();
    }
}