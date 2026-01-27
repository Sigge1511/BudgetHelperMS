using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BudgetHelperClassLibrary.Models;


namespace BudgetHelperClassLibrary.CalculationService
{
    public class Calculator
    {
        public Calculator() { }

        public decimal CalculateAverage(IEnumerable<dynamic> data)
        {
            var monthlySums = data
                .GroupBy(d => new { d.ReceivedDate.Year, d.ReceivedDate.Month })
                .OrderByDescending(g => g.Key.Year).ThenByDescending(g => g.Key.Month)
                .Take(3)
                .Select(g => (decimal)g.Sum(x => (decimal)x.Amount))
                .ToList();

            return monthlySums.Any() ? Math.Round(monthlySums.Average(), 2) : 0;
        } 


        public (decimal TotalIncome, 
               decimal TotalExpense, 
               decimal NetAmount) SumOfLastMonth(List<Income> incomes, List<Expense> expenses)
        {
            var lastMonth = DateTime.Now.AddMonths(-1); //Sätter bestämd månad en gång först
            int targetMonth = lastMonth.Month;
            int targetYear = lastMonth.Year;

            decimal sumIncome = incomes
                .Where(i => i.ReceivedDate.Month == targetMonth && i.ReceivedDate.Year == targetYear)
                .Sum(i => i.Amount);

            decimal sumExpense = expenses
                .Where(e => e.ExpenseDate.Month == targetMonth && e.ExpenseDate.Year == targetYear)
                .Sum(e => e.Amount);

            decimal netAmount = sumIncome - sumExpense;

            return (sumIncome, sumExpense, netAmount);
        }

        public (decimal Deduction, decimal Compensation) SickCompCalc(decimal monthlyIncome, 
                                                                      int daysAbsent, bool isVAK)
        {
            decimal yearlyIncome = monthlyIncome * 12;
            decimal basis = (isVAK && yearlyIncome > 410000m) ? 410000m / 12 : monthlyIncome;

            decimal dailyRate = basis / 21m; // Standard arbetsdagar
            decimal deduction = dailyRate * daysAbsent;
            decimal compensation = deduction * 0.8m;

            return (Math.Round(deduction, 2), Math.Round(compensation, 2));
        }


        public decimal GetTotalIncomeSoFar(List<Income> incomes)
        {
            int currentYear = DateTime.Now.Year;
            return incomes
                .Where(i => i.ReceivedDate.Year == currentYear)
                .Sum(i => i.Amount);
        }

        public decimal CalculateYearlyProjectionWithComp(List<Income> incomesSoFar, decimal expectedCompNextMonth)
        {
            int currentYear = DateTime.Now.Year;
            int monthsPassed = DateTime.Now.Month;
            int monthsRemaining = 12 - monthsPassed;

            // 1. Vad har vi fått hittills i år?
            decimal actualSoFar = incomesSoFar
                .Where(i => i.ReceivedDate.Year == currentYear)
                .Sum(i => i.Amount);

            // 2. Vad förväntar vi oss framåt? (Baserat på snittlön)
            decimal avgMonthly = incomesSoFar.Any() ? incomesSoFar.Average(i => i.Amount) : 0;
            decimal futureEstimate = avgMonthly * monthsRemaining;

            // 3. Lägg till kompensationen som kommer "släpande" nästa månad
            return actualSoFar + futureEstimate + expectedCompNextMonth;
        }

        public decimal GetYearlySalaryOnly (List<Income> Salary)
        {
            var salaryIncomes = Salary
                .Where(i => i.IncomeSourceId==1)
                .ToList();

            if (!salaryIncomes.Any()) return 0;
            decimal yearlySalary = salaryIncomes.Average(i => i.Amount);
            return yearlySalary * 12;
        }

        public (decimal ExpectedIn, decimal ExpectedOut, decimal Result) CalculateNextMonthForecast(
                        List<Income> allIncomes,
                        List<Expense> allExpenses,
                        int sickDays,  
                        int vakDays)   
        {
            decimal avgSalary = allIncomes.Where(i => i.IncomeSourceId == 1)
                                          .OrderByDescending(i => i.ReceivedDate)
                                          .Select(i => i.Amount)                                          
                                          .Take(3)
                                          .DefaultIfEmpty(0)
                                          .Average();

            // Calc sick and VAK differently
            var sick = SickCompCalc(avgSalary, sickDays, isVAK: false);
            var vak = SickCompCalc(avgSalary, vakDays, isVAK: true);

            decimal expectedComp = sick.Compensation + vak.Compensation;

            var recurringIds = new[] { 1, 6 }; //I.e salary and study loans

            var fixedIncomes = allIncomes
                .Where(i => recurringIds.Contains(i.IncomeSourceId))
                .GroupBy(i => i.IncomeSourceId)
                .Select(g => g.OrderByDescending(i => i.ReceivedDate).First().Amount)
                .Sum();

            var fixedExpenses = allExpenses
                .Where(e => e.IsRecurring)
                .GroupBy(e => e.Name) // Gruppera så vi inte dubbelräknar gamla hyror
                .Select(g => g.OrderByDescending(e => e.ExpenseDate).First().Amount)
                .Sum();

            decimal totalIn = fixedIncomes + expectedComp;
            decimal netResult = totalIn - fixedExpenses;

            return (Math.Round(totalIn, 2), Math.Round(fixedExpenses, 2), Math.Round(netResult, 2));
        }
        public decimal GetExpectedCompensationNextMonth(decimal avgMonthlyIncome, int sickDays, int vakDays)
        {
            // 1. Räkna ut sjukersättning på vanliga sjukdagar (isVAK: false)
            var sickResult = SickCompCalc(avgMonthlyIncome, sickDays, isVAK: false);

            // 2. Räkna ut ersättning för katt-dagar (isVAK: true -> aktiverar maxtaket i din SickCompCalc)
            var vakResult = SickCompCalc(avgMonthlyIncome, vakDays, isVAK: true);

            // 3. Slå ihop dem - detta är vad som kommer in extra på kontot nästa månad
            return sickResult.Compensation + vakResult.Compensation;
        }

        public IEnumerable<string> GetTopThreeSpendingCategories(List<Expense> expenses)
        {
            if (expenses == null || !expenses.Any()) return new List<string>();

            return expenses
                .Where(e => e.Category != null)
                .GroupBy(e => e.Category.Name)
                .Select(g => new
                {
                    Name = g.Key,
                    Total = g.Sum(e => e.Amount)
                })
                .OrderByDescending(x => x.Total)
                .Take(3)
                // Här gör vi om det till en snygg rad för din punktlista!
                .Select(x => $"{x.Name}: {x.Total:C}")
                .ToList();





            //public decimal CalculateActualSalaryYearly(List<Income> Salary)
            //{
            //    // Vi definierar vilka källor som räknas som "lön"
            //    var salaryKeywords = new[] { "Salary", "Lön", "VAK", "Sjuk", "Care of Cat" };

            //    var salaryIncomes = Salary
            //        .Where(i => salaryKeywords.Any(key =>
            //            i.IncomeSource.SourceName.Contains(key, StringComparison.OrdinalIgnoreCase)))
            //        .ToList();

            //    if (!salaryIncomes.Any()) return 0;

            //    // Här räknar vi ut snittet på bara dessa inkomster
            //    decimal yearlySalary = salaryIncomes.Average(i => i.Amount);
            //    return yearlySalary * 12;
            //}
        }
    }
}
