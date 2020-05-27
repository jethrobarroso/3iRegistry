using _3iRegistry.Core;
using _3iRegistry.DAL;
using _3iRegistry.WPF.Messages;
using _3iRegistry.WPF.Services;
using CryBitMVVMLib;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace _3iRegistry.WPF.ViewModel
{
    public class LearnerDetailViewModel : BindableBase
    { 
        private readonly IPageService _pageService;
        private readonly IBeneficiaryRepository _beneficiaryRepository;
        private Learner _selectedLearner;
        private Learner _copiedLearner;
        private BeneficiaryContainer _container;
        private ObservableCollection<string> _schools;
        private bool _deletable;

        public LearnerDetailViewModel(IPageService pageService, IBeneficiaryRepository beneficiaryRepository)
        {
            _pageService = pageService;
            _beneficiaryRepository = beneficiaryRepository;
            _container = BeneficiaryContainer.Instance;
            Messenger.Default.Register<Learner>(this, OnMessageReceive);

            SaveCommand = new RelayCommand(Save, CanSave);
            CancelCommand = new RelayCommand(Done);
            DeleteCommand = new RelayCommand(Delete);

            _schools = new ObservableCollection<string>(beneficiaryRepository.GetSchools());
        }

        public ICommand SaveCommand { get; set; }
        public ICommand CancelCommand { get; set; }
        public ICommand DeleteCommand { get; set; }

        public bool Deletable
        {
            get { return _deletable; }
            set
            {
                if (value != _deletable)
                {
                    _deletable = value;
                    RaisePropertyChanged();
                }
            }
        }

        public ObservableCollection<string> Schools
        {
            get { return _schools; }
            set { SetProperty(ref _schools, value); }
        }

        public Learner CopiedLearner
        {
            get { return _copiedLearner; }
            set
            {
                if (value != _copiedLearner)
                {
                    _copiedLearner = value;
                    RaisePropertyChanged();
                }
            }
        }

        private void Delete(object obj)
        {
            var removeItem = _container.SelectedLearners.SingleOrDefault(r => r.Id == _selectedLearner.Id);
            _container.SelectedLearners.Remove(removeItem);
            Messenger.Default.Send<Learner>(CopiedLearner, this);

            Messenger.Default.Send<ModifySubItemMessage>(new ModifySubItemMessage(CopiedLearner, MemberOperation.Delete), this);

            Done(null);
        }

        private void Done(object obj)
        {
            DialogCoordinator.Instance.HideMetroDialogAsync(ViewModelLocator.BeneficiaryDetailViewModel, _pageService.GetLearnerDetailDialog());

            // Release object to avoid breaking validation templates on GUI
            CopiedLearner = null;
        }

        private void Save(object obj)
        {
            UpdateLearner();
            if (!_schools.Contains(_copiedLearner.School))
                _beneficiaryRepository.AddSchool(_copiedLearner.School);
            Done(null);
        }

        private bool CanSave(object obj)
        {
            if (_copiedLearner != null && _copiedLearner.IsValid)
                return true;
            return false;
        }

        private void OnMessageReceive(Learner learner)
        {
            if(_container.IsEditLearner)
            {
                Deletable = true;
                _selectedLearner = learner;
                CopiedLearner = (Learner)learner.Clone();
            }
            else
            {
                Deletable = false;
                CopiedLearner = new Learner();
            }
        }

        private void UpdateLearner()
        {
            if (_container.IsEditLearner)
            {
                _selectedLearner.FirstName = _copiedLearner.FirstName;
                _selectedLearner.LastName = _copiedLearner.LastName;
                _selectedLearner.Gender = _copiedLearner.Gender;
                _selectedLearner.School = _copiedLearner.School;
                _selectedLearner.DOB = _copiedLearner.DOB;
                _selectedLearner.Grade = _copiedLearner.Grade;
            }
            else
            {
                Messenger.Default.Send<ModifySubItemMessage>(new ModifySubItemMessage(CopiedLearner, MemberOperation.Add), this);
            }
        }
    }
}
