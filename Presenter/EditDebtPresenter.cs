using DebtTracker._Shared;
using DebtTracker.BusinessLogic;
using DebtTracker.Entities;
using System;

namespace Presenter
{
    public class EditDebtPresenter
    {
        private readonly IEditDebtView _view;
        private readonly IDebtModel _model;

        public EditDebtPresenter(IEditDebtView view, IDebtModel model)
        {
            _view = view;
            _model = model;

            _view.SaveRequested += OnSave;
            _view.CancelRequested += _view.Close;

            LoadDebt();
            _view.ShowView();
        }

        private void LoadDebt()
        {
            try
            {
                var debt = _model.GetById(_view.DebtId);
                _view.Fill(debt);
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
                _view.Close();
            }
        }

        private void OnSave()
        {
            try
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
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }
    }
}
