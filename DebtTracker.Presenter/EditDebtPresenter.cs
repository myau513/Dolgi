using System;
using DebtTracker.BusinessLogic;
using DebtTracker.Dto;
using DebtTracker.Presentation.Contracts;

namespace DebtTracker.Presenter
{
    /// <summary>
    /// Presenter для экрана редактирования долга.
    /// Управляет загрузкой данных в форму и сохранением изменений.
    /// </summary>
    public class EditDebtPresenter
    {
        private readonly IEditDebtView _view;
        private readonly IDebtModel _model;

        /// <summary>
        /// Создает presenter и подписывает его на события View.
        /// </summary>
        /// <param name="view">Экран редактирования долга.</param>
        /// <param name="model">Модель (бизнес-логика) работы с долгами.</param>
        public EditDebtPresenter(IEditDebtView view, IDebtModel model)
        {
            _view = view ?? throw new ArgumentNullException(nameof(view));
            _model = model ?? throw new ArgumentNullException(nameof(model));

            _view.LoadView += OnLoadView;
            _view.SaveRequested += OnSaveRequested;
            _view.CancelRequested += OnCancelRequested;
        }

        /// <summary>
        /// Обработка загрузки формы.
        /// Получает данные долга из модели и заполняет View.
        /// </summary>
        private void OnLoadView()
        {
            var dto = _model.GetById(_view.DebtId);

            _view.Subject = dto.Subject;
            _view.Description = dto.Description;
            _view.Status = dto.Status;
            _view.Deadline = dto.Deadline;
        }

        /// <summary>
        /// Обработка сохранения изменений.
        /// Собирает данные из View и передает их модели на обновление.
        /// </summary>
        private void OnSaveRequested()
        {
            var dto = new DebtDto
            {
                Id = _view.DebtId,
                Subject = _view.Subject,
                Description = _view.Description,
                Status = _view.Status,
                Deadline = _view.Deadline
            };

            _model.UpdateDebt(dto);
            _view.Close();
        }

        /// <summary>
        /// Обработка отмены редактирования.
        /// Просто закрывает View без обращения к модели.
        /// </summary>
        private void OnCancelRequested()
        {
            _view.Close();
        }
    }
}
