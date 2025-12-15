using DebtTracker.DataAccessLayer;
using DebtTracker.Entities;
using Ninject;
using Ninject.Modules;
using System.Data.Entity;

namespace DebtTracker.BusinessLogic
{
    public class SimpleConfigModule : NinjectModule
    {
        public override void Load()
        {
            // 1. DbContext - регистрируем DebtContext как себя самого
            Bind<DebtContext>().ToSelf().InSingletonScope();

            // 2. И говорим, что когда требуется DbContext, возвращай DebtContext
            Bind<DbContext>().ToMethod(ctx => ctx.Kernel.Get<DebtContext>()).InSingletonScope();

            // 3. Репозитории
            Bind<IRepository<Debt>>().To<EntityFrameworkRepository<Debt>>().InSingletonScope();

            // 4. Сервисы
            Bind<IDebtService>().To<DebtService>().InTransientScope();
            Bind<IDebtValidator>().To<DebtValidator>().InSingletonScope();
        }
    }
}