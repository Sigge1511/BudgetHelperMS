using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetHelperClassLibrary.Models
{
    public class Income
    {
        public int Id { get; set; }
        [Required]
        public int IncomeSourceId { get; set; }
        [Required]
        public decimal Amount { get; set; }
        [Required]
        public DateTime ReceivedDate { get; set; }
        [Required]        
        public virtual ObservableCollection<IncomeSource>? IncomeSourceList { get; set; }
        public virtual ObservableCollection<Category>? CategoryList { get; set; }

        public Income() { }
    }
}
