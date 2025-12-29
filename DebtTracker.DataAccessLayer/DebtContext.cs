using System.Data.Entity;  // System.Data.Entity для EF6
using DebtTracker.Entities;

namespace DebtTracker.DataAccessLayer
{
    /// <summary>
    /// Контекст базы данных Entity Framework 6 для приложения DebtTracker.
    /// Наследуется от <see cref="DbContext"/> и управляет подключением к базе данных и набором сущностей.
    /// </summary>
    /// <remarks>
    /// Особенности конфигурации Entity Framework 6:
    /// <list type="bullet">
    /// <item>Использует подход Code First для работы с базой данных</item>
    /// <item>Настройка подключения через строку подключения в конфигурационном файле</item>
    /// <item>Отключены автоматические миграции для предотвращения неожиданных изменений схемы</item>
    /// <item>Содержит DbSet для каждой сущности, требующей сохранения в БД</item>
    /// </list>
    /// Важно: Этот класс настроен для EF6, а не для EF Core. Для EF Core используется другой подход конфигурации.
    /// </remarks>
    public class DebtContext : DbContext
    {
        /// <summary>
        /// Инициализирует новый экземпляр контекста базы данных.
        /// Использует строку подключения с именем "DebtDbConnection" из конфигурационного файла.
        /// </summary>
        /// <remarks>
        /// Ключевые настройки конструктора:
        /// <list type="bullet">
        /// <item>"name=DebtDbConnection" указывает на строку подключения в App.config/Web.config</item>
        /// <item>Database.SetInitializer(null) отключает автоматические миграции и Code First инициализацию</item>
        /// <item>Безопасность: явное управление созданием БД предотвращает случайную потерю данных</item>
        /// </list>
        /// </remarks>
        public DebtContext() : base("name=DebtDbConnection")
        {
            Database.SetInitializer<DebtContext>(null);
        }

        /// <summary>
        /// Набор сущностей Debt (Долги) для операций CRUD.
        /// Представляет таблицу "Debts" в базе данных.
        /// </summary>
        /// <remarks>
        /// DbSet предоставляет:
        /// <list type="bullet">
        /// <item>Доступ к данным через LINQ-запросы</item>
        /// <item>Возможность добавления, обновления и удаления сущностей</item>
        /// <item>Автоматическое отслеживание изменений сущностей</item>
        /// <item>Отложенную загрузку связанных данных</item>
        /// </list>
        /// </remarks>
        public DbSet<Debt> Debts { get; set; }
    }
}