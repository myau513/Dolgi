using System;
using System.Collections.Generic;
using System.Linq;
using DebtTracker.Entities;

namespace DebtTracker.BusinessLogic
{
    public class DebtService
    {
        private List<Debt> _debts = new List<Debt>();

        // Добавить долг
        public bool AddDebt(Debt debt)
        {
            if (DebtValidator.ValidateDebt(debt))
            {
                _debts.Add(debt);
                return true;
            }
            return false;
        }

        // Получить все долги, отсортированные по дедлайну
        public List<Debt> GetAllDebtsSorted()
        {
            return _debts.OrderBy(d => d.Deadline).ToList();
        }

        // Получить долги с завтрашним дедлайном
        public List<Debt> GetDebtsWithTomorrowDeadline()
        {
            DateTime tomorrow = DateTime.Today.AddDays(1);
            return _debts
                .Where(d => d.Deadline.Date == tomorrow)
                .OrderBy(d => d.Deadline)
                .ToList();
        }

        // Удалить долг по индексу в отсортированном списке
        public bool RemoveDebtBySortedIndex(int index)
        {
            var sortedDebts = GetAllDebtsSorted();

            if (index < 0 || index >= sortedDebts.Count)
                return false;

            var debtToRemove = sortedDebts[index];
            return _debts.Remove(debtToRemove);
        }

        // Получить долг по индексу в отсортированном списке
        public Debt GetDebtBySortedIndex(int index)
        {
            var sortedDebts = GetAllDebtsSorted();

            if (index < 0 || index >= sortedDebts.Count)
                return null;

            return sortedDebts[index];
        }

        // Обновить долг (заменяем старый на новый)
        public bool UpdateDebt(Debt oldDebt, Debt newDebt)
        {
            if (!DebtValidator.ValidateDebt(newDebt))
                return false;

            int index = _debts.IndexOf(oldDebt);
            if (index == -1)
                return false;

            _debts[index] = newDebt;
            return true;
        }

        // Получить позицию долга в отсортированном списке
        public int GetSortedPosition(Debt debt)
        {
            var sortedDebts = GetAllDebtsSorted();
            return sortedDebts.IndexOf(debt);
        }

        // Проверить, есть ли долги
        public bool HasDebts()
        {
            return _debts.Any();
        }
    }
}