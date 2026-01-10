using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetHelperClassLibrary.Models
{
    public class BudgetForPurchases
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Amount { get; set; }
        public int? CategoryId { get; set; }
        public DateTime Period { get; set; }
        public DateTimeOffset CreatedAt { get; } = DateTimeOffset.Now;

        public BudgetForPurchases()
        {
            
        }
    }
}
