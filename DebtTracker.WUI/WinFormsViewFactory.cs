using DebtTracker.Presentation.Contracts;

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
