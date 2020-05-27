using _3iRegistry.Core.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace _3iRegistry.Core
{
    public abstract class Adult : Person
    {
        private string _personId;

        public string PersonId
        {
            get { return _personId; }
            set { SetProperty(ref _personId, value); }
        }

        public override string ToString()
        {
            return string.Empty;
        }
    }
}
