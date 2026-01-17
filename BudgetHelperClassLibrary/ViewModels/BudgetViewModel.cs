using BudgetHelperClassLibrary.Commands;
using BudgetHelperClassLibrary.Models;
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

        // Listan som visar alla inkomster i UI:t (Denna behöver uppdateras manuellt)
        public ObservableCollection<Income> AllIncomes { get; set; } = new();

        public RelayCommand AddIncomeComm { get; }
        public RelayCommand AddExpenseComm { get; }
        public RelayCommand DeleteIncomeComm { get; }
        public RelayCommand DeleteExpenseComm { get; }

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
            Expenses.Clear();
            // 1. Hämta datan som den är (troligen en List eller IEnumerable)
            var sources = await _incomeRepo.GetAllIncomeSourcesAsync();

            // 2. Rensa din existerande ObservableCollection som finns i VM
            IncomeSources.Clear();

            // 3. Loopa igenom resultatet och lägg till i din samling
            if (sources != null)
            {
                foreach (var s in sources)
                {
                    IncomeSources.Add(s);

                }
            }
            // Gör samma sak för IncomeSources (om du har ett sånt repo)
            // var sources = await _sourceRepo.GetAllSourcesAsync();
            // foreach (var s in sources) IncomeSources.Add(s);

            // Hämta inkomster
            var incomes = await _incomeRepo.GetAllIncomesAsync();
            // Sortera (t.ex. på datum om du har det) och ta de 5 senaste
            var latest = incomes.OrderByDescending(x => x.Id).Take(5);

            LatestIncomes.Clear();
            foreach (var item in latest) LatestIncomes.Add(item);

            // Gör samma sak för Expenses...
            var expenses = await _expenseRepo.GetAllExpensesAsync();
            var latestEx = expenses.OrderByDescending(x => x.Id).Take(5);

            LatestExpenses.Clear();
            foreach (var item in latestEx) LatestExpenses.Add(item);
        }



        private bool CanAddIncome()
        {
            // Koll: Belopp, Kategori och Källa
            return NewIncomeAmount > 0 && SelectedCategory != null && SelectedSource != null;
        }

        private async Task AddIncome()
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
            SelectedCategory = null;
            SelectedSource = null;
            SelectedDate = DateTime.Now; // Återställ allt igen
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

            var expense = new Expense
            { Amount = (decimal)NewExpenseAmount,
              ExpenseDate = SelectedDate,
              IsRecurring = SelectedIsRecurring,
              FrequencyInMonths = SelectedFrequencyInMonths,
              CategoryId = (int)SelectedCategory
            };
            await _expenseRepo.AddExpenseAsync(expense);
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
