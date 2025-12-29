using System;
using System.Windows.Forms;
using DebtTracker.BusinessLogic;
using DebtTracker.Dto;
using DebtTracker.Presentation.Contracts;

namespace DebtTracker.Presenter
{
    /// <summary>
    /// Главный Presenter приложения.
    /// Управляет основным экраном списка долгов, реагирует на действия пользователя
    /// и координирует работу Model и различных View (WinForms/консоль).
    /// </summary>
    public class MainDebtPresenter
    {
        private readonly IMainDebtView _view;
        private readonly IDebtModel _model;
        private readonly IViewFactory _viewFactory;

        /// <summary>
        /// Создает главный Presenter и подписывается на события View и Model.
        /// </summary>
        /// <param name="view">Главное представление (список долгов).</param>
        /// <param name="model">Модель с бизнес-логикой работы с долгами.</param>
        /// <param name="viewFactory">Фабрика для создания экранов добавления/редактирования.</param>
        /// <exception cref="ArgumentNullException">
        /// Бросается, если одно из переданных значений равно null.
        /// </exception>
        public MainDebtPresenter(IMainDebtView view, IDebtModel model, IViewFactory viewFactory)
        {
            _view = view ?? throw new ArgumentNullException(nameof(view));
            _model = model ?? throw new ArgumentNullException(nameof(model));
            _viewFactory = viewFactory ?? throw new ArgumentNullException(nameof(viewFactory));

            _view.LoadView += OnLoadView;
            _view.AddRequested += OnAddRequested;
            _view.EditRequested += OnEditRequested;
            _view.DeleteRequested += OnDeleteRequested;
            _view.RefreshRequested += OnRefreshRequested;
            _view.TomorrowDebtsRequested += OnTomorrowDebtsRequested;

            _model.DebtsLoaded += OnDebtsLoaded;
            _model.TomorrowDebtsLoaded += OnTomorrowDebtsLoaded;
            _model.ErrorOccurred += OnErrorOccurred;
        }

        /// <summary>
        /// Запускает главный интерфейс приложения.
        /// В зависимости от типа View запускает либо WinForms-цикл, либо консольный цикл.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// Бросается, если тип View не поддерживается Presenter'ом.
        /// </exception>
        public void Run()
        {
            if (_view is Form form)
            {
                Application.Run(form);
            }
            else if (_view is IConsoleMainDebtView consoleView)
            {
                consoleView.RunLoop();
            }
            else
            {
                throw new InvalidOperationException("Неизвестный тип View для MainDebtPresenter");
            }
        }

        /// <summary>
        /// Обработка загрузки главного экрана.
        /// Загружает все долги и долги с дедлайном на завтра.
        /// </summary>
        private void OnLoadView()
        {
            _model.LoadDebts();
            _model.LoadTomorrowDebts();
        }

        /// <summary>
        /// Обработка команды "Обновить список".
        /// Повторно загружает все долги.
        /// </summary>
        private void OnRefreshRequested()
        {
            _model.LoadDebts();
        }

        /// <summary>
        /// Обработка команды "Показать долги на завтра".
        /// Запрашивает у модели список завтрашних долгов.
        /// </summary>
        private void OnTomorrowDebtsRequested()
        {
            _model.LoadTomorrowDebts();
        }

        /// <summary>
        /// Обработка команды "Добавить долг".
        /// Создает View и Presenter для добавления долга и показывает экран.
        /// </summary>
        private void OnAddRequested()
        {
            var addView = _viewFactory.CreateAddDebtView();
            var addPresenter = new AddDebtPresenter(addView, _model);
            addView.Show();
        }

        /// <summary>
        /// Обработка команды "Редактировать долг".
        /// Проверяет, выбран ли долг, создает экран редактирования и соответствующий Presenter.
        /// </summary>
        private void OnEditRequested()
        {
            if (_view.SelectedDebtId < 0)
            {
                _view.ShowError("Выберите долг для редактирования.");
                return;
            }

            var editView = _viewFactory.CreateEditDebtView(_view.SelectedDebtId);
            var editPresenter = new EditDebtPresenter(editView, _model);
            editView.Show();
        }

        /// <summary>
        /// Обработка команды "Удалить долг".
        /// Проверяет выбор и передает запрос на удаление модели.
        /// </summary>
        private void OnDeleteRequested()
        {
            if (_view.SelectedDebtId < 0)
            {
                _view.ShowError("Выберите долг для удаления.");
                return;
            }

            _model.DeleteDebt(_view.SelectedDebtId);
        }

        /// <summary>
        /// Обработка события модели о том, что список долгов загружен.
        /// Передает данные в View для отображения.
        /// </summary>
        /// <param name="debts">Коллекция DTO с долгами.</param>
        private void OnDebtsLoaded(System.Collections.Generic.IEnumerable<DebtDto> debts)
        {
            _view.ShowDebts(debts);
        }

        /// <summary>
        /// Обработка события модели о загруженных долгах на завтра.
        /// Передает их в View для отображения предупреждения.
        /// </summary>
        /// <param name="debts">Коллекция DTO с завтрашними долгами.</param>
        private void OnTomorrowDebtsLoaded(System.Collections.Generic.IEnumerable<DebtDto> debts)
        {
            _view.ShowTomorrowWarning(debts);
        }

        /// <summary>
        /// Обработка ошибок, возникших в модели.
        /// Показывает сообщение об ошибке пользователю через View.
        /// </summary>
        /// <param name="message">Текст ошибки.</param>
        private void OnErrorOccurred(string message)
        {
            _view.ShowError(message);
        }
    }
}
