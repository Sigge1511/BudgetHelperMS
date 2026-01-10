using BudgetHelperClassLibrary.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Configuration;
using System.Data;
using System.IO;
using System.Windows;

namespace BudgetHelperMS
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        // Skapa denna egenskap högst upp i klassen
        public static IServiceProvider ServiceProvider { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // HÄR inuti ska all din konfigurationskod ligga
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            IConfiguration configuration = builder.Build();

            var serviceCollection = new ServiceCollection();

            var connectionString = configuration.GetConnectionString("DefaultConnection");

            // EF Core registrering
            serviceCollection.AddDbContext<BudgetHelperDbContext>(options =>
                options.UseSqlServer(connectionString));

            //// Repository registrering
            //serviceCollection.AddScoped<IBudgetRepository, BudgetRepository>();

            // ViewModel registrering (viktigt för MVVM)
            serviceCollection.AddTransient<MainWindow>();

            ServiceProvider = serviceCollection.BuildServiceProvider();

            // Starta fönstret via DI
            var mainWindow = ServiceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }
    }
}
