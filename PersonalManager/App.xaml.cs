using Microsoft.EntityFrameworkCore;
using PersonalManager.Data;
using System;
using System.Windows;

namespace PersonalManager
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
            base.OnStartup(e);

            // Применяем миграции при старте
            using (var context = new AppDbContext())
            {
                context.Database.Migrate();
            }


        }
    }
}