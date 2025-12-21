using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DebtTracker.Entities;

namespace DebtTracker._Shared
{
    public interface IView
    {
        event Action LoadView;
        void ShowError(string message);
    }
    public interface IMainView : IView
    {
        event Action AddRequested;
        void DisplayDebts(IEnumerable<Debt> debts);
    }
}
