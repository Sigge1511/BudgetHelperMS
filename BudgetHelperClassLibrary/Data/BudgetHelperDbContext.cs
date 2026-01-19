using BudgetHelperClassLibrary.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetHelperClassLibrary.Data
{
    public class BudgetHelperDbContext: DbContext
    {
        public BudgetHelperDbContext(
                    DbContextOptions<BudgetHelperDbContext> options) 
                    : base(options){}
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Detta tvingar EF att ignorera CategoryId-kolumnen för Income-modellen
            modelBuilder.Entity<Income>().Ignore("CategoryId");

            base.OnModelCreating(modelBuilder);
        }
        public DbSet<Income> Incomes { get; set; }
        public DbSet<IncomeSource> IncomeSources { get; set; }
        public DbSet<Expense> Expenses { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<BudgetForPurchases> BudgetForPurchases { get; set; }

    }
}
