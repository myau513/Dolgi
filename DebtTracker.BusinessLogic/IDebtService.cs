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
        void AddDebt(Debt debt);

        // READ
        List<Debt> GetAllDebtsSorted();
        Debt GetDebtById(int id);
        List<Debt> GetDebtsWithTomorrowDeadline();
        bool HasDebts();

        // UPDATE
        void UpdateDebt(Debt debt);

        // DELETE
        void DeleteDebt(int id);

        // Дополнительные методы
        int GetSortedPosition(Debt debt);

        // Опционально: проверка существования
        void EnsureDebtExists(int id);
    }
}