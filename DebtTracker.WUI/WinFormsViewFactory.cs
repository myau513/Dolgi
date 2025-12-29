using DebtTracker.Presentation.Contracts;

namespace DebtTracker.WUI
{
    public class WinFormsViewFactory : IViewFactory
    {
        public IAddDebtView CreateAddDebtView()
        {
            return new AddDebtForm();
        }

        public IEditDebtView CreateEditDebtView(int debtId)
        {
            return new EditDebtForm(debtId);
        }
    }
}
