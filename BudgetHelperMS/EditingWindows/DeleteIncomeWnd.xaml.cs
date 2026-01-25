using BudgetHelperClassLibrary.Models;
using System;
using System.Collections.Generic;
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
    /// Interaction logic for DeleteIncomeWnd.xaml
    /// </summary>
    public partial class DeleteIncomeWnd : Window
    {
        public DeleteIncomeWnd(Income income)
        {            
            InitializeComponent();
            DataContext = income;
        }
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
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
