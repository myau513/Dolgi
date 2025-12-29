using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DebtTracker.Presentation.Contracts
{
    /// <summary>
    /// Контракт окна/экрана добавления долга.
    /// </summary>
    public interface IAddDebtView : IView
    {
        /// <summary>
        /// Событие: пользователь нажал "Сохранить".
        /// </summary>
        event Action SaveRequested;

        /// <summary>
        /// Событие: пользователь нажал "Отмена" / закрыл форму.
        /// </summary>
        event Action CancelRequested;

        /// <summary>
        /// Поля ввода.
        /// </summary>
        string Subject { get; set; }
        string Description { get; set; }
        string Status { get; set; }    
        DateTime Deadline { get; set; }

        /// <summary>
        /// Показать экран добавления (например, ShowDialog для WinForms или запуск цикла в консоли).
        /// </summary>
        void Show();

        /// <summary>
        /// Закрыть экран (форма закрывается, консольный ввод прекращается и т.д.).
        /// </summary>
        void Close();
    }
}

