using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetHelperClassLibrary.Models
{
    public class RecurringExpense
    {
        public int Id { get; set; }
        public string Name { get; set; }        
        public int FrequencyInMonths { get; set; } // e.g., 1 for monthly, 12 for yearly
        public int CategoryId { get; set; }

        //public virtual ObservableCollection<Category> CategoryList { get; set; } //????
        public RecurringExpense() { }
    }
}
