using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetHelperClassLibrary.Models
{
    public class Category
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public virtual ObservableCollection<Expense> ExpensesList { get; set; }
        public Category() { }
        public override string ToString()
        {
            return Name; // Forcing to show cat name in combobox
        }
    }
}
