using PersonalManager.Data;
using PersonalManager.Models;
using System;
using System.Linq;
using System.Windows;

namespace PersonalManager.Dialogs
{
    public partial class AddTransactionDialog : Window
    {
        private readonly AppDbContext _context;
        private readonly string _transactionType;

        public string TransactionTypeName => _transactionType switch
        {
            "income" => "Доход",
            "expense" => "Расход",
            "transfer" => "Перевод",
            _ => "Транзакция"
        };

        public AddTransactionDialog(AppDbContext context, string transactionType)
        {
            InitializeComponent();
            _context = context;
            _transactionType = transactionType;
            LoadData();
        }

        private void LoadData()
        {
            // Загрузка счетов
            AccountComboBox.ItemsSource = _context.Accounts.ToList();

            // Загрузка категорий по типу транзакции
            CategoryComboBox.ItemsSource = _context.TransactionCategories
                .Where(c => c.Type == _transactionType)
                .ToList();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (!decimal.TryParse(AmountTextBox.Text, out decimal amount) || amount <= 0)
            {
                MessageBox.Show("Введите корректную сумму", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (AccountComboBox.SelectedItem == null)
            {
                MessageBox.Show("Выберите счет", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var transaction = new Transaction
            {
                Amount = amount,
                TransactionType = _transactionType,
                Date = DateTime.Now,
                AccountId = ((Account)AccountComboBox.SelectedItem).AccountId,
                CategoryId = ((TransactionCategory)CategoryComboBox.SelectedItem)?.CategoryId
            };

            _context.Transactions.Add(transaction);
            _context.SaveChanges();

            DialogResult = true;
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}