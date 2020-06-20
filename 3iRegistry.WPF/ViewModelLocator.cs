using _3iRegistry.DAL;
using _3iRegistry.WPF.Services;
using _3iRegistry.WPF.View;
using _3iRegistry.WPF.ViewModel;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace _3iRegistry.WPF
{
    public class ViewModelLocator
    {
        static ViewModelLocator()
        {
            if (DesignerProperties.GetIsInDesignMode(new DependencyObject()))
            {
                var pageService = new PageService();
                var repository = new BeneficiaryRepository();
                var tokenFinder = new FdTokenFinder();

                LoginViewModel = new LoginViewModel(pageService, tokenFinder, repository);
                SnagDetailViewModel = new SnagDetailViewModel(pageService, repository);
                LearnerDetailViewModel = new LearnerDetailViewModel(pageService, repository);
                FurnitureDetailViewModel = new FurnitureDetailViewModel(pageService);
                PartnerDetailViewModel = new PartnerDetailViewModel(pageService);
                BeneficiaryDetailViewModel = new BeneficiaryDetailViewModel(repository);
                DashboardViewModel = new DashboardViewModel(new BeneficiaryRepository());
                MainWindowViewModel = new MainWindowViewModel(pageService);
            }
            else
            {
                LoginViewModel = App.ServiceProvider.GetRequiredService<LoginViewModel>();
                SnagDetailViewModel = App.ServiceProvider.GetRequiredService<SnagDetailViewModel>();
                LearnerDetailViewModel = App.ServiceProvider.GetRequiredService<LearnerDetailViewModel>();
                FurnitureDetailViewModel = App.ServiceProvider.GetRequiredService<FurnitureDetailViewModel>();
                PartnerDetailViewModel = App.ServiceProvider.GetRequiredService<PartnerDetailViewModel>();
                BeneficiaryDetailViewModel = App.ServiceProvider.GetRequiredService<BeneficiaryDetailViewModel>();
                DashboardViewModel = App.ServiceProvider.GetRequiredService<DashboardViewModel>();
                MainWindowViewModel = App.ServiceProvider.GetRequiredService<MainWindowViewModel>();
            }
        }

        public static SnagDetailViewModel SnagDetailViewModel { get; }
        public static LoginViewModel LoginViewModel { get; }
        public static LearnerDetailViewModel LearnerDetailViewModel { get; }
        public static FurnitureDetailViewModel FurnitureDetailViewModel { get; }
        public static PartnerDetailViewModel PartnerDetailViewModel { get; }
        public static BeneficiaryDetailViewModel BeneficiaryDetailViewModel { get; }
        public static DashboardViewModel DashboardViewModel { get; }
        public static MainWindowViewModel MainWindowViewModel { get; }
    }
}
