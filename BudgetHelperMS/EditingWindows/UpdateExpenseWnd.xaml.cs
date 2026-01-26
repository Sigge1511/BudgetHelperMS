using BudgetHelperClassLibrary.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace BudgetHelperMS.EditingWindows
{
    /// <summary>
    /// Interaction logic for UpdateExpenseWnd.xaml
    /// </summary>
    public partial class UpdateExpenseWnd : Window
    {
        public UpdateExpenseWnd(Expense expense, ObservableCollection<Category> categories)
        {
            InitializeComponent();
            DataContext = expense;
            CategoryComboBox.ItemsSource = categories;
        }
        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
