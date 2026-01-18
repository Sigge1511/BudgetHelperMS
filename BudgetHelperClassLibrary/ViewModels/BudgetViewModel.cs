using BudgetHelperClassLibrary.Commands;
using BudgetHelperClassLibrary.Models;
using BudgetHelperClassLibrary.CalculationService;
using BudgetHelperClassLibrary.Repositories;
using System.Collections.ObjectModel;


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

        // --- 2. Properties för valda objekt ---
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

        // Listan som visar alla inkomster i UI:t (Denna behöver uppdateras manuellt)
        public ObservableCollection<Income> AllIncomes { get; set; } = new();

        public RelayCommand AddIncomeComm { get; }
        public RelayCommand AddExpenseComm { get; }
        public RelayCommand DeleteIncomeComm { get; }
        public RelayCommand DeleteExpenseComm { get; }
        public RelayCommand UpdateMonthSumComm { get; }

        private readonly IIncomeRepo _incomeRepo;
        private readonly IExpenseRepo _expenseRepo;
        private readonly ICategoryRepo _categoryRepo;
        private readonly IBudgetRepo _budgetRepo;

        public BudgetViewModel(IIncomeRepo incomeRepo, IExpenseRepo expenseRepo,
                               ICategoryRepo categoryRepo, IBudgetRepo budgetRepo)
        {
            AddIncomeComm = new RelayCommand(async () => await AddIncome(), CanAddIncome);
            AddExpenseComm = new RelayCommand(async () => await AddExpense(), CanAddExpense);
            DeleteIncomeComm = new RelayCommand(async () => await DeleteIncome(), CanDeleteIncome);
            DeleteExpenseComm = new RelayCommand(async () => await DeleteExpense(), CanDeleteExpense);
            UpdateMonthSumComm = new RelayCommand(async () => await UpdateMonthSum());

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
            var incomes = await _incomeRepo.GetAllIncomesAsync();
            LatestIncomes.Clear();
            foreach (var i in incomes) LatestIncomes.Add(i);
            var latestIncomesList = incomes.OrderByDescending(x => x.Id).Take(5);
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
                var newIncome = new Income
                {
                    Amount = NewIncomeAmount!,
                    ReceivedDate = SelectedDate,
                    IncomeSourceId = (int)SelectedSource
                };

                await _incomeRepo.AddIncomeAsync(newIncome);

                // Nollställ allt efteråt
                NewIncomeAmount = 0;
                SelectedSource = null;
                SelectedDate = DateTime.Now; // Återställ allt igen
            }
            catch (Exception ex)
            {
                // Hantera eventuella fel här, t.ex. logga eller visa ett meddelande
                Console.WriteLine($"Error adding income: {ex.Message}");
            }
        }
//*******************************************************************
        private bool CanAddExpense()
        {
            return true;
                //NewExpenseAmount > 0
                //       && SelectedCategory != null
                //       && !string.IsNullOrWhiteSpace(NewExpenseName);
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
        //*******************************************************************
        public async Task UpdateMonthSum()
        {
            var allIncomes = await _incomeRepo.GetAllIncomesAsync();
            var allExpenses = await _expenseRepo.GetAllExpensesAsync();

            var calc = new Calculator();
            var result = calc.SumOfLastMonth(allIncomes.ToList(), allExpenses.ToList());

            // 3. Uppdatera dina variabler (OnPropertyChanged triggar UI-uppdateringen)
            TotalIncomesLastMonth = result.TotalIncome;
            TotalExpensesLastMonth = result.TotalExpense;
            NetAmountLastMonth = result.NetAmount;
        }

        //*******************************************************************
        private bool CanDeleteIncome()
        {
            throw new NotImplementedException();
        }

        private async Task DeleteIncome()
        {
            throw new NotImplementedException();
        }
        //*******************************************************************
        private bool CanDeleteExpense()
        {
            throw new NotImplementedException();
        }
        private async Task DeleteExpense()
        {
            throw new NotImplementedException();
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

        private async Task<ObservableCollection<Income>> GetLatestIncomesAsync()
        {
            var latestIncomes = await _incomeRepo.GetAllIncomesAsync();
            return new ObservableCollection<Income>(latestIncomes);
        }
    }
}
