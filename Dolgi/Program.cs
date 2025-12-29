using DebtTracker.Dto;
using DebtTracker.Presentation.Contracts;
using DebtTracker.BusinessLogic;
using DebtTracker.Presenter;
using Ninject;

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
