using System;
using DebtTracker.Dto;
using DebtTracker.Presentation.Contracts;

namespace DebtTracker.ConsoleApp
{
    /// <summary>
    /// Фабрика консольных представлений (View) для MVP.
    /// Создаёт нужные экраны консоли: добавление и редактирование долгов.
    /// </summary>
    public class ConsoleViewFactory : IViewFactory
    {
        public IAddDebtView CreateAddDebtView()
        {
            return new ConsoleAddDebtView();
        }

        public IEditDebtView CreateEditDebtView(int debtId)
        {
            return new ConsoleEditDebtView(debtId);
        }
    }

}
