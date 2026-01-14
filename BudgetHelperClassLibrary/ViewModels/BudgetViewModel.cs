using BudgetHelperClassLibrary.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BudgetHelperClassLibrary.ViewModels
{
    public class BudgetViewModel : ObservableObject
    {
        public RelayCommand AddIncomeComm { get; }
        public RelayCommand AddExpenseComm { get; }
        public RelayCommand DeleteIncomeComm { get; }
        public RelayCommand DeleteExpenseComm { get; }


        public BudgetViewModel()
        {
            AddIncomeComm = new RelayCommand(AddIncome, CanAddIncome);
            AddExpenseComm = new RelayCommand(AddExpense, CanAddExpense);
            DeleteIncomeComm = new RelayCommand(DeleteIncome, CanDeleteIncome);
            DeleteExpenseComm = new RelayCommand(DeleteExpense, CanDeleteExpense);
        }

        private bool CanAddIncome()
        {
            throw new NotImplementedException();
        }

        private void AddIncome()
        {
            throw new NotImplementedException();
        }
//*******************************************************************
        private bool CanAddExpense()
        {
            throw new NotImplementedException();
        }

        private void AddExpense()
        {
            throw new NotImplementedException();
        }
//*******************************************************************
        private bool CanDeleteIncome()
        {
            throw new NotImplementedException();
        }

        private void DeleteIncome()
        {
            throw new NotImplementedException();
        }
//*******************************************************************
        private bool CanDeleteExpense()
        {
            throw new NotImplementedException();
        }      
        private void DeleteExpense()
        {
            throw new NotImplementedException();
        }
    }
}
