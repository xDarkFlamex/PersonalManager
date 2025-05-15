using Microsoft.EntityFrameworkCore;
using PersonalManager.Data;
using PersonalManager.Models;
using System.Collections.ObjectModel;
using System;
using System.Linq;
using System.Windows;

namespace PersonalManager.Dialogs
{
    public partial class AddBudgetDialog : Window
    {
        private readonly AppDbContext _context;
        public Budget NewBudget { get; set; }
        public ObservableCollection<TransactionCategory> Categories { get; set; }

        public AddBudgetDialog(AppDbContext context)
        {
            InitializeComponent();
            _context = context;
            NewBudget = new Budget { StartDate = DateOnly.FromDateTime(DateTime.Now) };
            Categories = new ObservableCollection<TransactionCategory>(_context.TransactionCategories.ToList());
            DataContext = this;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (NewBudget != null)
            {
                _context.Budgets.Add(NewBudget);
                _context.SaveChanges();
                DialogResult = true;
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}