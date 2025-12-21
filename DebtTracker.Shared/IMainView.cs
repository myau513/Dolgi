using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DebtTracker.Shared
{
    public interface IMainView
    {
        // события (View → Presenter)
        event Action AddRequested;
        event Action EditRequested;
        event Action DeleteRequested;
        event Action RefreshRequested;

        int? SelectedDebtId { get; }

        // методы (Presenter → View)
        void ShowDebts(IEnumerable<DebtDto> debts);
        void ShowTomorrowWarning(IEnumerable<DebtDto> debts);
        void ShowError(string message);
        void ShowInfo(string message);
    }


}
