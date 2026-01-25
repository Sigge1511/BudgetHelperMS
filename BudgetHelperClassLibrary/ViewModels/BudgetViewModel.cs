using BudgetHelperClassLibrary.CalculationService;
using BudgetHelperClassLibrary.Commands;
using BudgetHelperClassLibrary.Models;
using BudgetHelperClassLibrary.Repositories;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace BudgetHelperClassLibrary.ViewModels
{
    public class BudgetViewModel : ObservableObject
    {
        // --- 1. Listor för rullistorna (Hämtas från databasen) ---
        public ObservableCollection<Category> Categories { get; set; } = new();
        public ObservableCollection<IncomeSource> IncomeSources { get; set; } = new();
        public ObservableCollection<Expense> Expenses { get; set; } = new();
        public ObservableCollection<Expense> LatestExpenses { get; set; } = new();
        public ObservableCollection<Income> LatestIncomes { get; set; } = new();
        public ObservableCollection<Income> IncomesThisYear { get; set; } = new();
        public ObservableCollection<string> TopCategories { get; set; } = new();

        

        Calculator calc = new Calculator();

        //*********** PROPS FOR INPUT  ************ 
        private int? _selectedCategory;
        public int? SelectedCategory
        {
            get => _selectedCategory;
            set { _selectedCategory = value; OnPropertyChanged(); AddExpenseComm.RaiseCanExecuteChanged(); }
        }

        private int? _selectedSource;
        public int? SelectedSource
        {
            get => _selectedSource;
            set { _selectedSource = value; OnPropertyChanged(); AddIncomeComm.RaiseCanExecuteChanged(); }
        }

        private decimal _newIncomeAmount;
        public decimal NewIncomeAmount
        {
            get => _newIncomeAmount;
            set
            {
                _newIncomeAmount = value; OnPropertyChanged();
                AddIncomeComm.RaiseCanExecuteChanged();
            }        
        }

        private Expense? _selectedExpense;
        public Expense? SelectedExpense
        {
            get => _selectedExpense;
            set { _selectedExpense = value; OnPropertyChanged(); AddExpenseComm.RaiseCanExecuteChanged(); }
        }

        private Income? _selectedIncome;
        public Income? SelectedIncome
        {
            get => _selectedIncome;
            set { _selectedIncome = value; OnPropertyChanged(); AddExpenseComm.RaiseCanExecuteChanged(); }
        }

        private decimal? newExpenseAmount;
        public decimal? NewExpenseAmount
        {
            get => newExpenseAmount;
            set { newExpenseAmount = value; 
                OnPropertyChanged(); 
                AddExpenseComm.RaiseCanExecuteChanged(); }
        }

        private string? newExpenseName;
        public string? NewExpenseName
        {
            get => newExpenseName;
            set { newExpenseName = value; 
                OnPropertyChanged(); 
                AddExpenseComm.RaiseCanExecuteChanged(); }
        }

        private DateTime _selectedDate = DateTime.Now; // Standardvärde som backup
        public DateTime SelectedDate
        {
            get => _selectedDate;
            set { _selectedDate = value; OnPropertyChanged(); 
                AddExpenseComm.RaiseCanExecuteChanged();
                AddIncomeComm.RaiseCanExecuteChanged();
            }
        }

        private bool _selectedReccuring;
        public bool SelectedIsRecurring
        {
            get => _selectedReccuring;
            set
            {
                _selectedReccuring = value; 
                OnPropertyChanged();
                AddExpenseComm.RaiseCanExecuteChanged();
            }
        }
        
        private int? _selectedFrequencyInMonths;
        public int? SelectedFrequencyInMonths {
            get => _selectedFrequencyInMonths;
            set {  _selectedFrequencyInMonths = value; OnPropertyChanged();}
        }

        private string newCategory;
        public string? NewCategory
        {
            get => newCategory;
            set { newCategory = value; OnPropertyChanged(); }
        }
        //******** LAST MONTH SUMMARY ********
        private decimal? totalIncomesLastMonth;
        public decimal? TotalIncomesLastMonth
        {
            get => totalIncomesLastMonth;
            set { totalIncomesLastMonth = value; OnPropertyChanged(); }
        }

        private decimal? totalExpensesLastMonth;
        public decimal? TotalExpensesLastMonth
        {
            get => totalExpensesLastMonth;
            set { totalExpensesLastMonth = value; OnPropertyChanged(); }
        }

        private decimal? netAmountLastMonth;
        public decimal? NetAmountLastMonth
        {
            get => netAmountLastMonth;
            set { netAmountLastMonth = value; OnPropertyChanged(); }
        }
        //******** FOR MY PROGNOSIS ********
        private decimal avgIncomeLastThreeMonths;
        public decimal AvgIncomeLastThreeMonths
        {
            get => avgIncomeLastThreeMonths;
            set { avgIncomeLastThreeMonths = value; OnPropertyChanged(); }
        }

        private decimal avgExpenseLastThreeMonths;
        public decimal AvgExpenseLastThreeMonths
        {
            get => avgExpenseLastThreeMonths;
            set { avgExpenseLastThreeMonths = value; OnPropertyChanged(); }
        }

        private decimal totalIncomeYear; 
        public decimal TotalIncomeYear
        {
            get => totalIncomeYear;
            set { totalIncomeYear = value; OnPropertyChanged(); }
        }

        private decimal projectedSalary;
        public decimal ProjectedSalary
        {
            get => projectedSalary;
            set { projectedSalary = value; OnPropertyChanged(); }
        }

        private decimal yearlySalary;
        public decimal YearlySalary
        {
            get => yearlySalary;
            set { yearlySalary = value; OnPropertyChanged(); }
        }

        private decimal _nextMonthIn;
        public decimal NextMonthIn
        {
            get => _nextMonthIn;
            set { _nextMonthIn = value; OnPropertyChanged(); }
        }

        private decimal _nextMonthOut;
        public decimal NextMonthOut
        {
            get => _nextMonthOut;
            set { _nextMonthOut = value; OnPropertyChanged(); }
        }

        private decimal _nextMonthNet;
        public decimal NextMonthNet
        {
            get => _nextMonthNet;
            set { _nextMonthNet = value; OnPropertyChanged(); }
        }

        //******** FOR SICK DAYS ETC ********
        private int sickDays;
        public int SickDays
        {
            get => sickDays;
            set { sickDays = value; OnPropertyChanged(); }
        }

        private int careOfCatsDays;
        public int CareOfCatsDays
        {
            get => careOfCatsDays;
            set { careOfCatsDays = value; OnPropertyChanged(); }
        }

        private int sickDaysCount;
        public int SickDaysCount
        {
            get => sickDaysCount;
            set { sickDaysCount = value; OnPropertyChanged(); }
        }


        // Listan som visar alla inkomster i UI:t (Denna behöver uppdateras manuellt)
        public ObservableCollection<Income> AllIncomes { get; set; } = new();

        public RelayCommand AddIncomeComm { get; }
        public RelayCommand UpdateIncomeComm { get; }
        public RelayCommand DeleteIncomeComm { get; }

        public RelayCommand AddExpenseComm { get; }
        public RelayCommand UpdateExpenseComm { get; }
        public RelayCommand DeleteExpenseComm { get; }

        public RelayCommand UpdateMonthSumComm { get; }
        public RelayCommand UpdateSickDaysComm { get; }
        public RelayCommand AddNewCategoryComm { get; }

        private readonly IWindowService _windowService;
        private readonly IIncomeRepo _incomeRepo;
        private readonly IExpenseRepo _expenseRepo;
        private readonly ICategoryRepo _categoryRepo;
        private readonly IBudgetRepo _budgetRepo;

        public BudgetViewModel(IWindowService windowService, IIncomeRepo incomeRepo, 
                               IExpenseRepo expenseRepo, ICategoryRepo categoryRepo, 
                               IBudgetRepo budgetRepo)
        {
            AddIncomeComm = new RelayCommand(async () => await AddIncome(), CanAddIncome);            
            UpdateIncomeComm = new RelayCommand(async () => await UpdateIncome(), CanUpdateIncome);
            DeleteIncomeComm = new RelayCommand(async () => await DeleteIncome(), CanDeleteIncome);

            AddExpenseComm = new RelayCommand(async () => await AddExpense(), CanAddExpense);
            UpdateExpenseComm = new RelayCommand(async () => await UpdateExpense(), CanUpdateExpense);
            DeleteExpenseComm = new RelayCommand(async () => await DeleteExpense(), CanDeleteExpense);

            UpdateMonthSumComm = new RelayCommand(async () => await UpdateMonthSum());
            UpdateSickDaysComm = new RelayCommand(async () => await UpdateSickDays(), CanUpdateSickdays);

            AddNewCategoryComm = new RelayCommand(async () => await AddNewCategory(), CanAddNewCategory);

            _windowService = windowService;
            _incomeRepo = incomeRepo;
            _expenseRepo = expenseRepo;
            _categoryRepo = categoryRepo;
            _budgetRepo = budgetRepo;
        }

        

        public async Task LoadDataAsync()
        {
            // Här hämtar vi kategorier från databasen via ditt repo
            var categoriesList = await _categoryRepo.GetAllCategoriesAsync();
            Categories.Clear();
            foreach (var c in categoriesList) Categories.Add(c);
            //*******************************************************************
            var sources = await _incomeRepo.GetAllIncomeSourcesAsync();
            IncomeSources.Clear();
            if (sources != null)
            {
                foreach (var s in sources)
                {
                    IncomeSources.Add(s);

                }
            }
            //*******************************************************************
            var expenseList = await _expenseRepo.GetAllExpensesAsync();
            LatestExpenses.Clear();
            foreach (var e in expenseList) LatestExpenses.Add(e);
            var latestExpensesList = expenseList.OrderByDescending(x => x.Id).Take(5);
            //*******************************************************************          
            var incomesList = await _incomeRepo.GetAllIncomesAsync();
            LatestIncomes.Clear();
            foreach (var i in incomesList) LatestIncomes.Add(i);
            var latestIncomesList = incomesList.OrderByDescending(x => x.Id).Take(5);

            //*******************************************************************          

            AvgIncomeLastThreeMonths = calc.CalculateAverage(incomesList);
            await UpdateMonthSum();
            await UpdateSickDays();  
            await UpdateAverages();
            await UpdateYearStats();
            await GetIncomesThisYearAsync();
        }

        

        //***********   METODER   ********************************************************
        private bool CanAddIncome()
        {
            // Kolla Belopp, Datum och Källa
            return NewIncomeAmount > 0 && SelectedSource.HasValue && SelectedDate != default;
            //NewIncomeAmount > 0 && SelectedCategory != null && SelectedSource != null;
        }
        private async Task AddIncome()
        {
            try
            {
                decimal amount = NewIncomeAmount;
                var newIncome = new Income
                {
                    Amount = amount, // Här sparas det (ev. omräknade) beloppet
                    ReceivedDate = SelectedDate,
                    IncomeSourceId = (int)SelectedSource
                };
                await _incomeRepo.AddIncomeAsync(newIncome);

                NewIncomeAmount = 0;
                SelectedSource = null;
                SelectedDate = DateTime.Now; // Återställ allt igen
                
                await UpdateMonthSum(); // Uppdatera direkt i vyn
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding income: {ex.Message}");
            }
        }

        private bool CanUpdateIncome()
        {
            return SelectedIncome != null;
        }
        private async Task UpdateIncome()
        {
            if (SelectedIncome == null) return;

            // Calling on my new service to show popup
            if (_windowService.ShowUpdateIncomeDialog(SelectedIncome) == true)
            {
                await _incomeRepo.UpdateIncomeAsync(SelectedIncome);
                await LoadDataAsync(); 
            }
        }


        private bool CanDeleteIncome()
        {
            return true;
        }
        private async Task DeleteIncome()
        {
            throw new NotImplementedException();
        }
        //*******************************************************************
        
        private bool CanAddExpense()
        {
            return  SelectedCategory != null && NewExpenseAmount.HasValue
                    && !string.IsNullOrWhiteSpace(NewExpenseName);
        }
        private async Task AddExpense()
        {
            try
            {
                var expense = new Expense
                {
                    Amount = (decimal)NewExpenseAmount,
                    ExpenseDate = SelectedDate,
                    IsRecurring = SelectedIsRecurring,
                    FrequencyInMonths = SelectedFrequencyInMonths,
                    CategoryId = (int)SelectedCategory
                };

                await _expenseRepo.AddExpenseAsync(expense);
            }
            catch (Exception ex)
            {
                // Hantera eventuella fel här, t.ex. logga eller visa ett meddelande
                Console.WriteLine($"Error adding expense: {ex.Message}");
            }
        }

        private bool CanUpdateExpense()
        {
            return true;
        }
        private async Task UpdateExpense()
        {
            throw new NotImplementedException();
        }

        private bool CanDeleteExpense()
        {
            return true;
        }
        private async Task DeleteExpense()
        {
            throw new NotImplementedException();
        }

        //*******************************************************************
        private bool CanAddNewCategory()
        {
            return SickDays!=0 || CareOfCatsDays!=0;
        }
        private async Task AddNewCategory()
        {
            throw new NotImplementedException();
        }

        //**************  FOR SUMMARY AND PROGNOSIS  ********************************
        public async Task UpdateMonthSum()
        {
            var allIncomes = await _incomeRepo.GetAllIncomesAsync();
            var allExpenses = await _expenseRepo.GetAllExpensesAsync();
            var incomeList = allIncomes.ToList();
            var expenseList = allExpenses.ToList();

            var result = calc.SumOfLastMonth(incomeList, expenseList);
            TotalIncomesLastMonth = result.TotalIncome;
            TotalExpensesLastMonth = result.TotalExpense;
            NetAmountLastMonth = result.NetAmount;

            var forecast = calc.CalculateNextMonthForecast(incomeList, expenseList, SickDays, CareOfCatsDays);

            NextMonthIn = forecast.ExpectedIn;
            NextMonthOut = forecast.ExpectedOut;
            NextMonthNet = forecast.Result;
        }
        private async Task UpdateAverages()
        {
            try
            {
                var allIncomes = await _incomeRepo.GetAllIncomesAsync();
                var incomeList = allIncomes.ToList();

                // 1. Snitt (Det du redan har)
                AvgIncomeLastThreeMonths = calc.CalculateAverage(incomeList.Select(i => new { ReceivedDate = i.ReceivedDate, Amount = i.Amount }));
                AvgExpenseLastThreeMonths = calc.CalculateAverage((await _expenseRepo.GetAllExpensesAsync()).Select(e => new { ReceivedDate = e.ExpenseDate, Amount = e.Amount }));

                // 2. Årsprognos (Använd din befintliga ProjectedSalary)
                // Vi räknar ut kompensationen baserat på snittlönen
                decimal expectedComp = calc.GetExpectedCompensationNextMonth(AvgIncomeLastThreeMonths, SickDays, CareOfCatsDays);

                // Här sätter vi din befintliga property med den nya kalkylen
                ProjectedSalary = calc.CalculateYearlyProjectionWithComp(incomeList, expectedComp);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error: {ex.Message}");
            }
        }
        

        private bool CanUpdateSickdays()
        {
            return SickDays != 0 || CareOfCatsDays != 0;
        }
        private async Task UpdateSickDays()
        {
            var stats = await _incomeRepo.GetAbsenceForMonthAsync(DateTime.Now.Year, DateTime.Now.Month);

            if (stats != null)
            {
                SickDaysCount = stats.SickDays;
                CareOfCatsDays = stats.VakDays;
            }
        }
        //***********************************************************
        //TOTALS
        private async Task UpdateYearStats()
        {
            var currentYear = DateTime.Now.Year;
            List<Income> incomes = (await _incomeRepo.GetAllIncomesAsync()).ToList();
            List<Expense> expenses = (await _expenseRepo.GetAllExpensesAsync()).ToList();

            var incomesThisYear = incomes.Where(i => i.ReceivedDate.Year == currentYear).ToList();
            var expensesThisYear = expenses.Where(e => e.ExpenseDate.Year == currentYear).ToList();

            IncomesThisYear.Clear();
            foreach (var inc in incomesThisYear) IncomesThisYear.Add(inc);

            Expenses.Clear();
            foreach (var exp in expensesThisYear) Expenses.Add(exp);
                        
            decimal avgMonthly = incomesThisYear.Any() ? incomesThisYear.Average(i => i.Amount) : 0;

            // Compensation delay for next month
            decimal expectedComp = calc.GetExpectedCompensationNextMonth(
                avgMonthly,
                SickDays,
                CareOfCatsDays);

            //Pure total income this year
            TotalIncomeYear = calc.GetTotalIncomeSoFar(incomesThisYear);

            // Total prognois with ALL incomesList and compensations for this year
            ProjectedSalary = calc.CalculateYearlyProjectionWithComp(incomesThisYear, expectedComp);

            // Yearly salary based on actual received SALARIES ONLY
            YearlySalary = calc.GetYearlySalaryOnly(incomes);

            // My top spending categories
            await GetTopSpendingInfo();
        }
        public async Task GetIncomesThisYearAsync()
        {
            var allIncomes = await _incomeRepo.GetAllIncomesAsync();
            int currentYear = DateTime.Now.Year;

            // Filtrera fram allt från i år
            var filtered = allIncomes
                .Where(x => x.ReceivedDate.Year == currentYear)
                .OrderByDescending(x => x.ReceivedDate); // Nyast först är oftast bäst

            IncomesThisYear.Clear();
            foreach (var income in filtered)
            {
                IncomesThisYear.Add(income);
            }
        }
                
        private async Task GetTopSpendingInfo()
        {
            var allExpenses = await _expenseRepo.GetAllExpensesAsync();

            // Fyll din stora Expenses-lista så UI-tabellen uppdateras [cite: 2025-11-22]
            Expenses.Clear();
            foreach (var e in allExpenses) Expenses.Add(e);

            // NU kör vi beräkningen för de tre största [cite: 2026-01-18]
            var topThree = calc.GetTopThreeSpendingCategories(Expenses.ToList());
            TopCategories.Clear();
            foreach (var cat in topThree) TopCategories.Add(cat);
        }


        

        //*******************************************************************



    }
}
