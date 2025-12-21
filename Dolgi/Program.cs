using DebtTracker._Shared;
using DebtTracker.BusinessLogic;
using DebtTracker.DataAccessLayer;
using DebtTracker.Presenter;
using Ninject;
using Presenter;

namespace DebtTracker.ConsoleApp
{
    class Program
    {
        static void Main()
        {
            var kernel = new StandardKernel(new SimpleConfigModule());

            var service = kernel.Get<IDebtService>();
            var model = new DebtModel(service);
            var view = new ConsoleDebtView();

            // ВАЖНО: используем MainDebtPresenter
            var presenter = new MainDebtPresenter(
                view,
                model,
                new ConsoleViewFactory() // если нужен Add/Edit
            );

            view.Run();
        }
    }
}
