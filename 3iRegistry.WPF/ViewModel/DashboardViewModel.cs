using _3iRegistry.Core;
using _3iRegistry.DAL;
using _3iRegistry.WPF.Extensions;
using _3iRegistry.WPF.Messages;
using CryBitExcelLib;
using CryBitMVVMLib;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace _3iRegistry.WPF.ViewModel
{
    public class DashboardViewModel : BindableBase
    {
        private IBeneficiaryRepository _beneficiaryRepository;
        private ObservableCollection<Beneficiary> _beneficiaries;
        private Beneficiary _selectedBeneficiary;
        private BeneficiaryContainer _container;
        private IDialogCoordinator _dialogCoordinator;
        private MetroDialogSettings dialogSettings;

        public DashboardViewModel(IBeneficiaryRepository beneficiaryRepository)
        {
            _container = BeneficiaryContainer.Instance;
            _beneficiaryRepository = beneficiaryRepository;
            _dialogCoordinator = DialogCoordinator.Instance;

            if(!DesignerProperties.GetIsInDesignMode(new DependencyObject()))
                Task.Factory.StartNew(() => LoadBeneficiaries());
            
            Messenger.Default.Register<DoneSignal>(this, SaveBeneficiary);
            Messenger.Default.Register<DeleteMessage>(this, DeleteBeneficiary, DeleteMessage.SyncContext);
            Messenger.Default.Register<object>(this, ExportBeneficiaries, NavigationToken.ExportToken);
            Messenger.Default.Register<ObservableCollection<Beneficiary>>(this, ImportReceived, MemberOperation.Import);

            dialogSettings = new MetroDialogSettings()
            {
                AffirmativeButtonText = "Yes",
                NegativeButtonText = "No"
            };
        }

        #region Dashboard Binding Properties
        public ObservableCollection<Beneficiary> Beneficiaries
        {
            get { return _beneficiaries; }
            set
            {
                if (_beneficiaries != value)
                {
                    _beneficiaries = value;
                    RaisePropertyChanged();
                }
            }
        }

        public Beneficiary SelectedBeneficiary
        {
            get { return _selectedBeneficiary; }
            set
            {
                if(_selectedBeneficiary != value)
                {
                    _selectedBeneficiary = value;
                    _container.SelectedBeneficiary = value;
                    RaisePropertyChanged();
                }
            }
        }
        #endregion

        #region Message Callbacks
        private async void SaveBeneficiary(DoneSignal obj)
        {
            if (!_container.IsEditMode && !_container.IsCanceled)
                _beneficiaryRepository.AddBeneficiary(_container.SelectedBeneficiary);

            if(!_container.IsCanceled)
            {
                await _beneficiaryRepository.UpdateBeneficiary(_container.SelectedBeneficiary);
                await LoadBeneficiaries();
                CSVBackupSystem.Backup(Beneficiaries);
            }
            
            RaisePropertyChanged("Beneficiaries");
            _container.IsCanceled = false;
        }

        private async void DeleteBeneficiary(DeleteMessage obj)
        {
            var result = await _dialogCoordinator.ShowMessageAsync(this,
                "Delete Beneficiary",
                $"Are you sure you want to delete {SelectedBeneficiary.FirstName} {SelectedBeneficiary.LastName}?",
                MessageDialogStyle.AffirmativeAndNegative,
                dialogSettings);
            
            if(result == MessageDialogResult.Affirmative)
                Beneficiaries.Remove(SelectedBeneficiary);

            CSVBackupSystem.Backup(Beneficiaries);
        }

        private void ExportBeneficiaries(object obj)
        {
            try
            {
                CustomCsvWriter.GenerateCSV(Beneficiaries);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }
        #endregion

        /// <summary>
        /// Populates Beneficiaries property from datasource asynchronously
        /// </summary>
        /// <returns></returns>
        private async Task LoadBeneficiaries()
        {
            try
            {
                var list = await _beneficiaryRepository.GetBeneficiaries();
                Beneficiaries = list.ToObservableCollection();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Read Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Beneficiaries = new ObservableCollection<Beneficiary>();
            }
            
        }

        private void ImportReceived(ObservableCollection<Beneficiary> list)
        {
            Beneficiaries = list;
            CSVBackupSystem.Backup(Beneficiaries);
        }
    }
}
