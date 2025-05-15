using Microsoft.EntityFrameworkCore;
using PersonalManager.Data;
using PersonalManager.Models;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using PersonalManager.Dialogs;

namespace PersonalManager.Pages
{
    public partial class FinancePage : Page
    {
        private readonly AppDbContext _context;
        public ObservableCollection<Transaction> Transactions { get; set; }
        public ObservableCollection<Account> Accounts { get; set; }
        public ObservableCollection<Budget> Budgets { get; set; }

        public FinancePage(AppDbContext context)
        {
            InitializeComponent();
            _context = context;
            DataContext = this;

            Transactions = new ObservableCollection<Transaction>();
            Accounts = new ObservableCollection<Account>();
            Budgets = new ObservableCollection<Budget>();

            LoadData();
        }

        private void LoadData()
        {
            LoadTransactions();
            LoadAccounts();
            LoadBudgets();
        }

        private void LoadTransactions()
        {
            Transactions.Clear();

            var transactions = _context.Transactions
                .Include(t => t.Account)
                .Include(t => t.Category)
                .OrderByDescending(t => t.Date)
                .ToList();

            foreach (var transaction in transactions)
            {
                Transactions.Add(transaction);
            }
        }

        private void LoadAccounts()
        {
            Accounts.Clear();

            var accounts = _context.Accounts
                .Include(a => a.User)
                .OrderBy(a => a.AccountName)
                .ToList();

            foreach (var account in accounts)
            {
                Accounts.Add(account);
            }
        }

        private void LoadBudgets()
        {
            Budgets.Clear();

            var budgets = _context.Budgets
                .Include(b => b.Category)
                .Include(b => b.User)
                .OrderBy(b => b.StartDate)
                .ToList();

            foreach (var budget in budgets)
            {
                Budgets.Add(budget);
            }
        }

        private void AddIncome_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new AddTransactionDialog(_context, "income");
            if (dialog.ShowDialog() == true)
            {
                LoadTransactions();
                LoadAccounts(); // Обновляем счета, так как баланс мог измениться
            }
        }

        private void AddExpense_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new AddTransactionDialog(_context, "expense");
            if (dialog.ShowDialog() == true)
            {
                LoadTransactions();
                LoadAccounts();
            }
        }

        private void AddTransfer_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new AddTransactionDialog(_context, "transfer");
            if (dialog.ShowDialog() == true)
            {
                LoadTransactions();
                LoadAccounts();
            }
        }

        private void AddAccount_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new AddAccountDialog(_context);
            if (dialog.ShowDialog() == true)
            {
                LoadAccounts();
            }
        }

        private void AddBudget_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new AddBudgetDialog(_context);
            if (dialog.ShowDialog() == true)
            {
                LoadBudgets();
            }
        }

        private void DeleteTransaction_Click(object sender, RoutedEventArgs e)
        {
            if (TransactionsGrid.SelectedItem is Transaction selectedTransaction)
            {
                var result = MessageBox.Show("Вы уверены, что хотите удалить эту транзакцию?", "Подтверждение", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    _context.Transactions.Remove(selectedTransaction);
                    _context.SaveChanges();
                    LoadTransactions();
                    LoadAccounts();
                }
            }
        }
    }
}