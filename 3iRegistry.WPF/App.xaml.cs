using _3iRegistry.DAL;
using _3iRegistry.WPF.Services;
using _3iRegistry.WPF.View;
using _3iRegistry.WPF.ViewModel;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace _3iRegistry.WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly IHost _host;

        public static IServiceProvider ServiceProvider { get; private set; }

        public App()
        {
            _host = Host.CreateDefaultBuilder().ConfigureServices((context, services) =>
            {
                ConfigureServices(services);
            })
            .ConfigureLogging(logging =>
            {
                logging.AddConsole();
            })
            .Build();

            ServiceProvider = _host.Services;
        }

        

        protected override async void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            await _host.StartAsync();

            //var appView = _host.Services.GetRequiredService<MainWindowView>();
            //appView.DataContext = _host.Services.GetRequiredService<MainWindowViewModel>();
            //appView.Show();

            //_host.Services.GetRequiredService<LoginView>().Show();
             _host.Services.GetRequiredService<IPageService>().ShowLoginView();
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            using (_host)
            {
                await _host.StopAsync(TimeSpan.FromSeconds(5));
            }

            base.OnExit(e);
        }

        private void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IPageService, PageService>();
            services.AddSingleton<IBeneficiaryRepository, BeneficiaryRepository>();
            services.AddSingleton<ITokenFinder, FdTokenFinder>();

            // Register all ViewModels
            services.AddSingleton<LoginViewModel>();
            services.AddSingleton<SnagDetailViewModel>();
            services.AddSingleton<LearnerDetailViewModel>();
            services.AddSingleton<PartnerDetailViewModel>();
            services.AddSingleton<FurnitureDetailViewModel>();
            services.AddSingleton<BeneficiaryDetailViewModel>();
            services.AddSingleton<DashboardViewModel>();
            services.AddSingleton<MainWindowViewModel>();

            // Register all Views
            //services.AddTransient<LoginView>();
            services.AddSingleton<SnagDetailView>();
            services.AddSingleton<LearnerDetailView>();
            services.AddSingleton<PartnerDetailView>();
            services.AddSingleton<FurnitureDetailView>();
            services.AddSingleton<BeneficiaryDetailView>();
            services.AddSingleton<DashboardView>();
            //services.AddTransient<MainWindowView>();

        }
    }
}
