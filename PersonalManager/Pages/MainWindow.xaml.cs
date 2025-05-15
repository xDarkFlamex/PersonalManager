using PersonalManager.Data;
using PersonalManager.Pages;
using System;
using System.Windows;

namespace PersonalManager
{
    public partial class MainWindow : Window
    {
        public AppDbContext Context { get; } 

        public MainWindow()
        {
            InitializeComponent();

            // Инициализация контекста БД
            Context = new AppDbContext();

            // Загрузка главной страницы
            MainFrame.Navigate(new DashboardPage(Context));
        }

        private void BtnDashboard_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new DashboardPage(Context));
        }

        private void BtnFinance_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new FinancePage(Context));
        }

        private void BtnNotes_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new NotesPage(Context));
        }

        private void BtnTasks_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new TasksPage(Context));
        }

        protected override void OnClosed(EventArgs e)
        {
            Context.Dispose();
            base.OnClosed(e);
        }



    }
}