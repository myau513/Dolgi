using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DebtTracker.Shared;
using DebtTracker.BusinessLogic;

namespace DebtTracker.Presenter
{
    public class MainPresenter
    {
        private readonly IMainView _view;
        private readonly IDebtService _model;

        public MainPresenter(IMainView view, IDebtService model)
        {
            _view = view;
            _model = model;

            _view.AddRequested += OnAdd;
            _view.EditRequested += OnEdit;
            _view.DeleteRequested += OnDelete;
            _view.RefreshRequested += LoadDebts;

            LoadDebts();
            LoadTomorrow();
        }

        private void LoadDebts()
        {
            try
            {
                var debts = _model.GetAllDebtsSorted()
                    .Select(d => new DebtDto
                    {
                        Id = d.Id,
                        Subject = d.Subject,
                        Description = d.Description,
                        Deadline = d.Deadline,
                        Status = d.Status.ToString()
                    });

                _view.ShowDebts(debts);
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void LoadTomorrow()
        {
            var debts = _model.GetDebtsWithTomorrowDeadline()
                .Select(d => new DebtDto
                {
                    Subject = d.Subject,
                    Description = d.Description,
                    Deadline = d.Deadline
                });

            _view.ShowTomorrowWarning(debts);
        }

        private void OnAdd() { /* открыть AddForm */ }
        private void OnEdit() { /* открыть EditForm */ }
        private void OnDelete()
        {
            if (_view.SelectedDebtId == null) return;

            try
            {
                _model.DeleteDebt(_view.SelectedDebtId.Value);
                LoadDebts();
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }
    }


}
