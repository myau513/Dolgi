using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DebtTracker.Presentation.Contracts
{
    /// <summary>
    /// Базовый интерфейс представления (View) в архитектуре MVP.
    /// Любой экран (главное окно, форма добавления и т.д.) обязан
    /// уметь сигнализировать о загрузке и показывать ошибки.
    /// </summary>
    public interface IView
    {
        /// <summary>
        /// Событие, сигнализирующее о том, что представление загружено
        /// и готово к работе (например, форма отрисована).
        /// </summary>
        event Action LoadView;

        /// <summary>
        /// Показать сообщение об ошибке пользователю.
        /// Конкретная реализация зависит от типа UI (MessageBox, 
        /// Console.WriteLine, всплывающее окно и т.п.).
        /// </summary>
        void ShowError(string message);

        /// <summary>
        /// Показать информационное сообщение пользователю.
        /// Не обязательно, но удобно иметь базово.
        /// </summary>
        void ShowMessage(string message);
    }
}

