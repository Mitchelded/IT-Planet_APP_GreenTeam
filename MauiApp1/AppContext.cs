using Microsoft.EntityFrameworkCore;
using MauiApp1.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiApp1
{
    internal class AppContext : DbContext
    {
        public DbSet<DayTask> Tasks { get; set; } = null!;
        public DbSet<TaskDecorations> TaskDecorations { get; set; } = null!;
        public DbSet<Options> Options { get; set; } = null!;

        public AppContext()
        {
            //Database.EnsureDeleted();
            Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //var t = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            //         string mainDir = Path.Combine(t, "Diary");
            //         Directory.CreateDirectory(mainDir);
            //var t = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);

            //var sqlitePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), 
            //    @"OlsonSoftware\FinanceManager"); 
            //Directory.CreateDirectory(sqlitePath); 
            //var fileName = $"{sqlitePath}\fmd.db"; 
            //if (!File.Exists(fileName)) 
            //    File.Create(fileName); 
            //optionsBuilder.UseSqlite($"Data Source={fileName}");
            //var t = System.Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
            var t = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            optionsBuilder.UseSqlite($"Data Source=/{t}/DiaryDB.db;");
            base.OnConfiguring(optionsBuilder);
        }
    }
}
