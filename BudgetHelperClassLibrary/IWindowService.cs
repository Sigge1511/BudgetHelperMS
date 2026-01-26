using BudgetHelperClassLibrary.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetHelperClassLibrary
{
    public interface IWindowService
    {
        bool? ShowUpdateIncomeDialog(Income income, ObservableCollection<IncomeSource> sources);
        bool? ShowUpdateExpenseDialog(Expense expense, ObservableCollection<Category> categories);

        bool? ShowDeleteIncomeDialog(Income income);
        bool? ShowDeleteExpenseDialog(Expense expense);
    }
}
