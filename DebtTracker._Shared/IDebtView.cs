using DebtTracker.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DebtTracker._Shared
{
    public interface IDebtView : IView
    {
        event Action LoadView;
        event Action AddRequested;

        string Subject { get; }
        string Description { get; }
        string Status { get; }
        DateTime Deadline { get; }

        void ShowDebts(IEnumerable<DebtDto> debts);
        void ShowError(string message);
    }

}
