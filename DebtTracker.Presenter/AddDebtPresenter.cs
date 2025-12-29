using System;
using DebtTracker.BusinessLogic;
using DebtTracker.Dto;
using DebtTracker.Presentation.Contracts;

namespace DebtTracker.Presenter
{
    public class AddDebtPresenter
    {
        private readonly IAddDebtView _view;
        private readonly IDebtModel _model;

        public AddDebtPresenter(IAddDebtView view, IDebtModel model)
        {
            _view = view ?? throw new ArgumentNullException(nameof(view));
            _model = model ?? throw new ArgumentNullException(nameof(model));

            _view.SaveRequested += OnSaveRequested;
            _view.CancelRequested += OnCancelRequested;
        }

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

        private void OnCancelRequested()
        {
            _view.Close();
        }
    }
}
