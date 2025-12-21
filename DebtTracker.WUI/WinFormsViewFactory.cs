using DebtTracker._Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DebtTracker.WUI
{
    public class WinFormsViewFactory : IViewFactory
    {
        public IAddDebtView CreateAddDebtView()
            => new AddDebtForm();

        public IEditDebtView CreateEditDebtView(int debtId)
            => new EditDebtForm(debtId);
    }

}
