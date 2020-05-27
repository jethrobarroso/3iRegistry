using _3iRegistry.Core.Validation;
using CryBitMVVMLib;
using System;
using System.ComponentModel;

namespace _3iRegistry.Core
{
    public class HOP : BindableBase, ICloneable, IDataErrorInfo
    {
        private string _project;
        private string _block;
        private string _unit;
        private string _elec;
        private string _waterE;
        private string _waterM;

        public string Project
        {
            get { return _project; }
            set { SetProperty(ref _project, value); }
        }

        public string Block
        {
            get { return _block; }
            set { SetProperty(ref _block, value); }
        }

        public string Unit
        {
            get { return _unit; }
            set { SetProperty(ref _unit, value); }
        }

        public string Elec
        {
            get { return _elec; }
            set { SetProperty(ref _elec, value); }
        }

        public string WaterE
        {
            get { return _waterE; }
            set { SetProperty(ref _waterE, value); }
        }
        public string WaterM
        {
            get { return _waterM; }
            set { SetProperty(ref _waterM, value); }
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        public override string ToString()
        {
            return $"{Project} {Block} {Unit} {Elec} {WaterE} {WaterM}";
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

                string result = string.Empty;

                switch (propertyName)
                {
                    case "Project":
                        if (string.IsNullOrEmpty(Project))
                            result = "Project field required";
                        break;
                    case "Block":
                        if (string.IsNullOrEmpty(Block))
                            result = "Block field required";
                        break;
                    case "Unit":
                        if (string.IsNullOrEmpty(Unit))
                            result = "Unit field required";
                        break;
                    case "Elec":
                        if (string.IsNullOrEmpty(Elec))
                            result = "Elec field required";
                        break;
                    case "WaterE":
                        if (string.IsNullOrEmpty(WaterE))
                            result = "WaterE field required";
                        break;
                    case "WaterM":
                        if (string.IsNullOrEmpty(WaterM))
                            result = "WaterM field required";
                        break;
                }

                ValidateProperty(propertyName, result);
                
                return result;
            }
        }
    }
}