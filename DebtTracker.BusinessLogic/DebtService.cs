using DebtTracker.DataAccessLayer;
using DebtTracker.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

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

        public void AddDebt(Debt debt)
        {
            if (!_validator.ValidateDebt(debt))
                throw new Exceptions.DebtValidationException("Некорректные данные долга");

            try
            {
                _repository.Create(debt);
            }
            catch (Exception ex)
            {
                throw new Exceptions.DebtOperationException($"Ошибка при создании долга: {ex.Message}", ex);
            }
        }

        // Получить все долги, отсортированные по дедлайну
        public List<Debt> GetAllDebtsSorted()
        {
            try
            {
                return _repository.GetAll()
                    .OrderBy(d => d.Deadline)
                    .ToList();
            }
            catch (Exception ex)
            {
                throw new Exceptions.DebtOperationException($"Ошибка при получении списка долгов: {ex.Message}", ex);
            }
        }

        // Получить долги с завтрашним дедлайном
        public List<Debt> GetDebtsWithTomorrowDeadline()
        {
            try
            {
                DateTime tomorrow = DateTime.Today.AddDays(1);
                return _repository.GetAll()
                    .Where(d => d.Deadline.Date == tomorrow)
                    .OrderBy(d => d.Deadline)
                    .ToList();
            }
            catch (Exception ex)
            {
                throw new Exceptions.DebtOperationException($"Ошибка при получении долгов с завтрашним дедлайном: {ex.Message}", ex);
            }
        }

        // Удалить долг по ID
        public void DeleteDebt(int id)
        {
            try
            {
                _repository.Delete(id);
            }
            catch (Exception ex)
            {
                throw new Exceptions.DebtOperationException($"Ошибка при удалении долга с ID {id}: {ex.Message}", ex);
            }
        }

        // Получить долг по ID
        public Debt GetDebtById(int id)
        {
            try
            {
                var debt = _repository.GetById(id);

                if (debt == null)
                    throw new Exceptions.DebtNotFoundException($"Долг с ID {id} не найден");

                return debt;
            }
            catch (Exceptions.DebtNotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exceptions.DebtOperationException($"Ошибка при получении долга с ID {id}: {ex.Message}", ex);
            }
        }

        // Обновить долг
        public void UpdateDebt(Debt debt)
        {
            if (!_validator.ValidateDebt(debt))
                throw new Exceptions.DebtValidationException("Некорректные данные для обновления долга");

            try
            {
                _repository.Update(debt);
            }
            catch (Exception ex)
            {
                throw new Exceptions.DebtOperationException($"Ошибка при обновлении долга с ID {debt.Id}: {ex.Message}", ex);
            }
        }

        // Получить позицию долга в отсортированном списке
        public int GetSortedPosition(Debt debt)
        {
            if (debt == null)
                throw new ArgumentNullException(nameof(debt), "Долг не может быть null");

            try
            {
                var sortedDebts = GetAllDebtsSorted();
                var position = sortedDebts.FindIndex(d => d.Id == debt.Id);

                if (position == -1)
                    throw new Exceptions.DebtNotFoundException($"Долг с ID {debt.Id} не найден в отсортированном списке");

                return position;
            }
            catch (Exceptions.DebtOperationException)
            {
                throw;
            }
        }

        // Проверить, есть ли долги
        public bool HasDebts()
        {
            try
            {
                return _repository.GetAll().Any();
            }
            catch (Exception ex)
            {
                throw new Exceptions.DebtOperationException($"Ошибка при проверке наличия долгов: {ex.Message}", ex);
            }
        }

        // Опционально: метод для массовой проверки существования долга
        public void EnsureDebtExists(int id)
        {
            var debt = GetDebtById(id);
            if (debt == null)
                throw new Exceptions.DebtNotFoundException($"Долг с ID {id} не существует");
        }
    }
}