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
               
        public virtual IncomeSource? IncomeSource { get; set; }

        public Income() { }
    }
}
