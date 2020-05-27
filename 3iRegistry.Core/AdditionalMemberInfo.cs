using CryBitMVVMLib;
using System;
using System.Collections.Generic;
using System.Text;

namespace _3iRegistry.Core
{
    public class AdditionalMemberInfo : BindableBase
    {
        private int _additionalMemberCount;
        private int _employedCount;
        private int _unemployedCount;
        private int _grantCount;
        private int _illnessCount;
        private string _illnessDescription;
        private string _grantDescription;

        public int AdditionalMemberCount
        {
            get { return _additionalMemberCount; }
            set
            {
                if(value != _additionalMemberCount)
                {
                    _additionalMemberCount = value;
                    RaisePropertyChanged();
                }
            }
        }

        public int EmployedCount
        {
            get { return _employedCount; }
            set
            {
                if (value != _employedCount)
                {
                    _employedCount = value;
                    RaisePropertyChanged();
                }
            }
        }

        public int UnemployedCount
        {
            get { return _unemployedCount; }
            set
            {
                if (value != _unemployedCount)
                {
                    _unemployedCount = value;
                    RaisePropertyChanged();
                }
            }
        }

        public int GrantCount
        {
            get { return _grantCount; }
            set
            {
                if (value != _grantCount)
                {
                    _grantCount = value;
                    RaisePropertyChanged();
                }
            }
        }

        public int IllnessCount
        {
            get { return _illnessCount; }
            set
            {
                if (value != _illnessCount)
                {
                    _illnessCount = value;
                    RaisePropertyChanged();
                }
            }
        }

        public string IllnessDescription
        {
            get { return _illnessDescription; }
            set
            {
                if (value != _illnessDescription)
                {
                    _illnessDescription = value;
                    RaisePropertyChanged();
                }
            }
        }

        public string GrantDescription
        {
            get { return _grantDescription; }
            set
            {
                if (value != _grantDescription)
                {
                    _grantDescription = value;
                    RaisePropertyChanged();
                }
            }
        }
    }
}
