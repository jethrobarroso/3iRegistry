﻿using _3iRegistry.Core;
using _3iRegistry.Core.Tools;
using CryBitExcelLib;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Design.Serialization;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace _3iRegistry.DAL
{

    public class BeneficiaryRepository : IBeneficiaryRepository
    {
        /// <summary>
        /// Lists that represent database entity data
        /// </summary>
        private List<Beneficiary> _beneficieries;
        private List<string> _schools;
        private List<string> _settlements;
        private List<User> _users;
        private List<string> _departments;

        public BeneficiaryRepository()
        {
            LoadUsers();
            LoadSchools();
            LoadSettlements();
            LoadDepartments();
        }

        /// <summary>
        /// Fetches all beneficiaries from the in-memory data store
        /// </summary>
        /// <returns>A generic list of beneficiaries</returns>
        public async Task<List<Beneficiary>> GetBeneficiaries()
        {
            if (_beneficieries == null)
                await LoadBeneficiariesCSV();

            return _beneficieries;
        }

        public void ImportBeneficiaries(IEnumerable<Beneficiary> newBeneficiaries)
        {
            _beneficieries = newBeneficiaries.ToList();
        }

        public Beneficiary AddBeneficiary(Beneficiary beneficiary)
        {
            _beneficieries.Add(beneficiary);
            return beneficiary;
        }

        /// <summary>
        /// Fetches a beneficiary by id from the data source
        /// </summary>
        /// <param name="id">Associated id number for the beneficiary</param>
        /// <returns>A Beneficiary object with the requested id</returns>
        public Beneficiary GetBeneficiaryById(string id)
        {
            return _beneficieries?.Where(b => b.PersonId == id).FirstOrDefault();
        }

        /// <summary>
        /// Delete a single beneficiary from the data source
        /// </summary>
        /// <param name="beneficiary">An instance of the beneficiary that should be deleted</param>
        /// <returns></returns>
        public Beneficiary DeleteBeneficiary(Beneficiary beneficiary)
        {
            _beneficieries.Remove(beneficiary);
            return beneficiary;
        }

        /// <summary>
        /// Update a single beneficiary in the data source
        /// </summary>
        /// <param name="beneficiary">Instance of the beneficiary object</param>
        /// <returns></returns>
        public async Task<Beneficiary> UpdateBeneficiary(Beneficiary beneficiary)
        {
            var index = await Task.Factory.StartNew(() =>
            _beneficieries.FindIndex(x => x.Id == beneficiary.Id));

            _beneficieries[index] = beneficiary;

            return beneficiary;
        }

        private async Task LoadBeneficiariesCSV()
        {
            string file = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\3i Developments\Data\DataStore.csv";
            _beneficieries = new List<Beneficiary>();

            if (!File.Exists(file))
                CustomCsvWriter.CreateBlankIfExistsCSV<Beneficiary>(file);

            CustomCsvReader reader = new CustomCsvReader(file);

            await Task.Factory.StartNew(() => _beneficieries = reader.ReadBeneficiariesFromCSV().ToList());
        }

        public string AddSchool(string school)
        {
            if (!string.IsNullOrEmpty(school) && !_schools.Contains(school))
                _schools.Add(school);
            return school;
        }

        public string AddSettlement(string settlement)
        {
            if (!string.IsNullOrEmpty(settlement) && !_settlements.Contains(settlement))
                _settlements.Add(settlement);
            return settlement;
        }

        public List<string> GetSchools()
        {
            return _schools;
        }

        public List<string> GetSettlements()
        {
            return _settlements;
        }

        public string AddDepartment(string department)
        {
            if (!string.IsNullOrEmpty(department) && !_departments.Contains(department))
                _departments.Add(department);
            return department;
        }

        public IEnumerable<string> GetDepartments()
        {
            return _departments;
        }

        public IEnumerable<User> GetUsers()
        {
            return _users;
        }

        private void LoadSettlements()
        {
            _settlements = new List<string>()
            {
                "Rest in Peace Informal",
                "Sonderwater Informal",
                "Crossroads Informal",
                "B Section Informal",
                "Shawela Informal",
                "Backyard Dwellers Greater Khutsong Location",
                "Backyard Dwellers Greater Fochville Kokosi",
                "Backyard Dwellers Greater Wedela",
                "New Mandela Informal Settlement",
                "Joe Slovo Informal Settlement",
                "RT Revonia Informal Settlement",
                "Leeuwpan informal settlement",
                "TR Informal settlement",
                "Backyard Dwellers Extension 4",
                "Backyard Dwellers Phase 2",
                "Backyard Dweller Extension 1 and 2",
                "KS informal Settlement",
                "Backyard Dwellers Extension 5",
                "Backyard Dwellers Maxhoseng Section"
            };
        }

        private void LoadSchools()
        {
            _schools = new List<string>()
            {
                "Relebogile Secondary",
                "Wildfontein Primary Farm",
                "Lemao Primary Farm",
                "Dinglo Primary Farm",
                "Hlangabeza Primary",
                "Phororong Primary",
                "Nayaboswa Primary",
                "Kamohelo Primary",
                "Tswasongu Secondary",
                "Badirile Secondary",
                "Mbulelo Primary",
                "Hlanganani Primary",
                "Khutsong South Primary",
                "Itumeleng Primary",
                "Denzel Primary",
            };
        }

        private void LoadUsers()
        {
            _users = new List<User>()
            {
                new User(){ Username = "sysadmin", Password = "Q@zw1234", UserType = UserType.Admin},
                new User(){ Username = "visitor", Password = "3iTempUser1!", UserType = UserType.Visitor}
            };
        }

        private void LoadDepartments()
        {
            _departments = new List<string>()
            {
                "Electrical", 
                "Water", 
                "Painting", 
                "Doors & cupboards", 
                "Walls, floors and ceilings"
            };
        }
    }
}

