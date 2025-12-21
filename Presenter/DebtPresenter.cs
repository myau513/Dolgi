using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DebtTracker._Shared;
using DebtTracker.BusinessLogic;
using DebtTracker.Entities;

namespace Presenter
{
    public class DebtPresenter
    {
        public DebtPresenter(IDebtView view, IDebtModel model)
        {
            view.LoadView += model.LoadDebts;
            view.AddRequested += () =>
            {
                model.AddDebt(new DebtDto
                {
                    Subject = view.Subject,
                    Description = view.Description,
                    Status = view.Status,
                    Deadline = view.Deadline
                });
            };

            model.DebtsLoaded += view.ShowDebts;
            model.ErrorOccurred += view.ShowError;
        }
    }


}
