using DebtTracker.Presentation.Contracts;

namespace DebtTracker.WUI
{
    /// <summary>
    /// Фабрика View для WinForms-интерфейса.
    /// Отвечает за создание окон (форм) и скрывает конкретные типы
    /// от Presenter'ов, чтобы они работали только с интерфейсами.
    /// </summary>
    public class WinFormsViewFactory : IViewFactory
    {
        /// <summary>
        /// Создаёт форму добавления долга.
        /// </summary>
        /// <returns>Экземпляр формы, реализующий IAddDebtView.</returns>
        public IAddDebtView CreateAddDebtView()
        {
            return new AddDebtForm();
        }

        /// <summary>
        /// Создаёт форму редактирования долга.
        /// </summary>
        /// <param name="debtId">ID долга, который будет редактироваться.</param>
        /// <returns>Экземпляр формы, реализующий IEditDebtView.</returns>
        public IEditDebtView CreateEditDebtView(int debtId)
        {
            return new EditDebtForm(debtId);
        }
    }
}
