using _3iRegistry.Core;
using _3iRegistry.Core.Extensions;
using _3iRegistry.Core.Tools;
using _3iRegistry.DAL;
using _3iRegistry.WPF.Extensions;
using _3iRegistry.WPF.Messages;
using _3iRegistry.WPF.Services;
using CryBitExcelLib;
using CryBitMVVMLib;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.DirectoryServices.ActiveDirectory;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Transactions;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Input;

namespace _3iRegistry.WPF.ViewModel
{
    public class BeneficiaryDetailViewModel : BindableBase
    {
        #region Private fields
        private Beneficiary _selectedBeneficiary;
        private Beneficiary _copiedBeneficiary;
        private Partner _selectedPartner;
        private Learner _selectedLearner;
        private Furniture _selectedFurniture;
        private ObservableCollection<Partner> _partners;
        private ObservableCollection<Learner> _learners;
        private ObservableCollection<Furniture> _furniture;
        private ObservableCollection<string> _settlements;
        private ObservableCollection<BuildingSnag> _snags;
        private int _memberCountExclAdds;
        private bool _hasErrors;
        private bool _editable;

        private double _groupboxMaxWidth = 1000;
        private double _groupboxWidth = 700;
        private IPageService _pageService = new PageService();
        private IDialogCoordinator _dialogCoordinator;
        private CustomDialog _activeDialog;
        private DependencyObject designChecker = new DependencyObject();
        private GlobalContainer _container;
        private DoneSignal _doneSignal = new DoneSignal();
        private MetroDialogSettings dialogSettings;
        private BuildingSnag _selectedSnag;
        private readonly IBeneficiaryRepository _beneficiaryRepository;
        #endregion

        public BeneficiaryDetailViewModel(IBeneficiaryRepository beneficiaryRepository)
        {
            _beneficiaryRepository = beneficiaryRepository;
            _container = GlobalContainer.Instance;
            _dialogCoordinator = DialogCoordinator.Instance;

            UpdatePageLayoutCommand = new RelayCommand(UpdateGroupboxes);
            SaveBeneficiaryCommand = new RelayCommand(SaveBeneficiary, CanSaveBeneficiary);
            CancelBeneficiaryCommand = new RelayCommand(Cancel);
            EditSelectedPartnerCommand = new RelayCommand(EditPartner, CanEditPartner);
            EditSelectedLearnerCommand = new RelayCommand(EditLearner, CanEditLearner);
            EditSelectedFurnitureCommand = new RelayCommand(EditFurniture, CanEditFurniture);
            EditSelectedSnagCommand = new RelayCommand(EditSnag, CanEditSnag);

            dialogSettings = new MetroDialogSettings()
            {
                AffirmativeButtonText = "Yes",
                NegativeButtonText = "No"
            };

            LoadMessengers();
        }

        #region Command properties
        public ICommand UpdatePageLayoutCommand { get; set; }
        public ICommand SaveBeneficiaryCommand { get; set; }
        public ICommand EditSelectedPartnerCommand { get; set; }
        public ICommand EditSelectedLearnerCommand { get; set; }
        public ICommand EditSelectedFurnitureCommand { get; set; }
        public ICommand EditSelectedSnagCommand { get; set; }
        public ICommand CancelBeneficiaryCommand { get; set; }
        #endregion

        #region Bindable Properties

        public string AddParam { get; } = "add";
        public string EditParam { get; } = "edit";

        public bool Editable
        {
            get { return _editable; }
            set { SetProperty(ref _editable, value); }
        }

        public int MemberCountExclAdds 
        {
            get { return _memberCountExclAdds; }
            set { SetProperty(ref _memberCountExclAdds, value); }
        }

        public bool HasErrors
        {
            get { return _hasErrors; }
            set { SetProperty(ref _hasErrors, value); }
        }

        public ObservableCollection<string> Settlements
        {
            get { return _settlements; }
            set { SetProperty(ref _settlements, value); }
        }

        public Beneficiary SelectedBeneficiary
        {
            get { return _selectedBeneficiary; }
            set { SetProperty(ref _selectedBeneficiary, value); }
        }

        public ObservableCollection<BuildingSnag> Snags
        {
            get { return _snags; }
            set { SetProperty(ref _snags, value); }
        }

        public ObservableCollection<Learner> Learners
        {
            get 
            {
                return _learners;
            }
            set 
            {
                SetProperty(ref _learners, value);
                _copiedBeneficiary.MemberCountExclAdds = 1;
                _copiedBeneficiary.MemberCountExclAdds += (_partners != null) ? _partners.Count : 0;
                _copiedBeneficiary.MemberCountExclAdds += _learners.Count;
            }
        }

        public ObservableCollection<Furniture> Furniture
        {
            get { return _furniture; }
            set { SetProperty(ref _furniture, value); }
        }

        public ObservableCollection<Partner> Partners
        {
            get { return _partners; }
            set 
            {
                SetProperty(ref _partners, value);
                _copiedBeneficiary.MemberCountExclAdds = 1;
                _copiedBeneficiary.MemberCountExclAdds += (_learners != null) ? _learners.Count : 0;
                _copiedBeneficiary.MemberCountExclAdds += _partners.Count;
            }
        }

        public Beneficiary CopiedBeneficiary
        {
            get { return _copiedBeneficiary; }
            set { SetProperty(ref _copiedBeneficiary, value); }
        }

        public double GroupboxMaxWidth
        {
            get { return _groupboxMaxWidth; }
            set { SetProperty(ref _groupboxMaxWidth, value); }
        }

        public double GroupboxWidth
        {
            get { return _groupboxWidth; }
            set { SetProperty(ref _groupboxWidth, value); }
        }

        public Partner SelectedPartner
        {
            get { return _selectedPartner; }
            set { SetProperty(ref _selectedPartner, value); }
        }

        public Learner SelectedLearner
        {
            get { return _selectedLearner; }
            set { SetProperty(ref _selectedLearner, value); }
        }

        public Furniture SelectedFurniture
        {
            get { return _selectedFurniture; }
            set { SetProperty(ref _selectedFurniture, value); }
        }

        public BuildingSnag SelectedSnag
        {
            get { return _selectedSnag; }
            set { SetProperty(ref _selectedSnag, value); }
        }
        #endregion

        #region Command callback methods
        private async void Cancel(object obj)
        {
            var result = await _dialogCoordinator.ShowMessageAsync(this,
                "Discard Changes", "Are you sure you want to discard the changes?",
                MessageDialogStyle.AffirmativeAndNegative, dialogSettings);

            if (result == MessageDialogResult.Affirmative)
            {
                _container.IsCanceled = true;
                Messenger.Default.Send<DoneSignal>(_doneSignal);
            }
        }

        private bool CanEditSnag(object mode) => CanEditObject(mode, _selectedSnag);
        private bool CanEditLearner(object mode) => CanEditObject(mode, _selectedLearner);
        private bool CanEditFurniture(object mode) => CanEditObject(mode, _selectedFurniture);
        private bool CanEditPartner(object mode) => CanEditObject(mode, _selectedPartner);

        private void EditSnag(object mode)
        {
            // Load dialog first before sending message. 
            // This will avoid breaking the validation template
            _pageService.ShowSnagDetailViewDialog(this);

            // If user clicks add or edit button
            if (mode.ToString() == AddParam)
            {
                _container.IsEditSnag = false;                                                                       
                Messenger.Default.Send(new BuildingSnag());
            }
            else
            {
                _container.IsEditSnag = true;
                Messenger.Default.Send(SelectedSnag);
            }
        }

        private async void EditLearner(object mode)
        {
            // Load dialog first before sending message. 
            // This will avoid breaking the validation template
            _activeDialog = _pageService.ShowLearnerDetailDialog();
            await _dialogCoordinator.ShowMetroDialogAsync(this, _activeDialog);

            // If user clicks add or edit button
            if (mode.ToString() == AddParam)
            {
                _container.IsEditLearner = false;
                Messenger.Default.Send<Learner>(new Learner());
            }
            else
            {
                _container.IsEditLearner = true;
                Messenger.Default.Send<Learner>(SelectedLearner);
            }
        }

        private async void EditFurniture(object mode)
        {
            // Load dialog first before sending message. 
            // This will avoid breaking the validation template
            _activeDialog = _pageService.ShowFurnitureDetailDialog();
            await _dialogCoordinator.ShowMetroDialogAsync(this, _activeDialog);

            // If user clicks add or edit button
            if (mode.ToString() == AddParam)
            {
                _container.IsEditFurniture = false;
                Messenger.Default.Send<Furniture>(new Furniture());
            }
            else
            {
                _container.IsEditFurniture = true;
                Messenger.Default.Send<Furniture>(SelectedFurniture);
            }
        }

        private async void EditPartner(object mode)
        {
            // Load dialog first before sending message. 
            // This will avoid breaking the validation template
            _activeDialog = _pageService.ShowPartnerDetailDialog();
            await _dialogCoordinator.ShowMetroDialogAsync(this, _activeDialog);

            // If user clicks add or edit button
            if (mode.ToString() == AddParam)
            {
                _container.IsEditPartner = false;
                Messenger.Default.Send<Partner>(new Partner());
            }
            else
            {
                _container.IsEditPartner = true;
                Messenger.Default.Send<Partner>(SelectedPartner);
            }
        }

        private void SaveBeneficiary(object beneficiary)
        {
            CopiedBeneficiary.Partners = new List<Partner>(Partners);
            CopiedBeneficiary.Learners = new List<Learner>(Learners);
            CopiedBeneficiary.Furniture = new List<Furniture>(Furniture);
            CopiedBeneficiary.Snags = new List<BuildingSnag>(Snags);
            _container.SelectedBeneficiary = CopiedBeneficiary;

            if (!Settlements.Contains(CopiedBeneficiary.Settlement))
                _beneficiaryRepository.AddSettlement(CopiedBeneficiary.Settlement);

            Messenger.Default.Send<DoneSignal>(_doneSignal);
        }

        private bool CanSaveBeneficiary(object beneficiary)
        {
            // Check if data is null or not modified
            if (CopiedBeneficiary == null)
                return false;
            if (CopiedBeneficiary.IsValid && CopiedBeneficiary.Hop.IsValid)
                return true;

            return false;
        }

        private void OnStateMessageReceived(PageTransitionMessage obj)
        {
            if (_container.IsEditMode)
            {
                SelectedBeneficiary = _container.SelectedBeneficiary;
                CopiedBeneficiary = (Beneficiary)SelectedBeneficiary.Clone();

                if (_container.UserLogingType == UserType.Admin)
                    Editable = true;
                else Editable = false;
            }
            else
            {
                Editable = true;
                SelectedBeneficiary = Beneficiary.Create;
                CopiedBeneficiary = Beneficiary.Create;
            }

            Partners = new ObservableCollection<Partner>(CopiedBeneficiary.Partners);
            _container.SelectedPartners = Partners;
            Learners = new ObservableCollection<Learner>(CopiedBeneficiary.Learners);
            _container.SelectedLearners = Learners;
            Furniture = new ObservableCollection<Furniture>(CopiedBeneficiary.Furniture);
            _container.SelectedFurniture = Furniture;
            Snags = new ObservableCollection<BuildingSnag>(CopiedBeneficiary.Snags);
            Settlements = new ObservableCollection<string>(_beneficiaryRepository.GetSettlements());
            UpdateHouseholdMinCount();
        }

        /// <summary>
        /// Allows the Beneficiary details view to scale responsively
        /// </summary>
        /// <param name="size"></param>
        private void UpdateGroupboxes(object size)
        {
            double value = (double)size;

            if (value < 1000)
                GroupboxWidth = value - 30;
            else if (value >= 1000)
                GroupboxWidth = (value / 2) - 15;
        }
        #endregion

        private void UpdateHouseholdMinCount()
        {
            _copiedBeneficiary.MemberCountExclAdds = 1;
            _copiedBeneficiary.MemberCountExclAdds += (_learners != null) ? _learners.Count : 0;
            _copiedBeneficiary.MemberCountExclAdds += (_partners != null) ? _partners.Count : 0;
            if (_copiedBeneficiary.HouseholdMemberCount < _copiedBeneficiary.MemberCountExclAdds)
                _copiedBeneficiary.HouseholdMemberCount = _copiedBeneficiary.MemberCountExclAdds;


        }

        #region Initializers
        /// <summary>
        /// Initilialize subscriptions to Messenger in order
        /// to allow communication between pages and dialogs
        /// </summary>
        private void LoadMessengers()
        {
            /// Register to receive Beneficiary from Dashboard
            Messenger.Default.Register<PageTransitionMessage>(this, OnStateMessageReceived);

            Messenger.Default.Register<ModifySubItemMessage>(this, s =>
            {
                if (!_container.IsEditPartner)
                {
                    Partners.Add(s.SubObject as Partner);
                }
                    
                switch (s.Operation)
                {
                    case MemberOperation.Add:
                        _copiedBeneficiary.MemberCountExclAdds++;
                        if (_copiedBeneficiary.HouseholdMemberCount <= _copiedBeneficiary.MemberCountExclAdds)
                            _copiedBeneficiary.HouseholdMemberCount++;
                        break;
                    case MemberOperation.Delete:
                        _copiedBeneficiary.MemberCountExclAdds--;
                        _copiedBeneficiary.HouseholdMemberCount--;
                        break;
                    default:
                        break;
                }

                RaisePropertyChanged("Partners");
            }, ViewModelLocator.PartnerDetailViewModel);

            Messenger.Default.Register<ModifySubItemMessage>(this, l =>
            {
                if (!_container.IsEditLearner)
                    Learners.Add(l.SubObject as Learner);

                switch (l.Operation)
                {
                    case MemberOperation.Add:
                        _copiedBeneficiary.MemberCountExclAdds++;
                        if (_copiedBeneficiary.HouseholdMemberCount <= _copiedBeneficiary.MemberCountExclAdds)
                            _copiedBeneficiary.HouseholdMemberCount++;
                        break;
                    case MemberOperation.Delete:
                        _copiedBeneficiary.MemberCountExclAdds--;
                        _copiedBeneficiary.HouseholdMemberCount--;
                        break;
                    default:
                        break;
                }

                RaisePropertyChanged("Learners");
            }, ViewModelLocator.LearnerDetailViewModel);

            Messenger.Default.Register<ModifySubItemMessage>(this, s =>
            {
                var snagMessage = s.SubObject as BuildingSnag;

                switch (s.Operation)
                {
                    case MemberOperation.Add:
                        Snags.Add(snagMessage);
                        break;
                    case MemberOperation.Delete:
                        Snags.Remove(SelectedSnag);
                        break;
                }

                _pageService.HideSnagDetailViewDialog(this);
                RaisePropertyChanged("Snags");
            }, ViewModelLocator.SnagDetailViewModel);

            Messenger.Default.Register<ModifySubItemMessage>(this, f =>
            {
                if (!_container.IsEditFurniture)
                    Furniture.Add(f.SubObject as Furniture);
                RaisePropertyChanged("Furniture");
            }, ViewModelLocator.FurnitureDetailViewModel);
        }
        #endregion

        /// <summary>
        /// Helper method for CanEdit[Method] command callbacks
        /// </summary>
        /// <typeparam name="T">The object that the command will check against</typeparam>
        /// <param name="mode">The mode received from the controls parameter</param>
        /// <param name="property">The object that the command will check against</param>
        /// <returns></returns>
        private bool CanEditObject<T>(object mode, T property)
        {
            if (DesignerProperties.GetIsInDesignMode(designChecker) || mode == null)
                return true;

            if (mode.ToString() == AddParam)
                return true;
            if (property != null)
                return true;

            return false;
        }
    }
}
