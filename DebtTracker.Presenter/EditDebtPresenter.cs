using System;
using DebtTracker.BusinessLogic;
using DebtTracker.Dto;
using DebtTracker.Presentation.Contracts;

namespace DebtTracker.Presenter
{
    public class EditDebtPresenter
    {
        private readonly IEditDebtView _view;
        private readonly IDebtModel _model;

        public EditDebtPresenter(IEditDebtView view, IDebtModel model)
        {
            _view = view ?? throw new ArgumentNullException(nameof(view));
            _model = model ?? throw new ArgumentNullException(nameof(model));

            _view.LoadView += OnLoadView;
            _view.SaveRequested += OnSaveRequested;
            _view.CancelRequested += OnCancelRequested;
        }

        private void OnLoadView()
        {
            var dto = _model.GetById(_view.DebtId);
            _view.Subject = dto.Subject;
            _view.Description = dto.Description;
            _view.Status = dto.Status;
            _view.Deadline = dto.Deadline;
        }

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

        private void OnCancelRequested()
        {
            _view.Close();
        }
    }
}
