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

        //Denna behöver bli en lista över alla inkomstkällor senare
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
            _dbcntxt.Incomes.Update(income);
            await _dbcntxt.SaveChangesAsync();
        }

        public async Task DeleteIncomeAsync(Income income)
        {
            _dbcntxt.Incomes.Remove(income);
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
            // 1. Kolla om det redan finns en post för denna månad [cite: 2025-09-29]
            var existing = await _dbcntxt.AbsenceDays
                .FirstOrDefaultAsync(a => a.Year == year && a.Month == month);

            if (existing != null)
            {
                // 2. Uppdatera befintlig [cite: 2026-01-18]
                existing.SickDays = sickDays;
                existing.VakDays = vakDays;
            }
            else
            {
                // 3. Skapa ny om den inte fanns [cite: 2026-01-18]
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

