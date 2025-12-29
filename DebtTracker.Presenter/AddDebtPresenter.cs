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
    public class AddDebtPresenter
    {
        private readonly IAddDebtView _view;
        private readonly IDebtModel _model;

        public AddDebtPresenter(IAddDebtView view, IDebtModel model)
        {
            _view = view;
            _model = model;

            _view.SaveRequested += OnSave;
            _view.CancelRequested += _view.Close;
        }

        private void OnSave()
        {
            try
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
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }
    }
}
