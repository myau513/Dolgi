using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DebtTracker._Shared
{
    public interface IMainDebtView
    {
        event Action LoadView;
        event Action AddRequested;
        event Action EditRequested;
        event Action DeleteRequested;
        event Action RefreshRequested;

        int SelectedDebtId { get; } 

        void ShowDebts(IEnumerable<DebtDto> debts);
        void ShowMessage(string message);
        void ShowError(string message);
    }



}
