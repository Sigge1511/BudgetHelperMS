using BudgetHelperClassLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetHelperClassLibrary.Repositories
{
    public interface IBudgetRepo
    {
        Task<List<Expense>> GetAllExpensesAsync();
        Task<Expense?> GetExpenseByIdAsync(int id);
        Task<List<Expense>> GetExpensesByCategoryIdAsync(int categoryId);
        Task<List<Expense>> GetExpensesByMonthAsync(int month, int year);
        Task AddExpenseAsync(Expense expense);
        Task DeleteExpenseAsync(Expense expense);
        Task UpdateExpenseAsync(Expense expense);
    }
}
