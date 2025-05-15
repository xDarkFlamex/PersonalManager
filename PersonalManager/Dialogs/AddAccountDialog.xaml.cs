using Microsoft.EntityFrameworkCore;
using PersonalManager.Data;
using PersonalManager.Models;
using System;
using System.Windows;

namespace PersonalManager.Dialogs
{
    public partial class AddAccountDialog : Window
    {
        private readonly AppDbContext _context;
        public Account NewAccount { get; set; }

        public AddAccountDialog(AppDbContext context)
        {
            InitializeComponent();
            _context = context;
            NewAccount = new Account();
            DataContext = this;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (NewAccount != null)
            {
                NewAccount.CreatedAt = DateTime.Now;
                NewAccount.IsActive = true;
                _context.Accounts.Add(NewAccount);
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