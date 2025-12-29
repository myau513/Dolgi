
using System;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;


namespace DebtTracker.Presenter
{
    public class MainDebtPresenter
    {
        private readonly IMainDebtView _view;
        private readonly IDebtModel _model;
        private readonly IViewFactory _viewFactory;

        public MainDebtPresenter(IMainDebtView view, IDebtModel model, IViewFactory viewFactory)
        {
            _view = view ?? throw new ArgumentNullException(nameof(view));
            _model = model ?? throw new ArgumentNullException(nameof(model));
            _viewFactory = viewFactory ?? throw new ArgumentNullException(nameof(viewFactory));

            // Подписки на события View
            _view.LoadView += OnLoadView;
            _view.AddRequested += OnAddRequested;
            _view.EditRequested += OnEditRequested;
            _view.DeleteRequested += OnDeleteRequested;
            _view.RefreshRequested += OnRefreshRequested;
            _view.TomorrowDebtsRequested += OnTomorrowDebtsRequested;

            // Подписки на события Model
            _model.DebtsLoaded += OnDebtsLoaded;
            _model.TomorrowDebtsLoaded += OnTomorrowDebtsLoaded;
            _model.ErrorOccurred += OnErrorOccurred;
        }

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

        private void OnLoadView()
        {
            _model.LoadDebts();
        }

        private void OnRefreshRequested()
        {
            _model.LoadDebts();
        }

        private void OnTomorrowDebtsRequested()
        {
            _model.LoadTomorrowDebts();
        }

        private void OnAddRequested()
        {
            var addView = _viewFactory.CreateAddDebtView();
            var addPresenter = new AddDebtPresenter(addView, _model);
            addView.Show();
        }

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

        private void OnDeleteRequested()
        {
            if (_view.SelectedDebtId < 0)
            {
                _view.ShowError("Выберите долг для удаления.");
                return;
            }

            _model.DeleteDebt(_view.SelectedDebtId);
        }

        private void OnDebtsLoaded(System.Collections.Generic.IEnumerable<DebtDto> debts)
        {
            _view.ShowDebts(debts);
        }

        private void OnTomorrowDebtsLoaded(System.Collections.Generic.IEnumerable<DebtDto> debts)
        {
            _view.ShowTomorrowWarning(debts);
        }

        private void OnErrorOccurred(string message)
        {
            _view.ShowError(message);
        }
    }
}
