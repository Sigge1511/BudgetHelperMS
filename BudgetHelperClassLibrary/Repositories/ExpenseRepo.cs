using BudgetHelperClassLibrary.Models;
using BudgetHelperClassLibrary.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetHelperClassLibrary.Repositories
{
    public class ExpenseRepo : IExpenseRepo
    {
        private readonly BudgetHelperDbContext _dbcntxt;

        public ExpenseRepo(BudgetHelperDbContext dbcntxt) =>_dbcntxt = dbcntxt;


        public async Task<List<Expense>> GetAllExpensesAsync() =>
            await _dbcntxt.Expenses
                .AsNoTracking()
                .Include(e => e.Category) 
                .ToListAsync(); 
        public async Task<Expense?> GetExpenseByIdAsync(int id)=> 
                                    await _dbcntxt.Expenses
                                    .FirstOrDefaultAsync(e => e.Id == id);        
        public async Task<List<Expense>>? GetExpensesByCategoryIdAsync(int categoryId)
        {
            return await _dbcntxt.Expenses
                .Where(e => e.CategoryId == categoryId)
                .ToListAsync();
        }
        public async Task<List<Expense>>? GetExpensesByMonthAsync(int month, int year)
        {
            return await _dbcntxt.Expenses
                .Where(e => e.ExpenseDate.Month == month && e.ExpenseDate.Year == year)
                .ToListAsync();
        }
        public async Task AddExpenseAsync(Expense expense)
        {
            await _dbcntxt.Expenses.AddAsync(expense);
            await _dbcntxt.SaveChangesAsync();
        }
        public async Task DeleteExpenseAsync(Expense expense)
        {
            _dbcntxt.Expenses.Remove(expense);
            await _dbcntxt.SaveChangesAsync();
        }
        public async Task UpdateExpenseAsync(Expense expense)
        {
            _dbcntxt.Expenses.Update(expense);
            await _dbcntxt.SaveChangesAsync();
        }

    }
}
