using System;

namespace DebtTracker._Shared
{
    /// <summary>
    /// Фабрика представлений (View) в архитектуре MVP.
    /// Используется Presenter'ом для создания конкретных View
    /// без прямой зависимости от UI-фреймворка (WinForms, Console и т.д.).
    /// </summary>
    public interface IViewFactory
    {
        /// <summary>
        /// Создаёт представление для добавления нового долга.
        /// Presenter использует этот метод для открытия формы добавления,
        /// не зная конкретной реализации View.
        /// </summary>
        /// <returns>Экземпляр представления добавления долга.</returns>
        IAddDebtView CreateAddDebtView();

        /// <summary>
        /// Создаёт представление для редактирования существующего долга.
        /// Presenter передаёт идентификатор долга, который требуется отредактировать.
        /// </summary>
        /// <param name="debtId">Идентификатор редактируемого долга.</param>
        /// <returns>Экземпляр представления редактирования долга.</returns>
        IEditDebtView CreateEditDebtView(int debtId);
    }
}
