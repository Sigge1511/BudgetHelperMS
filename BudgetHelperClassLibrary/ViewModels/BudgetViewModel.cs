using BudgetHelperClassLibrary.CalculationService;
using BudgetHelperClassLibrary.Commands;
using BudgetHelperClassLibrary.Models;
using BudgetHelperClassLibrary.Repositories;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
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
        private Category? _selectedCategory;
        public Category? SelectedCategory
        {
            get => _selectedCategory;
            set { _selectedCategory = value; OnPropertyChanged(); AddExpenseComm.RaiseCanExecuteChanged(); }
        }

        private IncomeSource? _selectedSource;
        public IncomeSource? SelectedSource
        {
            get => _selectedSource;
            set { _selectedSource = value; OnPropertyChanged(); AddIncomeComm.RaiseCanExecuteChanged(); }
        }

        private decimal? _newIncomeAmount;
        public decimal? NewIncomeAmount
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
            set { sickDays = value; OnPropertyChanged();
                UpdateSickDaysComm?.RaiseCanExecuteChanged();
            }
        }
        private int sickDaysCount;
        public int SickDaysCount
        {
            get => sickDaysCount;
            set { sickDaysCount = value; OnPropertyChanged(); }
        }



        private int careOfCatsDays;
        public int CareOfCatsDays
        {
            get => careOfCatsDays;
            set { careOfCatsDays = value; OnPropertyChanged();
                UpdateSickDaysComm?.RaiseCanExecuteChanged();
            }
        }
        private int careOfCatsDaysCount;
        public int CareOfCatsDaysCount
        {
            get => careOfCatsDaysCount;
            set { careOfCatsDaysCount = value; OnPropertyChanged(); }
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
            AddIncomeComm = new RelayCommand(async _ => await AddIncome(), () => CanAddIncome());
            UpdateIncomeComm = new RelayCommand(async param => await UpdateIncome(param));
            DeleteIncomeComm = new RelayCommand(async param => await DeleteIncome(param));

            AddExpenseComm = new RelayCommand(async _ => await AddExpense(), () => CanAddExpense());
            UpdateExpenseComm = new RelayCommand(async param => await UpdateExpense(param));
            DeleteExpenseComm = new RelayCommand(async param => await DeleteExpense(param));

            UpdateMonthSumComm = new RelayCommand(async _ => await UpdateMonthSum());
            UpdateSickDaysComm = new RelayCommand(ExecuteUpdateSickDays, CanUpdateSickDays);

            AddNewCategoryComm = new RelayCommand(async _ => await AddNewCategory(), CanAddNewCategory);

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
            foreach (var e in expenseList.OrderByDescending(x => x.ExpenseDate))
            {
                LatestExpenses.Add(e);
            }
            //*******************************************************************          
            var incomesList = await _incomeRepo.GetAllIncomesAsync();
            LatestIncomes.Clear();
            foreach (var i in incomesList.OrderByDescending(x => x.ReceivedDate))
            {
                LatestIncomes.Add(i);
            }

            //*******************************************************************          

            AvgIncomeLastThreeMonths = calc.CalculateAverage(incomesList);
            await UpdateMonthSum();
            await UpdateAverages();
            await UpdateYearStats();
            await GetIncomesThisYearAsync();
        }

        

        //***********   METODER   ********************************************************
        private bool CanAddIncome()
        {
            // Check for valid data
            return NewIncomeAmount > 0 
                && SelectedSource!=null 
                && SelectedSource.Id!=0;
        }
        private async Task AddIncome()
        {
            try
            {
                decimal amount = (decimal)NewIncomeAmount;
                var newIncome = new Income
                {
                    Amount = amount, // Här sparas det (ev. omräknade) beloppet
                    ReceivedDate = SelectedDate,
                    IncomeSourceId = SelectedSource.Id,
                    IncomeSource = SelectedSource
                };
                await _incomeRepo.AddIncomeAsync(newIncome);

                NewIncomeAmount = null;
                SelectedSource = null;                
                SelectedDate = DateTime.Now; // Reset everything

                await LoadDataAsync(); //Refresh data everywhere after adding                                       
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding income: {ex.Message}");
            }
        }

        //private bool CanUpdateIncome() => true;
        private async Task UpdateIncome(object? parameter)
        {
            try
            {
                if (parameter is Income incomeToUpdate)
                {
                    // Using income object passed as parameter
                    bool? result = _windowService.ShowUpdateIncomeDialog(incomeToUpdate, IncomeSources);

                    if (result == true)
                    {
                        await _incomeRepo.UpdateIncomeAsync(incomeToUpdate);
                        await LoadDataAsync(); // Updating data in UI after update
                    }
                }
            }
            catch (Exception ex) {Console.WriteLine($"Error updating income: {ex.Message}");}            
        }

        //private bool CanDeleteIncome() => true;
        private async Task DeleteIncome(object? parameter)
        {
            try
            {
                if (parameter is Income incomeToDelete)
                {
                    // Using income object passed as parameter
                    bool? result = _windowService.ShowDeleteIncomeDialog(incomeToDelete);

                    if (result == true)
                    {
                        await _incomeRepo.DeleteIncomeAsync(incomeToDelete);
                        await LoadDataAsync(); // Updating data in UI after update
                    }
                }
            }
            catch (Exception ex) { Console.WriteLine($"Error deleting income: {ex.Message}"); }            
        }
        //*******************************************************************
        
        private bool CanAddExpense()
        {
            return  SelectedCategory != null && NewExpenseAmount.HasValue
                    && NewExpenseAmount > 0
                    && !string.IsNullOrWhiteSpace(NewExpenseName);
        }
        private async Task AddExpense()
        {
            try
            {
                var expense = new Expense
                {
                    Amount = (decimal)NewExpenseAmount,
                    Name = NewExpenseName,
                    ExpenseDate = SelectedDate,
                    IsRecurring = SelectedIsRecurring,
                    FrequencyInMonths = SelectedFrequencyInMonths,
                    CategoryId = SelectedCategory.Id,
                    Category=SelectedCategory
                };

                await _expenseRepo.AddExpenseAsync(expense);

                //clear fields
                NewExpenseAmount = null;
                NewExpenseName = string.Empty;
                SelectedDate = DateTime.Now;
                SelectedCategory = null;
                SelectedIsRecurring = false;
                SelectedFrequencyInMonths = null;

                await LoadDataAsync(); //Refresh data everywhere after adding
            }
            catch (Exception ex)
            {
                // Hantera eventuella fel här, t.ex. logga eller visa ett meddelande
                Console.WriteLine($"Error adding expense: {ex.Message}");
            }
        }

        //private bool CanUpdateExpense() => true;
        private async Task UpdateExpense(object? parameter)
        {
            try
            {
                if (parameter is Expense expenseToUpdate)
                {
                    // Using object passed as parameter
                    bool? result = _windowService.ShowUpdateExpenseDialog(expenseToUpdate, Categories);

                    if (result == true)
                    {                        
                        await _expenseRepo.UpdateExpenseAsync(expenseToUpdate);
                    }
                }
                await LoadDataAsync();
            }
            catch (Exception ex) { Console.WriteLine($"Error updating income: {ex.Message}"); }
        }

        //private bool CanDeleteExpense() => true;
        private async Task DeleteExpense(object? parameter)
        {
            try
            {
                if (parameter is Expense expenseToDelete)
                {
                    // Using income object passed as parameter
                    bool? result = _windowService.ShowDeleteExpenseDialog(expenseToDelete);

                    if (result == true)
                    {
                        await _expenseRepo.DeleteExpenseAsync(expenseToDelete);
                        await LoadDataAsync(); // Updating data in UI after update
                    }
                }
            }
            catch (Exception ex) { Console.WriteLine($"Error deleting income: {ex.Message}"); }
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

            var forecast = calc.CalculateNextMonthForecast(
                    incomeList,
                    expenseList,
                    SickDaysCount,
                    CareOfCatsDaysCount);
            
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


        private async void ExecuteUpdateSickDays(object? parameter)
        {
            await UpdateSickDays();
        }

        private bool CanUpdateSickDays()
        {
            return SickDays > 0 || CareOfCatsDays > 0;
        }
        private async Task UpdateSickDays()
        {
            // Try saving input to database
            await _incomeRepo.SaveOrUpdateAbsenceAsync(
                            DateTime.Now.Year,
                            DateTime.Now.Month,
                            SickDays,
                            CareOfCatsDays);

            // 2. Hämta de uppdaterade siffrorna
            var stats = await _incomeRepo.GetAbsenceForMonthAsync(DateTime.Now.Year, DateTime.Now.Month);
            //And add to counter prop
            if (stats != null)
            {
                SickDaysCount = stats.SickDays;
            }

            //Clean input fields
            SickDays = 0;
            CareOfCatsDays = 0;

            // Update info to all calcs
            await LoadDataAsync();
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
