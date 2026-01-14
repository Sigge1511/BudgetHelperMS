using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using BudgetHelperClassLibrary;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using BudgetHelperClassLibrary.ViewModels;

namespace BudgetHelperMS
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly BudgetViewModel viewModel;

        public MainWindow()
        {
            InitializeComponent();
            viewModel = new BudgetViewModel();
            DataContext = viewModel;
            Loaded += MainWindow_Loaded;
        }

        private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            await viewModel.LoadDataAsync();
        }
    }
}