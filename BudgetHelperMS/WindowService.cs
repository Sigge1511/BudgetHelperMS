using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BudgetHelperClassLibrary;
using BudgetHelperClassLibrary.Models;

namespace BudgetHelperMS
{
     // Eller var ditt interface bor

    public class WindowService : IWindowService
    {
        public bool? ShowUpdateIncomeDialog(object income)
        {
            // Här har vi tillgång till fönstret eftersom vi är i huvudprojektet!
            var win = new UpdateIncomeWnd(income as Income);
            win.Owner = System.Windows.Application.Current.MainWindow;
            return win.ShowDialog();
        }
    }
}
