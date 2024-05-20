using Estore.Models;
using Estore.Repositories;
using Estore.Views.Admin;
using Microsoft.Extensions.DependencyInjection;
using System.Configuration;
using System.Data;
using System.Windows;

namespace Estore
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private ServiceProvider serviceProvider;
        public App()
        {
            ServiceCollection services = new ServiceCollection();
            ConfigureServices(services);
            serviceProvider = services.BuildServiceProvider();
        }

        private void ConfigureServices(ServiceCollection services)
        {
            services.AddDbContext<MyStoreContext>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IStaffRepository, StaffRepository>();
            services.AddSingleton<StaffsManageView>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddSingleton<MainWindow>();

        }

        private void OnStartUp(object sender, StartupEventArgs e)
        {
            var newWindow = serviceProvider.GetService<StaffsManageView>();
            newWindow.Show();
        }
    }

}
