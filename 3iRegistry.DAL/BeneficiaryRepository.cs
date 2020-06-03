using _3iRegistry.Core;
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

        public BeneficiaryRepository()
        {
            LoadUsers();
            LoadSchools();
            LoadSettlements();
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
            string file = Directory.GetCurrentDirectory() + @"\Data\DataStore.csv";
            _beneficieries = new List<Beneficiary>();

            if (!File.Exists(file))
                CustomCsvWriter.CreateBlankIfExistsCSV<Beneficiary>(file);

            CustomCsvReader reader = new CustomCsvReader(file);

            //try
            //{
            await Task.Factory.StartNew(() => _beneficieries = reader.ReadBeneficiariesFromCSV().ToList());
            //}
            //catch (Exception ex)
            //{

            //    MessageBox.Show(ex.Message);
            //}
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

        private async Task LoadBeneficiariesInMemory()
        {
            await Task.Factory.StartNew(() => _beneficieries = new List<Beneficiary>()
            {
                new Beneficiary()
                {
                    FirstName = "Rudolf",
                    LastName = "Jacobs",
                    PersonId = "9102115553087",
                    Gender = Gender.Male,
                    Contact = "+27765556666",
                    Address = "123 Sesame Street, Wakandaland",
                    Settlement = "TR Informal settlement",
                    Hop = new HOP() { Block = "A1", Project = "Project B", Unit = "8", Elec = "E1234", WaterE = "WE1234", WaterM = "WM1234" },
                    Partners = new List<Partner>()
                    {
                        new Partner()
                        {
                            FirstName = "Sarah", LastName = "Conner",
                            Gender = Gender.Female
                        },
                        new Partner()
                        {
                            FirstName = "Sabrina", LastName = "Lopez",
                            Gender = Gender.Female
                        }
                    },
                    Learners = new List<Learner>()
                    {
                        new Learner() { FirstName = "Sipho", LastName = "Beast" },
                        new Learner() { FirstName = "Ryan", LastName = "Jarule" }
                    },
                    Furniture = new List<Furniture>()
                    {
                        new Furniture() { Name = "Chair", Qty = 3 },
                        new Furniture() { Name = "Bed", Qty = 4 }
                    },
                    Notes = "This is a random note",
                    HouseholdMemberCount = 8
                },
                new Beneficiary()
                {
                    FirstName = "Jonathan",
                    LastName = "Joestark",
                    PersonId = "9003145556087",
                    Gender = Gender.Male,
                    Contact = "+27762223333",
                    Address = "123 Sesame2 Street, Wakandaland2",
                    Settlement = "New Mandela Informal Settlement",
                    Hop = new HOP() { Block = "A1", Project = "Project B", Unit = "8", Elec = "E1234", WaterE = "WE1234", WaterM = "WM1234" },
                    Partners = new List<Partner>()
                    {
                        new Partner()
                        {
                            FirstName = "Silo", LastName = "Mage",
                            Gender = Gender.Female
                        },
                        new Partner()
                        {
                            FirstName = "Kate", LastName = "Leblank",
                            Gender = Gender.Female
                        }
                    },
                    Learners = new List<Learner>()
                    {
                        new Learner() { FirstName = "Jims", LastName = "Wamg" },
                        new Learner() { FirstName = "Shiro", LastName = "Kurusaki" }
                    },
                    Furniture = new List<Furniture>()
                    {
                        new Furniture() { Name = "Chair", Qty = 3 },
                        new Furniture() { Name = "Bed", Qty = 4 }
                    },
                    Notes = "This is a random note",
                    HouseholdMemberCount = 8
                }
            });
        }
    }
}

