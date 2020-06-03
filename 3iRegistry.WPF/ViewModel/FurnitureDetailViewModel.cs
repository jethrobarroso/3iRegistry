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
    public class FurnitureDetailViewModel : BindableBase
    {
        private readonly IPageService _pageService;
        private Furniture _selectedFurniture;
        private Furniture _copiedFurniture;
        private GlobalContainer _container;
        private bool _deletable;

        public FurnitureDetailViewModel(IPageService pageService)
        {
            _pageService = pageService;
            _container = GlobalContainer.Instance;
            Messenger.Default.Register<Furniture>(this, OnMessageReceive);

            SaveCommand = new RelayCommand(Save, CanSave);
            CancelCommand = new RelayCommand(Done);
            DeleteCommand = new RelayCommand(Delete);
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

        public Furniture CopiedFurniture
        {
            get { return _copiedFurniture; }
            set
            {
                if(value != _copiedFurniture)
                {
                    _copiedFurniture = value;
                    RaisePropertyChanged();   
                }
            }
        }

        private void Done(object obj)
        {
            DialogCoordinator.Instance.HideMetroDialogAsync(ViewModelLocator.BeneficiaryDetailViewModel, _pageService.ShowFurnitureDetailDialog());

            // Release object to avoid breaking GUI validation template
            CopiedFurniture = null;
        }

        private void Delete(object obj)
        {
            var removeItem = _container.SelectedFurniture.SingleOrDefault(r => r.SuperId == _selectedFurniture.SuperId);
            _container.SelectedFurniture.Remove(removeItem);
            Messenger.Default.Send<ModifySubItemMessage>(new ModifySubItemMessage(CopiedFurniture, MemberOperation.Delete), this);
            Done(null);
        }

        private void Save(object obj)
        {
            UpdateProperties();
            Done(null);
        }

        private bool CanSave(object obj)
        {
            if (_copiedFurniture != null &&_copiedFurniture.IsValid)
                return true;
            return false;
        }

        private void UpdateProperties()
        {
            if (_container.IsEditFurniture)
            {
                _selectedFurniture.Name = _copiedFurniture.Name;
                _selectedFurniture.Qty = _copiedFurniture.Qty;
                Messenger.Default.Send<ModifySubItemMessage>(new ModifySubItemMessage(CopiedFurniture, MemberOperation.Update), this);
            }
            else
            {
                Messenger.Default.Send<ModifySubItemMessage>(new ModifySubItemMessage(CopiedFurniture, MemberOperation.Add), this);
            }
        }

        private void OnMessageReceive(Furniture furniture)
        {
            if (_container.IsEditFurniture)
            {
                Deletable = true;
                _selectedFurniture = furniture;
                CopiedFurniture = (Furniture)furniture.Clone();
            }
            else
            {
                Deletable = false;
                CopiedFurniture = new Furniture();
            }
        }
    }
}
