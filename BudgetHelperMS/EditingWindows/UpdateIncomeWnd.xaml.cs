using BudgetHelperClassLibrary.Models;
using BudgetHelperClassLibrary.ViewModels;
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
    /// Interaction logic for UpdateIncomeWnd.xaml
    /// </summary>
    public partial class UpdateIncomeWnd : Window
    {       
        public UpdateIncomeWnd(Income income, ObservableCollection<IncomeSource> sources)
        {
            InitializeComponent();
            DataContext = income;
            SourceComboBox.ItemsSource = sources;
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

