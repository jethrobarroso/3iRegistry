using _3iRegistry.Core;
using _3iRegistry.WPF.Messages;
using _3iRegistry.WPF.Services;
using CryBitMVVMLib;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace _3iRegistry.WPF.ViewModel
{
    public class PartnerDetailViewModel : BindableBase
    {
        private readonly IPageService _pageService;
        private Partner _selectedPartner;
        private Partner _copiedPartner;
        private BeneficiaryContainer _container;
        private bool _deletable;

        public PartnerDetailViewModel(IPageService pageService)
        {
            _container = BeneficiaryContainer.Instance;
            Messenger.Default.Register<Partner>(this, OnPartnerReceiveMessage);
            _pageService = pageService;

            CancelCommand = new RelayCommand(Done);
            SaveCommand = new RelayCommand(Save, CanSave);
            DeleteCommand = new RelayCommand(Delete);
        }

        public ICommand CancelCommand { get; set; }
        public ICommand SaveCommand { get; set; }
        public ICommand DeleteCommand { get; set; }

        public bool Deletable
        {
            get { return _deletable; }
            set { SetProperty(ref _deletable, value); }
        }

        public Partner CopiedPartner
        {
            get { return _copiedPartner; }
            set { SetProperty(ref _copiedPartner, value); }
        }

        private void Done(object obj)
        {
            
            DialogCoordinator.Instance.HideMetroDialogAsync(ViewModelLocator.BeneficiaryDetailViewModel, _pageService.GetPartnerDetailDialog());
            CopiedPartner = null;
        }

        private void Delete(object obj)
        {
            var removeItem = _container.SelectedPartners.SingleOrDefault(r => r.Id == _selectedPartner.Id);
            var result = _container.SelectedPartners.Remove(removeItem);
            Messenger.Default.Send<ModifySubItemMessage>(new ModifySubItemMessage(CopiedPartner, MemberOperation.Delete), this);
            Done(null);
        }

        private void Save(object obj)
        {
            UpdateProperties();
            Done(null);
        }

        private bool CanSave(object obj)
        {
            // Check if data is null or not modified
            if (_copiedPartner != null && _copiedPartner.IsValid)
                return true;

            return false;
        }

        private void OnPartnerReceiveMessage(Partner partner)
        {
            if (_container.IsEditPartner)
            {
                Deletable = true;
                _selectedPartner = partner;
                CopiedPartner = (Partner)partner.Clone();
            }
            else
            {
                Deletable = false;
                CopiedPartner = new Partner();
            }

            //CopiedPartner.EnableValidation = true;
        }

        private void UpdateProperties()
        {
            if(_container.IsEditPartner)
            {
                _selectedPartner.FirstName = _copiedPartner.FirstName;
                _selectedPartner.LastName = _copiedPartner.LastName;
                _selectedPartner.PersonId = _copiedPartner.PersonId;
                _selectedPartner.MaritalStatus = _copiedPartner.MaritalStatus;
                _selectedPartner.Gender = _copiedPartner.Gender;
                Messenger.Default.Send<ModifySubItemMessage>(new ModifySubItemMessage(CopiedPartner, MemberOperation.Update), this);
            }
            else
            {
                Messenger.Default.Send<ModifySubItemMessage>(new ModifySubItemMessage(CopiedPartner, MemberOperation.Add), this);
            }
        }
    }
}
