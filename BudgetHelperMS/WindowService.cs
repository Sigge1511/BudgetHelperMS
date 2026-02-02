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
    public class WindowService : IWindowService
    {
        // Update popups
        public bool? ShowUpdateIncomeDialog(Income income, ObservableCollection<IncomeSource> sources)
        {
            var dialog = new UpdateIncomeWnd(income, sources);

            dialog.DataContext = new
            {
                SelectedIncome = income,
                IncomeSources = sources
            };

            return dialog.ShowDialog();
        }
        public bool? ShowUpdateExpenseDialog(Expense expense, ObservableCollection<Category> categories)
        {
            var dialog = new UpdateExpenseWnd(expense, categories);

            dialog.DataContext = new
            {
                SelectedExpense = expense,
                Categories = categories
            };

            return dialog.ShowDialog();
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
