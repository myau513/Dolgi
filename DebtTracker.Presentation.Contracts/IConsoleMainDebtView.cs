using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DebtTracker.Presentation.Contracts
{
    /// <summary>
    /// Контракт для консольного главного экрана.
    /// Наследуется от IMainDebtView и добавляет метод запуска цикла.
    /// </summary>
    public interface IConsoleMainDebtView : IMainDebtView
    {
        void RunLoop();
    }
}

