using System;
using DebtTracker.BusinessLogic;
using DebtTracker.Dto;
using DebtTracker.Presentation.Contracts;

namespace DebtTracker.Presenter
{
    /// <summary>
    /// Presenter для экрана добавления долга.
    /// Связывает View (форму/консоль) и Model (бизнес-логику),
    /// обрабатывает действия пользователя и не позволяет View
    /// напрямую работать с моделью или базой данных.
    /// </summary>
    public class AddDebtPresenter
    {
        private readonly IAddDebtView _view;
        private readonly IDebtModel _model;

        /// <summary>
        /// Создает presenter и подписывается на события View.
        /// </summary>
        /// <param name="view">Экран добавления долга.</param>
        /// <param name="model">Бизнес-модель для работы с долгами.</param>
        /// <exception cref="ArgumentNullException">
        /// Бросается, если view или model не переданы.
        /// </exception>
        public AddDebtPresenter(IAddDebtView view, IDebtModel model)
        {
            _view = view ?? throw new ArgumentNullException(nameof(view));
            _model = model ?? throw new ArgumentNullException(nameof(model));

            // Подписываемся на события от View
            _view.SaveRequested += OnSaveRequested;
            _view.CancelRequested += OnCancelRequested;
        }

        /// <summary>
        /// Обрабатывает нажатие "Сохранить".
        /// Собирает данные из View, создает DTO
        /// и передает его в модель для сохранения.
        /// </summary>
        private void OnSaveRequested()
        {
            var dto = new DebtDto
            {
                Subject = _view.Subject,
                Description = _view.Description,
                Status = _view.Status,
                Deadline = _view.Deadline
            };

            _model.AddDebt(dto);
            _view.Close();
        }

        /// <summary>
        /// Обрабатывает нажатие "Отмена".
        /// Просто закрывает View, не вызывая модель.
        /// </summary>
        private void OnCancelRequested()
        {
            _view.Close();
        }
    }
}
