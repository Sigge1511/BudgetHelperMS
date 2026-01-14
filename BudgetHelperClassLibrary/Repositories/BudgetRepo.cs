using BudgetHelperClassLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetHelperClassLibrary.Repositories
{
    public class BudgetRepo : IBudgetRepo
    {
        public Task AddExpenseAsync(Expense expense)
        {
            throw new NotImplementedException();
        }

        public Task DeleteExpenseAsync(Expense expense)
        {
            throw new NotImplementedException();
        }

        public Task<List<Expense>> GetAllExpensesAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Expense?> GetExpenseByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<Expense>> GetExpensesByCategoryIdAsync(int categoryId)
        {
            throw new NotImplementedException();
        }

        public Task<List<Expense>> GetExpensesByMonthAsync(int month, int year)
        {
            throw new NotImplementedException();
        }

        public Task UpdateExpenseAsync(Expense expense)
        {
            throw new NotImplementedException();
        }
    }
}
