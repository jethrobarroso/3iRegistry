using _3iRegistry.Core.Extensions;
using _3iRegistry.Core.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;

namespace _3iRegistry.Core
{
    public class Beneficiary : Adult, ICloneable, IDataErrorInfo
    {
        #region Fields
        private string _contact;
        private string _altContact;
        private string _email;
        private string _address;
        private string _settlement;
        private HOP _hop;
        private string _team;
        private List<Partner> _partners;
        private List<Learner> _learners;
        private List<Furniture> _furniture;
        private List<BuildingSnag> _snags;
        private DSTVState _dstv;
        private string _notes;
        private int _householdMemberCount = 1;
        //private int _employedCount;
        private int _unemployedCount;
        private int _grantCount;
        private int _illnessCount;
        private string _illnessDescription;
        private string _grantDescription;
        private int _memberCountExclAdds = 1;
        #endregion

        #region Entity properties
        public int MemberCountExclAdds
        {
            get { return _memberCountExclAdds; }
            set { SetProperty(ref _memberCountExclAdds, value); }
        }

        public string Contact
        {
            get { return _contact; }
            set { SetProperty(ref _contact, value); }
        }

        public string AltContact
        {
            get { return _altContact; }
            set { SetProperty(ref _altContact, value); }
        }

        public string Email
        {
            get { return _email; }
            set { SetProperty(ref _email, value); }
        }

        public string Address
        {
            get { return _address; }
            set { SetProperty(ref _address, value); }
        }

        public string Settlement
        {
            get { return _settlement; }
            set { SetProperty(ref _settlement, value); }
        }

        public HOP Hop
        {
            get { return _hop; }
            set { SetProperty(ref _hop, value); }
        }

        public string Team
        {
            get { return _team; }
            set { SetProperty(ref _team, value); }
        }

        public List<Partner> Partners
        {
            get { return _partners; }
            set { SetProperty(ref _partners, value); }
        }

        public List<Learner> Learners
        {
            get { return _learners; }
            set { SetProperty(ref _learners, value); }
        }

        public List<Furniture> Furniture
        {
            get { return _furniture; }
            set { SetProperty(ref _furniture, value); }
        }

        public List<BuildingSnag> Snags
        {
            get { return _snags; }
            set { SetProperty(ref _snags, value); }
        }

        public DSTVState DSTV
        {
            get { return _dstv; }
            set { SetProperty(ref _dstv, value); }
        }

        public string Notes
        {
            get { return _notes; }
            set { SetProperty(ref _notes, value); }
        }

        public int HouseholdMemberCount
        {
            get { return _householdMemberCount; }
            set { SetProperty(ref _householdMemberCount, value); }
        }

        public int UnemployedCount
        {
            get { return _unemployedCount; }
            set { SetProperty(ref _unemployedCount, value); }
        }

        public int GrantCount
        {
            get { return _grantCount; }
            set { SetProperty(ref _grantCount, value); }
        }

        public int IllnessCount
        {
            get { return _illnessCount; }
            set { SetProperty(ref _illnessCount, value); }
        }

        public string IllnessDescription
        {
            get { return _illnessDescription; }
            set { SetProperty(ref _illnessDescription, value); }
        }

        public string GrantDescription
        {
            get { return _grantDescription; }
            set { SetProperty(ref _grantDescription, value); }
        }
        #endregion

        /// <summary>
        /// Performs a deep copy of the object instance.
        /// </summary>
        public object Clone()
        {
            Beneficiary clone = (Beneficiary)this.MemberwiseClone();
            var cloneSpouses = new List<Partner>();
            var cloneLearners = new List<Learner>();
            var cloneFurniture = new List<Furniture>();
            var cloneHOP = (HOP)clone.Hop.Clone();

            foreach (var spouse in clone.Partners)
            {
                cloneSpouses.Add((Partner)spouse.Clone());
            }

            foreach (var learner in clone.Learners)
            {
                cloneLearners.Add((Learner)learner.Clone());
            }

            foreach (var furn in clone.Furniture)
            {
                cloneFurniture.Add((Furniture)furn.Clone());
            }

            clone.Partners = cloneSpouses;
            clone.Learners = cloneLearners;
            clone.Furniture = cloneFurniture;
            clone.Hop = cloneHOP;

            return clone;
        }

        public override string ToString()
        {
            return $"{FullName} {PersonId} {Gender} {Contact} {Address} {Settlement} " +
                    $"{Partners.GetListString()} {Learners.GetListString()} {Furniture.GetListString()} " +
                    $"{Hop} {Notes} {DSTV} {Team}";
        }

        /// <summary>
        /// Factory method to return a new beneficiary object.
        /// </summary>
        public static Beneficiary Create
        {
            get
            {
                Beneficiary beneficiary = new Beneficiary();
                beneficiary.Partners = new List<Partner>();
                beneficiary.Learners = new List<Learner>();
                beneficiary.Furniture = new List<Furniture>();
                beneficiary.Snags = new List<BuildingSnag>();
                beneficiary.Hop = new HOP();

                return beneficiary;
            }
        }

        #region Validation
        string IDataErrorInfo.Error => null;

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

                    case "Contact":
                        if (string.IsNullOrEmpty(Contact))
                            result = "Contact required";
                        else if (!RegexValidation.IsNumber(Contact))
                            result = "Invalid cell phone number";
                        break;

                    case "AltContact":
                        if (!string.IsNullOrEmpty(AltContact) && !RegexValidation.IsNumber(AltContact))
                            result = "Invalid cell phone number";
                        break;

                    case "Email":
                        if (!string.IsNullOrEmpty(Email) && !RegexValidation.IsEmail(Email))
                            result = "Invalid email number";
                        break;

                    case "Address":
                        if (string.IsNullOrEmpty(Address))
                            result = "Address required";
                        break;

                    case "Settlement":
                        if (string.IsNullOrEmpty(Address))
                            result = "Settlement required";
                        break;
                    case "HouseholdMemberCount":
                        if (HouseholdMemberCount < MemberCountExclAdds)
                            result = "Member count is less than the members added on this form";
                        //else if (UnemployedCount > (_householdMemberCount - _learners.Count))
                        //    result = "Unemployed ratio not adding up";
                        break;

                    case "UnemployedCount":
                        if (UnemployedCount > _householdMemberCount - _learners.Count)
                            result = "Value cannot be more than the number of adults";
                        break;

                    default:
                        result = null;
                        break;
                }

                ValidateProperty(propertyName, result);

                return result;
            }
        }
        #endregion
    }
}
