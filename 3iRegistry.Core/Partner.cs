using _3iRegistry.Core.Validation;
using System;
using System.ComponentModel;

namespace _3iRegistry.Core
{
    public class Partner : Adult, ICloneable, IDataErrorInfo
    {
        private MaritalStatus _maritalStatus;
        private DateTime _dob = DateTime.Now;

        public MaritalStatus MaritalStatus
        {
            get { return _maritalStatus; }
            set { SetProperty(ref _maritalStatus, value); }
        }

        public DateTime DOB
        {
            get { return _dob; }
            set { SetProperty(ref _dob, value); }
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        public override string ToString()
        {
            return $"{PersonId},{FirstName},{LastName},{Gender},{DOB.ToShortDateString()},{MaritalStatus}";
        }

        string IDataErrorInfo.Error
        {
            get { return null; }
        }

        string IDataErrorInfo.this[string propertyName]
        {
            get
            {
                string result = null;
                Gender gender = Gender;

                switch (propertyName)
                {
                    case "PersonId":
                        if (string.IsNullOrEmpty(PersonId))
                            result = "ID number cannot be empty";
                        else if (RegexValidation.IsIdNumber(PersonId, ref gender))
                        {
                            Gender = gender;
                        }
                        else result = "Incorrect ID format";
                        break;

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

                    case "Gender":
                        if (RegexValidation.IsIdNumber(PersonId, ref gender))
                        {
                            if (Gender != gender)
                                result = "Gender conflict with ID number";
                        }
                        break;

                    default:
                        result = null;
                        break;
                }

                ValidateProperty(propertyName, result);

                return result;
            }
        }
    }
}