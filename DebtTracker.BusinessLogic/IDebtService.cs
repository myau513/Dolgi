using DebtTracker.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DebtTracker.BusinessLogic
{
    public interface IDebtService
    {
        // CREATE
        bool AddDebt(Debt debt);

        // READ
        List<Debt> GetAllDebtsSorted();
        Debt GetDebtById(int id);
        List<Debt> GetDebtsWithTomorrowDeadline();
        bool HasDebts();

        // UPDATE
        bool UpdateDebt(Debt debt);

        // DELETE
        bool DeleteDebt(int id);

        // Дополнительные методы
        int GetSortedPosition(Debt debt);
    }
}
