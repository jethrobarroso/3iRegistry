using _3iRegistry.Core;
using _3iRegistry.Core.Tools;
using _3iRegistry.WPF.Extensions;
using _3iRegistry.WPF.Messages;
using _3iRegistry.WPF.Services;
using _3iRegistry.WPF.View;
using CryBitExcelLib;
using CryBitExcelLib.Exceptions;
using CryBitMVVMLib;
using MahApps.Metro.Controls.Dialogs;
using Microsoft.Win32;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace _3iRegistry.WPF.ViewModel
{
    public class MainWindowViewModel : BindableBase
    {
        #region Fields
        private readonly IPageService _pageService;
        private bool _isDash;
        private UserControl _activePage;
        private GlobalContainer _container = GlobalContainer.Instance;
        private PageTransitionMessage _stateChange = new PageTransitionMessage();
        private IDialogCoordinator _coordinator = DialogCoordinator.Instance;
        private MetroDialogSettings _dialogSettings;
        private string _title;
        private string _assemblyVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString(3);
        #endregion

        public MainWindowViewModel(IPageService pageService)
        {
            _pageService = pageService; 
            Messenger.Default.Register<DoneSignal>(this, OnSaveMessageReceived);

            _dialogSettings = new MetroDialogSettings()
            {
                AffirmativeButtonText = "Yes",
                NegativeButtonText = "No"
            };

            Title = $"3i Developments RDP Assistant v{_assemblyVersion}";
            AddParam = "add";
            EditParam = "edit";

            InitializeCommands();
        }

        #region Command properties
        public ICommand ShowDashBoardCommand { get; set; }
        public ICommand AddEditBeneficiaryCommand { get; set; }
        public ICommand DeleteBeneficiaryCommand { get; set; }
        public ICommand ExportBeneficiaryCommand { get; set; }
        public ICommand ImportBeneficiaryCommand { get; set; }
        public ICommand LogoutCommand { get; set; }
        #endregion

        #region Binding properties
        public string AddParam { get; } = "add";
        public string EditParam { get; } = "edit";

        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public bool IsDash
        {
            get { return _isDash; }
            set { SetProperty(ref _isDash, value); }
        }

        public UserControl ActivePage
        {
            get { return _activePage; }
            set
            {
                if (_activePage != value)
                {
                    _activePage = value;
                    RaisePropertyChanged();

                    if (value.GetType() == typeof(DashboardView))
                        IsDash = true;
                }
            }
        }
        #endregion

        #region Command callbacks
        private void ImportBeneficiary(object obj)
        {
            ObservableCollection<Beneficiary> list;
            string filePath = string.Empty;

            OpenFileDialog ofd = new OpenFileDialog()
            {
                Filter = "CSV Files (*.csv)|*.csv",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
            };

            if (ofd.ShowDialog() == true)
                filePath = ofd.FileName;

            if (string.IsNullOrEmpty(filePath))
                return;

            try
            {
                var reader = new CustomCsvReader(filePath);
                list = reader.ReadBeneficiariesFromCSV().ToObservableCollection();
                Messenger.Default.Send(list, MemberOperation.Import);
            }
            catch (CoreEnumConverterException ex)
            {
                _coordinator.ShowMessageAsync(this, $"CSV Read Error ({ex.ErrorOriginatorType.Name})", ex.Message);
            }
            catch (CsvImportException ex)
            {
                _coordinator.ShowMessageAsync(this, $"CSV Read Error", ex.Message);
            }
            catch (ArgumentException ex)
            {
                _coordinator.ShowMessageAsync(this, "Invalid CSV Input", ex.Message);
            }
            catch (Exception ex)
            {
                _coordinator.ShowMessageAsync(this, "Error", ex.Message);
            }
        }

        private void AddEditBeneficiary(object param)
        {
            if (param.ToString() == AddParam)
                _container.IsEditMode = false;
            else
                _container.IsEditMode = true;

            ActivePage = _pageService.ShowBeneficiaryDetailsView();
            Messenger.Default.Send<PageTransitionMessage>(_stateChange);
            IsDash = false;
        }

        private bool CanAddEditBeneficiary(object param)
        {
            if (DesignerProperties.GetIsInDesignMode(new DependencyObject()) || param == null)
                return true;
            if (param.ToString() == AddParam)
                return true;
            if (param.ToString() == EditParam && _container.SelectedBeneficiary != null)
                return true;
            return false;
        }

        private async void ShowDashBoard(object obj)
        {
            if(ActivePage == null)
            {
                ActivePage = _pageService.ShowDashboardView();
                IsDash = true;
            }
            else if (!IsDash)
            {
                var result = await _coordinator.ShowMessageAsync(this,
                    "Discard Changes", "Are you sure you want to discard the changes\n" +
                    "and return to the dashboard?", MessageDialogStyle.AffirmativeAndNegative,
                    _dialogSettings);

                if (result == MessageDialogResult.Affirmative)
                {
                    ActivePage = _pageService.ShowDashboardView();
                    IsDash = true;
                }      
            }
        }

        private void OnSaveMessageReceived(DoneSignal message)
        {
            ActivePage = _pageService.ShowDashboardView();
        }

        private void DeleteBeneficiary(object obj)
        {
            Messenger.Default.Send<DeleteMessage>(new DeleteMessage(), DeleteMessage.SyncContext);
        }

        private void ExportBeneficiary(object obj)
        {
            Messenger.Default.Send(new object(), NavigationToken.ExportToken);
        }

        private bool CanExportBeneficiary(object obj)
        {
            return true;
        }

        private async void Logout(object obj)
        {
            var result = await _coordinator.ShowMessageAsync(this, "Logout", "Are you sure you want to logout?",
                MessageDialogStyle.AffirmativeAndNegative, _dialogSettings);

            if(result == MessageDialogResult.Affirmative)
                _pageService.ShowLoginView();
        }
        #endregion

        #region Initilializers
        public void InitializePage()
        {
            ActivePage = _pageService.ShowDashboardView();
            IsDash = true;
        }

        private void InitializeCommands()
        {
            ShowDashBoardCommand = new RelayCommand(ShowDashBoard);
            AddEditBeneficiaryCommand = new RelayCommand(AddEditBeneficiary, CanAddEditBeneficiary);
            DeleteBeneficiaryCommand = new RelayCommand(DeleteBeneficiary, o => CanAddEditBeneficiary(EditParam));
            ExportBeneficiaryCommand = new RelayCommand(ExportBeneficiary, CanExportBeneficiary);
            ImportBeneficiaryCommand = new RelayCommand(ImportBeneficiary);
            LogoutCommand = new RelayCommand(Logout);
        }
        #endregion
    }
}
