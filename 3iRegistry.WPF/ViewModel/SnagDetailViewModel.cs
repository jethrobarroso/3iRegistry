using _3iRegistry.Core;
using _3iRegistry.DAL;
using _3iRegistry.WPF.Messages;
using _3iRegistry.WPF.Services;
using CryBitMVVMLib;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace _3iRegistry.WPF.ViewModel
{
    public class SnagDetailViewModel : BindableBase, IDataErrorInfo
    {
        private readonly IPageService _pageService;
        private readonly IBeneficiaryRepository _repository;
        private BuildingSnag _snag;
        private string _department;
        private List<string> _departments;
        private string _comments;
        private bool _deletable;
        private ModifySubItemMessage _message;
        private GlobalContainer _container;

        public SnagDetailViewModel(IPageService pageService, IBeneficiaryRepository repository)
        {
            _pageService = pageService;
            _container = GlobalContainer.Instance;
            _repository = repository;
            Messenger.Default.Register<BuildingSnag>(this, OnSnagMessageReceive);

            SaveCommand = new RelayCommand(Save, CanSave);
            CancelCommand = new RelayCommand(Cancel);
            DeleteCommand = new RelayCommand(Delete, CanDelete);

        }

        private void OnSnagMessageReceive(object obj)
        {
            _snag = obj as BuildingSnag;
            _message = new ModifySubItemMessage(_snag);
            Department = _snag.Department;
            Comments = _snag.Comment;
            Departments = _repository.GetDepartments().ToList();
            Deletable = _container.IsEditSnag ? true : false;
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
            _message.Operation = MemberOperation.Delete;
            Messenger.Default.Send(_message, this);
        }

        private bool CanDelete(object obj) => Deletable;

        private void Cancel(object obj)
        {
            _pageService.HideSnagDetailViewDialog(ViewModelLocator.BeneficiaryDetailViewModel);
        }

        private void Save(object obj)
        {
            _snag.Department = Department;
            _snag.Comment = Comments;

            if (_container.IsEditSnag)
                _message.Operation = MemberOperation.Update;
            else _message.Operation = MemberOperation.Add;

            Messenger.Default.Send(_message, this);
        }

        private bool CanSave(object obj) => this.IsValid ? true : false;
        #endregion

        #region Validation
        public string Error => null;

        public string this[string propertyName]
        {
            get
            {
                string result = string.Empty;

                switch (propertyName)
                {
                    case "Department":
                        if (string.IsNullOrEmpty(_department))
                            result = "Department fields required";
                        break;
                    case "Comments":
                        if (string.IsNullOrEmpty(_comments))
                            result = "Must comment on the snag";
                        break;
                }

                ValidateProperty(propertyName, result);

                return result;
            }
        }
        #endregion
    }
}
