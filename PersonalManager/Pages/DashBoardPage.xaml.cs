using PersonalManager.Data;
using System.Linq;
using System.Windows.Controls;

namespace PersonalManager.Pages
{
    public partial class DashboardPage : Page
    {
        private readonly AppDbContext _context;

        public DashboardPage(AppDbContext context)
        {
            InitializeComponent();
            _context = context;
            LoadDashboardData();
        }

        private void LoadDashboardData()
        {
            // Финансовая сводка
            var totalBalance = _context.Accounts.Sum(a => a.Balance);
            TotalBalance.Text = totalBalance.ToString("C");

            var totalIncome = _context.Transactions
                .Where(t => t.TransactionType == "income")
                .Sum(t => t.Amount);
            TotalIncome.Text = totalIncome.ToString("C");

            var totalExpense = _context.Transactions
                .Where(t => t.TransactionType == "expense")
                .Sum(t => t.Amount);
            TotalExpense.Text = totalExpense.ToString("C");

            RecentTasksList.ItemsSource = _context.Tasks
                .OrderByDescending(t => t.CreatedAt)
                .Take(5)
                .ToList();

            // Последние заметки
            RecentNotesList.ItemsSource = _context.Notes
                .OrderByDescending(n => n.CreatedAt)
                .Take(5)
                .ToList();
        }
    }
}