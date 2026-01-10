using BudgetHelperClassLibrary.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetHelperClassLibrary.Data
{
    internal class BudgetHelperDbContext: DbContext
    {
        public BudgetHelperDbContext() { }



        public DbSet<Income> Incomes { get; set; }
        public DbSet<IncomeSource> IncomeSources { get; set; }
        public DbSet<Expense> Expenses { get; set; }
        public DbSet<RecurringExpense> RecurringExpenses { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<BudgetForPurchases> BudgetForPurchases { get; set; }

    }
}
