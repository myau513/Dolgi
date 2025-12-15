using System;
using System.Collections.Generic;
using System.Linq;
using DebtTracker.DataAccessLayer;
using DebtTracker.Entities;

namespace DebtTracker.BusinessLogic
{
    public class DebtService
    {
        private IRepository<Debt> _repository;

        // Можно переключаться между реализациями
        public DebtService(bool useDapper = false)
        {
            if (useDapper)
            {
                _repository = new DapperRepository<Debt>();
            }
            else
            {
                _repository = new EntityFrameworkRepository<Debt>();
            }
        }

        // Добавить долг
        public bool AddDebt(Debt debt)
        {
            if (DebtValidator.ValidateDebt(debt))
            {
                try
                {
                    _repository.Create(debt);
                    return true;
                }
                catch
                {
                    return false;
                }
            }
            return false;
        }

        // Получить все долги, отсортированные по дедлайну
        public List<Debt> GetAllDebtsSorted()
        {
            return _repository.GetAll()
                .OrderBy(d => d.Deadline)
                .ToList();
        }

        // Получить долги с завтрашним дедлайном
        public List<Debt> GetDebtsWithTomorrowDeadline()
        {
            DateTime tomorrow = DateTime.Today.AddDays(1);
            return _repository.GetAll()
                .Where(d => d.Deadline.Date == tomorrow)
                .OrderBy(d => d.Deadline)
                .ToList();
        }

        // Удалить долг по ID
        public bool RemoveDebtById(int id)
        {
            try
            {
                _repository.Delete(id);
                return true;
            }
            catch
            {
                return false;
            }
        }

        // Получить долг по ID
        public Debt GetDebtById(int id)
        {
            return _repository.GetById(id);
        }

        // Обновить долг
        public bool UpdateDebt(Debt debt)
        {
            if (!DebtValidator.ValidateDebt(debt))
                return false;

            try
            {
                _repository.Update(debt);
                return true;
            }
            catch
            {
                return false;
            }
        }

        // Получить позицию долга в отсортированном списке
        public int GetSortedPosition(Debt debt)
        {
            var sortedDebts = GetAllDebtsSorted();
            return sortedDebts.FindIndex(d => d.Id == debt.Id);
        }

        // Проверить, есть ли долги
        public bool HasDebts()
        {
            return _repository.GetAll().Any();
        }
    }
}