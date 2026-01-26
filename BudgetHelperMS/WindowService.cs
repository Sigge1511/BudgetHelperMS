using BudgetHelperClassLibrary;
using BudgetHelperClassLibrary.Models;
using BudgetHelperMS.EditingWindows;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetHelperMS
{
     // Eller var ditt interface bor

    public class WindowService : IWindowService
    {
        // Update popups
        public bool? ShowUpdateIncomeDialog(Income income, ObservableCollection<IncomeSource> sources)
        {
            // Här har vi tillgång till fönstret eftersom vi är i huvudprojektet!
            var win = new UpdateIncomeWnd(income, sources)
            {
                Owner = System.Windows.Application.Current.MainWindow
            };
            return win.ShowDialog();
        }
        public bool? ShowUpdateExpenseDialog(Expense expense, ObservableCollection<Category> categories)
        {
            var win = new UpdateExpenseWnd(expense, categories)
            {
                Owner = System.Windows.Application.Current.MainWindow
            };
            return win.ShowDialog();
        }

        // Delete popups
        public bool? ShowDeleteIncomeDialog(Income income)
        {
            var win = new DeleteIncomeWnd(income)
            {
                Owner = System.Windows.Application.Current.MainWindow
            };
            return win.ShowDialog();
        }
        public bool? ShowDeleteExpenseDialog(Expense expense)
        {
            var win = new DeleteExpenseWnd(expense)
            {
                Owner = System.Windows.Application.Current.MainWindow
            };
            return win.ShowDialog();
        }
    }
}
