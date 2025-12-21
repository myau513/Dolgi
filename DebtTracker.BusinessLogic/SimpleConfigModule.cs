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
        public override void Load()
        {
            // DbContext — transient (ПРАВИЛЬНО)
            Bind<DebtContext>().ToSelf().InTransientScope();
            Bind<DbContext>()
                .ToMethod(ctx => ctx.Kernel.Get<DebtContext>())
                .InTransientScope();

            // Репозиторий — transient
            Bind<IRepository<Debt>>()
                .To<EntityFrameworkRepository<Debt>>()
                .InTransientScope();

            // Бизнес-логика
            Bind<IDebtService>()
                .To<DebtService>()
                .InTransientScope();

            Bind<IDebtValidator>()
                .To<DebtValidator>()
                .InSingletonScope();

            Bind<IDebtModel>()
                .To<DebtModel>()
                .InSingletonScope();
        }

    }
}