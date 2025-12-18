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
            // ИЗМЕНЕНИЕ 1: DebtContext должен быть Transient или ThreadScope
            Bind<DebtContext>().ToSelf().InTransientScope(); // ИЛИ .InScope(ctx => StandardScopeCallbacks.Thread(ctx));

            // ИЗМЕНЕНИЕ 2: DbContext тоже Transient
            Bind<DbContext>().ToMethod(ctx => ctx.Kernel.Get<DebtContext>()).InTransientScope();

            // ИЗМЕНЕНИЕ 3: Репозиторий тоже должен быть Transient
            Bind<IRepository<Debt>>().To<EntityFrameworkRepository<Debt>>().InTransientScope();

            // Сервисы оставляем как есть
            Bind<IDebtService>().To<DebtService>().InTransientScope();
            Bind<IDebtValidator>().To<DebtValidator>().InSingletonScope();
        }
    }
}