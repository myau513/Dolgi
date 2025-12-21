using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DebtTracker._Shared
{
    public interface IAddDebtView
    {
        event Action SaveRequested;
        event Action CancelRequested;

        string Subject { get; }
        string Description { get; }
        string Status { get; }
        DateTime Deadline { get; }

        void Close();
        void ShowView(); 
        void ShowError(string message);
    }
}
