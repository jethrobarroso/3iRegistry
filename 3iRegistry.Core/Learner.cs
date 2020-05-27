using _3iRegistry.Core.Validation;
using System;
using System.ComponentModel;

namespace _3iRegistry.Core
{
    public class Learner : Person, ICloneable, IDataErrorInfo
    {
        private string _school;
        private DateTime _dob = DateTime.Now;
        private string _grade;

        public DateTime DOB
        {
            get { return _dob; }
            set { SetProperty(ref _dob, value); }
        }

        public string Grade
        {
            get { return _grade; }
            set { SetProperty(ref _grade, value); }
        }

        public string School
        {
            get { return _school; }
            set { SetProperty(ref _school, value); }
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        public override string ToString()
        {
            return $"{FirstName},{LastName},{Gender},{DOB.ToShortDateString()},{School},{Grade}";
        }

        string IDataErrorInfo.Error
        {
            get { return null; }
        }

        string IDataErrorInfo.this[string propertyName]
        {
            get
            {
                //if (!EnableValidation)
                //    return null;

                string result = null;

                switch (propertyName)
                {
                    case "FirstName":
                        if (string.IsNullOrEmpty(FirstName))
                            result = "First name required";
                        else if (!RegexValidation.IsName(FirstName))
                            result = "Incorrect name format";
                        break;

                    case "LastName":
                        if (string.IsNullOrEmpty(LastName))
                            result = "Last name required";
                        else if (!RegexValidation.IsName(LastName))
                            result = "Incorrect name format";
                        break;

                    //case "Grade":
                    //    if (string.IsNullOrEmpty(_grade))
                    //        result = "Grade required";
                    //    break;

                    //case "School":
                    //    if (string.IsNullOrEmpty(_grade))
                    //        result = "School name required";
                    //    break;
                }

                ValidateProperty(propertyName, result);
                
                return result;
            }
        }
    }
}