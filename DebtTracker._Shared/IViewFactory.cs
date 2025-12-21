using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DebtTracker._Shared
{
    //Она создаёт нужные View, а презентор просто говорит:«Дай мне View для добавления долга»
    public interface IViewFactory
    {
        IAddDebtView CreateAddDebtView();
        IEditDebtView CreateEditDebtView(int debtId);
    }

}
