using DebtTracker._Shared;
using DebtTracker.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DebtTracker.BusinessLogic
{
    public interface IDebtModel
    {
        event Action<IEnumerable<DebtDto>> DebtsLoaded;
        event Action<string> ErrorOccurred;

        void LoadDebts();
        void AddDebt(DebtDto debt);
        void UpdateDebt(DebtDto debt);
        void DeleteDebt(int debtId);

        DebtDto GetById(int id);
    }
}

