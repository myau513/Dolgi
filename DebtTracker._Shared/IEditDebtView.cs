using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DebtTracker._Shared
{
    public interface IEditDebtView
    {
        event Action SaveRequested;
        event Action CancelRequested;

        int DebtId { get; }

        string Subject { get; }
        string Description { get; }
        string Status { get; }
        DateTime Deadline { get; }

        void Fill(DebtDto debt);
        void ShowView();
        void Close();
        void ShowError(string message);
    }
}
