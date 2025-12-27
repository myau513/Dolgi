using System;
using System.Collections.Generic;
using DebtTracker.Entities;

namespace DebtTracker._Shared
{
    /// <summary>
    /// Базовый интерфейс представления (View) в архитектуре MVP.
    /// Определяет минимальный контракт, который должен поддерживать любой View.
    /// </summary>
    public interface IView
    {
        /// <summary>
        /// Событие, сигнализирующее о том, что представление загружено
        /// и готово к отображению данных.
        /// Используется Presenter'ом как точка старта логики приложения.
        /// </summary>
        event Action LoadView;

        /// <summary>
        /// Отображает сообщение об ошибке пользователю.
        /// Реализация зависит от типа UI (консоль, WinForms, WPF и т.д.).
        /// </summary>
        /// <param name="message">Текст ошибки.</param>
        void ShowError(string message);
    }

    /// <summary>
    /// Интерфейс главного представления приложения.
    /// Расширяет базовый интерфейс IView функциональностью,
    /// необходимой для отображения списка долгов.
    /// </summary>
    public interface IMainView : IView
    {
        /// <summary>
        /// Событие запроса на добавление нового долга.
        /// Генерируется представлением при соответствующем действии пользователя.
        /// </summary>
        event Action AddRequested;

        /// <summary>
        /// Отображает список долгов в пользовательском интерфейсе.
        /// Используется Presenter'ом для передачи данных в View.
        /// </summary>
        /// <param name="debts">Коллекция доменных объектов долгов.</param>
        void DisplayDebts(IEnumerable<Debt> debts);
    }
}
