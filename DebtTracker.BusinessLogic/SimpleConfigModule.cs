using DebtTracker.DataAccessLayer;
using DebtTracker.Entities;
using Ninject;
using Ninject.Modules;
using System.Data.Entity;

namespace DebtTracker.BusinessLogic
{
    /// <summary>
    /// Модуль конфигурации Ninject для внедрения зависимостей.
    /// Определяет все зависимости и их жизненные циклы для DI-контейнера.
    /// </summary>
    public class SimpleConfigModule : NinjectModule
    {
        /// <summary>
        /// Загружает конфигурацию связей (bindings) для DI-контейнера Ninject.
        /// </summary>
        public override void Load()
        {
            // 1. Регистрация контекста базы данных Entity Framework
            // DebtContext регистрируется как синглтон для всего приложения
            Bind<DebtContext>().ToSelf().InSingletonScope();

            // 2. Связывание абстракции DbContext с конкретной реализацией DebtContext
            // Когда требуется DbContext, возвращается экземпляр DebtContext
            Bind<DbContext>().ToMethod(ctx => ctx.Kernel.Get<DebtContext>()).InSingletonScope();

            // 3. Регистрация репозитория для работы с долгами
            // Используется EntityFrameworkRepository с синглтон скоупом
            Bind<IRepository<Debt>>().To<EntityFrameworkRepository<Debt>>().InSingletonScope();

            // 4. Регистрация сервисов бизнес-логики
            // DebtService - transient (новый экземпляр для каждого запроса)
            Bind<IDebtService>().To<DebtService>().InTransientScope();
            // DebtValidator - синглтон (не имеет состояния, можно переиспользовать)
            Bind<IDebtValidator>().To<DebtValidator>().InSingletonScope();
        }
    }
}