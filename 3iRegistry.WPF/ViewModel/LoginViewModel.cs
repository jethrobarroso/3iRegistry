using _3iRegistry.DAL;
using _3iRegistry.WPF.Messages;
using _3iRegistry.WPF.Services;
using CryBitMVVMLib;
using QuickHash.Gen;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Input;

namespace _3iRegistry.WPF.ViewModel
{
    public class LoginViewModel : BindableBase
    {
        private readonly IPageService _pageService;
        private readonly ITokenFinder _tokenSearcher;
        private readonly IBeneficiaryRepository _repo;
        private string _loginErrorMessage;
        private string _username;

        public LoginViewModel(IPageService pageService, ITokenFinder tokenSearcher, IBeneficiaryRepository repo)
        {
            _pageService = pageService;
            _tokenSearcher = tokenSearcher;
            _repo = repo;
            LoginCommand = new RelayCommand(Login, CanLogin);

        }

        public ICommand LoginCommand { get; set; }

        #region Binding properties
        public string Username
        {
            get { return _username; }
            set { SetProperty(ref _username, value); }
        }

        public string LoginErrorMessage
        {
            get { return _loginErrorMessage; }
            set { SetProperty(ref _loginErrorMessage, value); }
        }
        #endregion

        #region Command callbacks
        private void Login(object obj)
        {
            HashGenerator gen = new HashGenerator(_username);
            string password = ((PasswordBox)obj).Password;
            var user = _repo.GetUsers().FirstOrDefault(u => u.Username.Equals(_username) &&
            u.Password.Equals(password));

            var token = File.ReadAllText(_tokenSearcher.GetToken());
            var currentHash = gen.HashIt();

            if (user != null && token == currentHash)
            {
                GlobalContainer.Instance.UserLogingType = user.UserType;
                LoginErrorMessage = string.Empty;
                Username = string.Empty;
                _pageService.ShowMainView();
            }
            else
                LoginErrorMessage = "Invalid username, password or token";

            //_pageService.ShowMainView();
        }

        private bool CanLogin(object obj)
        {
            if (!string.IsNullOrEmpty(_username))
                return true;
            return false;
        }
        #endregion
    }
}
