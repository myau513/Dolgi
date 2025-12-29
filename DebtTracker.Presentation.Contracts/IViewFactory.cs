using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DebtTracker.Presentation.Contracts
{
    /// <summary>
    /// Фабрика для создания View.
    /// Позволяет презентеру не знать, как именно создаются конкретные формы/экраны.
    /// </summary>
    public interface IViewFactory
    {
        IAddDebtView CreateAddDebtView();
        IEditDebtView CreateEditDebtView(int debtId);
    }
}

