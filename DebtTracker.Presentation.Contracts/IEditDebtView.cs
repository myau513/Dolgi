using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DebtTracker.Presentation.Contracts
{
    /// <summary>
    /// Контракт окна/экрана редактирования существующего долга.
    /// </summary>
    public interface IEditDebtView : IView
    {
        /// <summary>
        /// Id редактируемого долга.
        /// </summary>
        int DebtId { get; }

        event Action SaveRequested;
        event Action CancelRequested;

        string Subject { get; set; }
        string Description { get; set; }
        string Status { get; set; }
        DateTime Deadline { get; set; }

        void Show();
        void Close();
    }
}

