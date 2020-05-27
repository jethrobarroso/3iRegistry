using _3iRegistry.WPF.View;
using _3iRegistry.WPF.ViewModel;
using MahApps.Metro.Controls.Dialogs;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace _3iRegistry.WPF.Services
{
    public class PageService : IPageService
    {
        #region Page Accessors
        public UserControl GetDashboardPage()
        {
            return App.ServiceProvider.GetRequiredService<DashboardView>();
        }

        public UserControl GetBeneficiaryDetailsPage()
        {
            var page = App.ServiceProvider.GetRequiredService<BeneficiaryDetailView>();
            page.scrollMain.ScrollToTop();
            return page;
        }

        public PartnerDetailView GetPartnerDetailDialog()
        {
            return App.ServiceProvider.GetRequiredService<PartnerDetailView>();
        }

        public FurnitureDetailView GetFurnitureDetailDialog()
        {
            return App.ServiceProvider.GetRequiredService<FurnitureDetailView>();
        }

        public LearnerDetailView GetLearnerDetailDialog()
        {
            return App.ServiceProvider.GetRequiredService<LearnerDetailView>();
        }
        #endregion
    }
}
