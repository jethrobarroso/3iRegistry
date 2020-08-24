using _3iRegistry.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace _3iRegistry.Core.Validation
{
    public static class RegexValidation
    {
        private static readonly Regex _regexName = new Regex(@"^[a-zA-Z\s-]{2,}$");
        private static readonly Regex _regexNumber = new Regex(@"^(\+)?(0|27)([5-9])([0-9])( )?([0-9]{3})( )?([0-9]{4})$");
        private static readonly Regex _regexId = new Regex(@"^(?<year>\d{2})(?<month>(?:0[1-9])|(?:1[012]))(?<day>(?:[0-2]\d)|(?:3[01]))(?<gender>(?:[0-4]\d{3})|(?:[5-9]\d{3}))[01]\d\d$");
        private static readonly Regex _regexEmail = new Regex(@"^[A-Z0-9._%+-]+@[A-Z0-9._%+-]+(?:\.\w{2,}){1,2}$", RegexOptions.IgnoreCase);

        public static bool IsName(string name)
        {
            return _regexName.IsMatch(name);
        }

        public static bool IsNumber(string number)
        {
            return _regexNumber.IsMatch(number);
        }

        public static bool IsEmail(string email)
        {
            return _regexEmail.IsMatch(email);
        }

        public static bool IsIdNumber(string id, ref Gender gender)
        {
            if (id == null || !_regexId.IsMatch(id))
                return false;

            Match match = _regexId.Match(id);
            int genderVal = int.Parse(match.Groups["gender"].Value);
            bool result;

            if (genderVal >= 5000 && genderVal <= 9999)
            {
                gender = Gender.Male;
                result = true;
            }
            else
            {
                gender = Gender.Female;
                result = true;
            }

            return result;
        }
    }
}
