using BudgetHelperClassLibrary.Data;
using BudgetHelperClassLibrary.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetHelperClassLibrary.Repositories
{
    public class IncomeRepo: IIncomeRepo
    {
        private readonly BudgetHelperDbContext _dbcntxt;
        public IncomeRepo(BudgetHelperDbContext dbcntxt) {_dbcntxt = dbcntxt;}


        public async Task<List<Income>> GetAllIncomesAsync()
        {
            return await _dbcntxt.Incomes.ToListAsync();
        }

        //Denna behöver bli en lista över alla inkomstkällor senare
        public async Task<IncomeSource?> GetIncomeSourceAsync()
        { return await _dbcntxt.IncomeSources.FirstOrDefaultAsync(); }

        public async Task<Income?> GetIncomeByIdAsync(int id)
        {
            return await _dbcntxt.Incomes.FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<List<Income>> GetIncomesBySourceIdAsync(int sourceId)
        {
            return await _dbcntxt.Incomes
                .Where(i => i.IncomeSourceId == sourceId)
                .ToListAsync();
        }

        public async Task<List<Income>> GetIncomesByMonthAsync(int month, int year)
        {
            return await _dbcntxt.Incomes
                .Where(i => i.ReceivedDate.Month == month && i.ReceivedDate.Year == year)
                .ToListAsync();
        }

        public async Task AddIncomeAsync(Income income)
        {
            await _dbcntxt.Incomes.AddAsync(income);
            await _dbcntxt.SaveChangesAsync();
        }

        public async Task UpdateIncomeAsync(Income income)
        {
            _dbcntxt.Incomes.Update(income);
            await _dbcntxt.SaveChangesAsync();
        }

        public async Task DeleteIncomeAsync(Income income)
        {
            _dbcntxt.Incomes.Remove(income);
            await _dbcntxt.SaveChangesAsync();
        }
    }


}

