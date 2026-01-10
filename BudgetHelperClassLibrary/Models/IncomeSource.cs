using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetHelperClassLibrary.Models
{
    public class IncomeSource
    {
        public int Id { get; set; }
        [Required]
        public string SourceName { get; set; }
        
        public virtual ObservableCollection<Income>? Incomes { get; set; }
        public IncomeSource()
        { }
    }
}
