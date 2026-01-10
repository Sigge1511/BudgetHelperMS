using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetHelperClassLibrary.Models
{
    public class Expense
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Amount { get; set; }
        public DateTime ExpenseDate { get; set; }
        public int CategoryId { get; set; }
        public bool IsRecurring { get; set; }
        public int? RecurringExpenseId { get; set; }   

        public virtual ObservableCollection<Category> CategoryList { get; set; }
        public virtual ObservableCollection<RecurringExpense> RecurringExpenseList { get; set; }
        public Expense() { }
    }
}
