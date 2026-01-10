using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetHelperClassLibrary.Data
{
    public class BudgetHelperContextFactory : IDesignTimeDbContextFactory<BudgetHelperDbContext>
    {
        public BudgetHelperDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<BudgetHelperDbContext>();
            optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=BudgetHelperMS;Integrated Security=True;");

            return new BudgetHelperDbContext(optionsBuilder.Options);
        }
    }
}
