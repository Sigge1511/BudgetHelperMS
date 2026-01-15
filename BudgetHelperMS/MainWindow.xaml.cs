using BudgetHelperClassLibrary;
using BudgetHelperClassLibrary.Repositories;
using BudgetHelperClassLibrary.ViewModels;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BudgetHelperMS
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly BudgetViewModel budgetViewModel ;

        [DllImport("dwmapi.dll")]
        private static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, ref int attrValue, int attrSize);

        private const int DWMWA_USE_IMMERSIVE_DARK_MODE = 20;
        public MainWindow(BudgetViewModel budgetViewModel)
        {
            InitializeComponent();
            this.budgetViewModel = budgetViewModel;   
            DataContext = this.budgetViewModel;
            this.Loaded += MainWindow_Loaded;

            this.SourceInitialized += (s, e) =>
            {
                IntPtr handle = new WindowInteropHelper(this).Handle;
                int True = 1;
                DwmSetWindowAttribute(handle, DWMWA_USE_IMMERSIVE_DARK_MODE, ref True, sizeof(int));
            };
        }

        private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            await budgetViewModel.LoadDataAsync();
        }
    }
}