using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace CryBitMVVMLib
{
    public class BindableBase : INotifyPropertyChanged
    {
        private bool _valid = true;
        //private bool _enableValidation = false;

        protected Dictionary<string, string> _errors = new Dictionary<string, string>();

        //public bool EnableValidation
        //{
        //    get { return _enableValidation; }
        //    set { SetProperty(ref _enableValidation, value); }
        //}

        public bool IsValid
        {
            get { return _valid; }
            set { SetProperty(ref _valid, value); }
        }

        protected virtual void SetProperty<T>(ref T member, T val, [CallerMemberName]string propertyName = null)
        {
            if (object.Equals(member, val)) return;
            member = val;
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        protected void RaisePropertyChanged([CallerMemberName]string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        protected void ValidateProperty(string propertyName, string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                if (!_errors.ContainsKey(propertyName))
                    _errors.Add(propertyName, value);
            }
            else if (_errors.ContainsKey(propertyName))
            {
                _errors.Remove(propertyName);
            }

            //if (EnableValidation && _errors.Count == 0)
            //    IsValid = true;
            //else IsValid = false;
            IsValid = (_errors.Count == 0) ? true : false;
        }
    }
}
