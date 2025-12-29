using System;
using DebtTracker.Dto;
using DebtTracker.Presentation.Contracts;

namespace DebtTracker.ConsoleApp
{
    public class ConsoleViewFactory : IViewFactory
    {
        public IAddDebtView CreateAddDebtView()
            => new ConsoleAddDebtView();

        public IEditDebtView CreateEditDebtView(int debtId)
            => new ConsoleEditDebtView(debtId);
    }

}
