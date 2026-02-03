using BudgetHelperClassLibrary.Data;
using BudgetHelperClassLibrary.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
            return await _dbcntxt.Incomes
                        .Include(i => i.IncomeSource)
                        .ToListAsync();
        }

        public async Task<ObservableCollection<IncomeSource>?> GetAllIncomeSourcesAsync()
        {
            List<IncomeSource> sourceList = await _dbcntxt.IncomeSources.ToListAsync();
            return new ObservableCollection<IncomeSource>(sourceList);
        }
        public async Task<IncomeSource?> GetIncomeSourceAsync(int id)
        {
            return await _dbcntxt.IncomeSources.FirstOrDefaultAsync(s => s.Id == id);
        }
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
            income.IncomeSource = null;
            _dbcntxt.Incomes.Update(income); 
            await _dbcntxt.SaveChangesAsync();
        }

        public async Task DeleteIncomeAsync(Income income)
        {
            _dbcntxt.Entry(income).State = EntityState.Deleted; 
            await _dbcntxt.SaveChangesAsync();
        }

        public async Task<AbsenceDays?> GetAbsenceForMonthAsync(int year, int month)
        {
            // Vi letar efter en post som matchar både år och månad
            return await _dbcntxt.AbsenceDays
                .FirstOrDefaultAsync(a => a.Year == year && a.Month == month);
        }
        public async Task SaveOrUpdateAbsenceAsync(int year, int month, int sickDays, int vakDays)
        {
            // 1. Kolla om det redan finns en post för denna månad
            var existing = await _dbcntxt.AbsenceDays
                                 .FirstOrDefaultAsync(a => a.Year == year && a.Month == month);

            if (existing != null)
            {
                // Add to current count
                existing.SickDays += sickDays;
                existing.VakDays += vakDays;
            }
            else
            {
                // Create new post if needed
                _dbcntxt.AbsenceDays.Add(new AbsenceDays
                {
                    Year = year,
                    Month = month,
                    SickDays = sickDays,
                    VakDays = vakDays
                });
            }

            await _dbcntxt.SaveChangesAsync();
        }
    }


}

