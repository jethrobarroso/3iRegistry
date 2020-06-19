using _3iRegistry.Core;
using _3iRegistry.WPF.Services;
using CryBitMVVMLib;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace _3iRegistry.WPF.ViewModel
{
    public class SnagDetailViewModel : BindableBase
    {
        private readonly IPageService _pageService;
        private BuildingSnag _snag;
        private string _department;
        private List<string> _departments;
        private string _comments;

        public SnagDetailViewModel(IPageService pageService)
        {
            _pageService = pageService;

            SaveCommand = new RelayCommand(Save, CanSave);
            CancelCommand = new RelayCommand(Cancel);
            DeleteCommand = new RelayCommand(Delete, CanDelete);

        }

        #region Command Properties
        public ICommand SaveCommand { get; set; }
        public ICommand CancelCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        #endregion

        #region Binding Properties
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
            throw new NotImplementedException();
        }

        private bool CanDelete(object obj)
        {
            throw new NotImplementedException();
        }

        private void Cancel(object obj)
        {
            throw new NotImplementedException();
        }

        private void Save(object obj)
        {
            throw new NotImplementedException();
        }

        private bool CanSave(object obj)
        {
            throw new NotImplementedException();
        }
        #endregion

    }
}
