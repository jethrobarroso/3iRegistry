using _3iRegistry.Core;
using _3iRegistry.WPF.Extensions;
using _3iRegistry.WPF.Messages;
using _3iRegistry.WPF.Services;
using _3iRegistry.WPF.View;
using CryBitMVVMLib;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
        private BeneficiaryContainer _container;
        private PageTransitionMessage _stateChange = new PageTransitionMessage();
        private IDialogCoordinator _coordinator;
        private MetroDialogSettings _dialogSettings;
        #endregion

        public MainWindowViewModel(IPageService pageService)
        {
            _pageService = pageService;
            _container = BeneficiaryContainer.Instance;
            Messenger.Default.Register<DoneSignal>(this, OnSaveMessageReceived);

            _coordinator = DialogCoordinator.Instance;
            _dialogSettings = new MetroDialogSettings()
            {
                AffirmativeButtonText = "Yes",
                NegativeButtonText = "No"
            };

            AddParam = "add";
            EditParam = "edit";
            InitializeCommands();
        }

        #region Command properties
        public ICommand ShowDashBoardCommand { get; set; }
        public ICommand AddEditBeneficiaryCommand { get; set; }
        public ICommand DeleteBeneficiaryCommand { get; set; }
        public ICommand ExportBeneficiaryCommand { get; set; }
        #endregion

        #region Binding properties
        public string AddParam { get; } = "add";
        public string EditParam { get; } = "edit";

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
        private void AddEditBeneficiary(object param)
        {
            if (param.ToString() == AddParam)
                _container.IsEditMode = false;
            else
                _container.IsEditMode = true;

            ActivePage = _pageService.GetBeneficiaryDetailsPage();
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
                ActivePage = _pageService.GetDashboardPage();
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
                    ActivePage = _pageService.GetDashboardPage();
                    IsDash = true;
                }      
            }
        }

        private void OnSaveMessageReceived(DoneSignal message)
        {
            ActivePage = _pageService.GetDashboardPage();
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
        #endregion

        #region Initilializers
        public void InitializePage()
        {
            ActivePage = _pageService.GetDashboardPage();
            IsDash = true;
        }

        private void InitializeCommands()
        {
            ShowDashBoardCommand = new RelayCommand(ShowDashBoard);
            AddEditBeneficiaryCommand = new RelayCommand(AddEditBeneficiary, CanAddEditBeneficiary);
            DeleteBeneficiaryCommand = new RelayCommand(DeleteBeneficiary, o => CanAddEditBeneficiary(EditParam));
            ExportBeneficiaryCommand = new RelayCommand(ExportBeneficiary, CanExportBeneficiary);
        }
        #endregion
    }
}
