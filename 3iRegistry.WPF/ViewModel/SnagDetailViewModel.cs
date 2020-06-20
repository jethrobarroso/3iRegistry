using _3iRegistry.Core;
using _3iRegistry.DAL;
using _3iRegistry.WPF.Messages;
using _3iRegistry.WPF.Services;
using CryBitMVVMLib;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace _3iRegistry.WPF.ViewModel
{
    public class SnagDetailViewModel : BindableBase
    {
        private readonly IPageService _pageService;
        private readonly IBeneficiaryRepository _repository;
        private BuildingSnag _snag;
        private string _department;
        private List<string> _departments;
        private string _comments;
        private bool _deletable;

        public SnagDetailViewModel(IPageService pageService, IBeneficiaryRepository _repository)
        {
            _pageService = pageService;
            this._repository = _repository;
            Messenger.Default.Register<ModifySubItemMessage>(this, OnSnagMessageReceive, this);

            SaveCommand = new RelayCommand(Save, CanSave);
            CancelCommand = new RelayCommand(Cancel);
            DeleteCommand = new RelayCommand(Delete, CanDelete);

        }

        private void OnSnagMessageReceive(object obj)
        {
            ModifySubItemMessage message = obj as ModifySubItemMessage;
            _snag = message.SubObject as BuildingSnag;

            Department = _snag.Department;
            Comments = _snag.Comment;
            Departments = _repository.GetDepartments().ToList();

            if(message.Operation == MemberOperation.Add)
            {
                Deletable = false;
            }
            else
            {
                Deletable = true;
            }
        }

        #region Command Properties
        public ICommand SaveCommand { get; set; }
        public ICommand CancelCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        #endregion

        #region Binding Properties
        public bool Deletable
        {
            get { return _deletable; }
            set { SetProperty(ref _deletable, value); }
        }

        public string Department
        {
            get { return _department; }
            set { SetProperty(ref _department, value); }
        }

        public List<string> Departments
        {
            get { return _departments; }
            set { SetProperty(ref _departments, value); }
        }

        public string Comments
        {
            get { return _comments; }
            set { SetProperty(ref _comments, value); }
        }
        #endregion

        #region Command callbacks
        private void Delete(object obj)
        {
            var message = new ModifySubItemMessage(_snag, MemberOperation.Delete);
            Messenger.Default.Send(message, this);
        }

        private bool CanDelete(object obj)
        {
            return true;
        }

        private void Cancel(object obj)
        {
            _pageService.HideSnagDetailViewDialog(ViewModelLocator.BeneficiaryDetailViewModel);
        }

        private void Save(object obj)
        {
            throw new NotImplementedException();
        }

        private bool CanSave(object obj)
        {
            return true;
        }
        #endregion

    }
}
