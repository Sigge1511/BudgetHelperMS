using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetHelperClassLibrary.Models
{
    public class Expense
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public decimal Amount { get; set; }
        public DateTime ExpenseDate { get; set; }
        [Required]
        public int CategoryId { get; set; }
        public bool IsRecurring { get; set; }
        public int? FrequencyInMonths { get; set; }
        public virtual Category? Category { get; set; }

        public Expense() { }
    }
}
