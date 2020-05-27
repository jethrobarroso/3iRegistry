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
                //if (!EnableValidation)
                //    return null;

                string result = null;
                Gender gender = Gender;

                switch (propertyName)
                {
                    case "PersonId":
                        if (!string.IsNullOrEmpty(PersonId) && !RegexValidation.IsIdNumber(PersonId, ref gender))
                            result = "Invalid ID number";
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