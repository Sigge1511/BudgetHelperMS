using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetHelperClassLibrary
{
    public class Income
    {
        public int Id { get; set; }
        public int IncomeSourceId { get; set; }
        public double Amount { get; set; }
        public DateTime ReceivedDate { get; set; }
        public int CategoryId { get; set; }
        
        public virtual ObservableCollection<IncomeSource> IncomeSourceList { get; set; }
        public virtual ObservableCollection<Category> CategoryList { get; set; }

        public Income() { }
    }
}
