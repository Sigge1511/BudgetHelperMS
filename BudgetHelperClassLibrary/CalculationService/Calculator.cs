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
                                                                      int daysAbsent, bool isVAK = false)
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


        public decimal CalculateActualSalaryYearly(List<Income> allIncomes)
        {
            // Vi definierar vilka källor som räknas som "lön"
            var salaryKeywords = new[] { "Salary", "Lön", "VAK", "Sjuk", "Care of Cat" };

            var salaryIncomes = allIncomes
                .Where(i => salaryKeywords.Any(key =>
                    i.IncomeSource.SourceName.Contains(key, StringComparison.OrdinalIgnoreCase)))
                .ToList();

            if (!salaryIncomes.Any()) return 0;

            // Här räknar vi ut snittet på bara dessa inkomster
            decimal averageSalary = salaryIncomes.Average(i => i.Amount);
            return averageSalary * 12;
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
        }
    }
}
