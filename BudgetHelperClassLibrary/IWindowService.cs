using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetHelperClassLibrary
{
    public interface IWindowService
    {
        bool? ShowUpdateIncomeDialog(object income);
    }
}
