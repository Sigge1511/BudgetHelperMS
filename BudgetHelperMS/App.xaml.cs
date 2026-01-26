using BudgetHelperClassLibrary;
using BudgetHelperClassLibrary.Data;
using BudgetHelperClassLibrary.Repositories;
using BudgetHelperClassLibrary.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Markup;

namespace BudgetHelperMS
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        // Root service provider accessible throughout the app
        public static IServiceProvider? ServiceProvider { get; private set; }

        public App() 
        {
            var culture = new CultureInfo("en-US");
            culture.NumberFormat.NumberGroupSeparator = "";
            culture.NumberFormat.NumberDecimalSeparator = ".";

            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;

            FrameworkElement.LanguageProperty.OverrideMetadata(
                typeof(FrameworkElement),
                new FrameworkPropertyMetadata(
                    XmlLanguage.GetLanguage(culture.IetfLanguageTag)));
        }
        public static IServiceScope? AppScope { get; private set; }

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

            // EF Core & DbContext
            serviceCollection.AddDbContext<BudgetHelperDbContext>(options =>
                options.UseSqlServer(connectionString));

            // Sätt repos
            serviceCollection.AddScoped<IIncomeRepo, IncomeRepo>();
            serviceCollection.AddScoped<IExpenseRepo, ExpenseRepo>();
            serviceCollection.AddScoped<ICategoryRepo, CategoryRepo>();
            serviceCollection.AddScoped<IBudgetRepo, BudgetRepo>();

            // Register views / viewmodels
            serviceCollection.AddTransient<MainWindow>();
            serviceCollection.AddTransient<BudgetViewModel>();
            serviceCollection.AddTransient<IWindowService, WindowService>(); //Generic to open new popups

            // Build root provider
            ServiceProvider = serviceCollection.BuildServiceProvider();

            // Create and hold a scope for the lifetime of the application so scoped services remain valid
            AppScope = ServiceProvider.CreateScope();

            // Resolve main window from the scoped provider and start the app
            var mainWindow = AppScope.ServiceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }
    }
}
