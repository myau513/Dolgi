using System;
using System.Collections.Generic;
using System.Linq;
using DebtTracker.DataAccessLayer;
using DebtTracker.Entities;

namespace DebtTracker.BusinessLogic
{
    public class DebtService : IDebtService
    {
        private readonly IRepository<Debt> _repository;
        private readonly IDebtValidator _validator;

        public DebtService(IRepository<Debt> repository, IDebtValidator validator)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _validator = validator ?? throw new ArgumentNullException(nameof(validator));
        }

        public bool AddDebt(Debt debt)
        {
            if (!_validator.ValidateDebt(debt))
                return false;

            try
            {
                _repository.Create(debt);
                return true;
            }
            catch (Exception ex)
            {
                // Здесь можно добавить логирование в будущем
                Console.WriteLine($"Ошибка при создании долга: {ex.Message}");
                return false;
            }
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
        public bool DeleteDebt(int id)
        {
            try
            {
                _repository.Delete(id);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при удалении долга: {ex.Message}");
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
            if (!_validator.ValidateDebt(debt))
                return false;

            try
            {
                _repository.Update(debt);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при обновлении долга: {ex.Message}");
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