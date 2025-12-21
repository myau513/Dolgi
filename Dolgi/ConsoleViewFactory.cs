using DebtTracker._Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
