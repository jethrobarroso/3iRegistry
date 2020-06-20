using _3iRegistry.WPF.View;
using _3iRegistry.WPF.ViewModel;
using MahApps.Metro.Controls.Dialogs;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace _3iRegistry.WPF.Services
{
    public class PageService : IPageService
    {
        Window _mainWindow = null;
        Window _loginWindow = null;

        #region Page Accessors
        public UserControl ShowDashboardView()
        {
            return App.ServiceProvider.GetRequiredService<DashboardView>();
        }

        public UserControl ShowBeneficiaryDetailsView()
        {
            var page = App.ServiceProvider.GetRequiredService<BeneficiaryDetailView>();
            page.scrollMain.ScrollToTop();
            return page;
        }

        public PartnerDetailView ShowPartnerDetailDialog()
        {
            return App.ServiceProvider.GetRequiredService<PartnerDetailView>();
        }

        public FurnitureDetailView ShowFurnitureDetailDialog()
        {
            return App.ServiceProvider.GetRequiredService<FurnitureDetailView>();
        }

        public LearnerDetailView ShowLearnerDetailDialog()
        {
            return App.ServiceProvider.GetRequiredService<LearnerDetailView>();
        }

        public async void ShowSnagDetailViewDialog(object context)
        {
            await DialogCoordinator.Instance.ShowMetroDialogAsync(context, 
                App.ServiceProvider.GetRequiredService<SnagDetailView>());
        }

        public async void HideSnagDetailViewDialog(object context)
        {
            await DialogCoordinator.Instance.HideMetroDialogAsync(context, 
                App.ServiceProvider.GetRequiredService<SnagDetailView>());
        }

        public void ShowLoginView()
        {
            if (_loginWindow == null)
                _loginWindow = new LoginView();

            _loginWindow.Show();

            if (_mainWindow != null)
            {
                _mainWindow.Close();
                _mainWindow = null;
            }
        }

        public void ShowMainView()
        {
            if(_mainWindow == null)
                _mainWindow = new MainWindowView();

            _mainWindow.Show();

            if (_loginWindow != null)
            {
                _loginWindow.Close();
                _loginWindow = null;
            }
                
        }
        #endregion
    }
}
