using System.Data.Entity;  // System.Data.Entity для EF6
using DebtTracker.Entities;

namespace DebtTracker.DataAccessLayer
{
    public class DebtContext : DbContext
    {
        // Конструктор ДОЛЖЕН вызывать base("connectionStringName")
        public DebtContext() : base("name=DebtDbConnection")
        {
            // Отключаем автоматические миграции
            Database.SetInitializer<DebtContext>(null);
        }

        public DbSet<Debt> Debts { get; set; }

        // НЕТ OnConfiguring! Это только для EF Core
        // НЕТ Database.EnsureCreated! Это только для EF Core
    }
}