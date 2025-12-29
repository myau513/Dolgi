using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DebtTracker.BusinessLogic;
using DebtTracker.ConsoleApp;
using DebtTracker.Dto;
using DebtTracker.Presentation.Contracts;
using DebtTracker.WUI;

namespace DebtTracker.Presenter
{
    public class MainDebtPresenter
    {
        private readonly IMainDebtView _view;
        private readonly IDebtModel _model;
        private readonly IViewFactory _factory;

        public MainDebtPresenter(IMainDebtView view, IDebtModel model, IViewFactory factory)
        {
            _view = view;
            _model = model;
            _factory = factory;

            _view.LoadView += OnLoad;
            _view.AddRequested += OnAdd;
            _view.EditRequested += OnEdit;
            _view.DeleteRequested += OnDelete;
            _view.RefreshRequested += _model.LoadDebts;

            _model.DebtsLoaded += _view.ShowDebts;
            _model.ErrorOccurred += _view.ShowError;
        }

        private void OnLoad()
        {
            _model.LoadDebts();
        }

        private void OnEdit()
        {
            if (_view.SelectedDebtId < 0)
            {
                _view.ShowMessage("Выберите долг");
                return;
            }

            var view = _factory.CreateEditDebtView(_view.SelectedDebtId);
            new EditDebtPresenter(view, _model);
            view.ShowView();
        }

        private void OnDelete()
        {
            if (_view.SelectedDebtId < 0)
            {
                _view.ShowMessage("Выберите долг");
                return;
            }

            _model.DeleteDebt(_view.SelectedDebtId);
        }
        private void OnAdd()
        {
            var addView = _factory.CreateAddDebtView();
            new AddDebtPresenter(addView, _model);
            addView.ShowView();

        }

    }
}
