using System;
using System.Collections.Generic;
using System.Linq;
using DebtTracker.DataAccessLayer;
using DebtTracker.Entities;
using DebtTracker.DataAccessLayer.Exceptions;

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
            // Используем просто имена классов, так как добавили using
            catch (EntityNotFoundException ex)
            {
                throw new Exceptions.DebtNotFoundException(ex.Message, ex);
            }
            catch (DataAccessException ex)
            {
                throw new Exceptions.DebtOperationException($"Ошибка при создании долга: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new Exceptions.DebtOperationException($"Неизвестная ошибка при создании долга: {ex.Message}", ex);
            }
        }

        public List<Debt> GetAllDebtsSorted()
        {
            try
            {
                return _repository.GetAll()
                    .OrderBy(d => d.Deadline)
                    .ToList();
            }
            catch (DataAccessException ex)
            {
                throw new Exceptions.DebtOperationException($"Ошибка при получении списка долгов: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new Exceptions.DebtOperationException($"Неизвестная ошибка при получении списка долгов: {ex.Message}", ex);
            }
        }

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
            catch (DataAccessException ex)
            {
                throw new Exceptions.DebtOperationException($"Ошибка при получении долгов с завтрашним дедлайном: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new Exceptions.DebtOperationException($"Неизвестная ошибка при получении долгов с завтрашним дедлайном: {ex.Message}", ex);
            }
        }

        public void DeleteDebt(int id)
        {
            try
            {
                _repository.Delete(id);
            }
            catch (EntityNotFoundException ex)
            {
                throw new Exceptions.DebtNotFoundException($"Долг с ID {id} не найден: {ex.Message}", ex);
            }
            catch (DataAccessException ex)
            {
                throw new Exceptions.DebtOperationException($"Ошибка при удалении долга с ID {id}: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new Exceptions.DebtOperationException($"Неизвестная ошибка при удалении долга: {ex.Message}", ex);
            }
        }

        public Debt GetDebtById(int id)
        {
            try
            {
                var debt = _repository.GetById(id);

                if (debt == null)
                    throw new Exceptions.DebtNotFoundException($"Долг с ID {id} не найден");

                return debt;
            }
            catch (EntityNotFoundException ex)
            {
                throw new Exceptions.DebtNotFoundException(ex.Message, ex);
            }
            catch (DataAccessException ex)
            {
                throw new Exceptions.DebtOperationException($"Ошибка при получении долга с ID {id}: {ex.Message}", ex);
            }
            catch (Exceptions.DebtNotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exceptions.DebtOperationException($"Неизвестная ошибка при получении долга: {ex.Message}", ex);
            }
        }

        public void UpdateDebt(Debt debt)
        {
            if (!_validator.ValidateDebt(debt))
                throw new Exceptions.DebtValidationException("Некорректные данные для обновления долга");

            try
            {
                _repository.Update(debt);
            }
            catch (EntityNotFoundException ex)
            {
                throw new Exceptions.DebtNotFoundException($"Долг с ID {debt.Id} не найден: {ex.Message}", ex);
            }
            catch (DataAccessException ex)
            {
                throw new Exceptions.DebtOperationException($"Ошибка при обновлении долга с ID {debt.Id}: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new Exceptions.DebtOperationException($"Неизвестная ошибка при обновлении долга: {ex.Message}", ex);
            }
        }

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

        public bool HasDebts()
        {
            try
            {
                return _repository.GetAll().Any();
            }
            catch (DataAccessException ex)
            {
                throw new Exceptions.DebtOperationException($"Ошибка при проверке наличия долгов: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new Exceptions.DebtOperationException($"Неизвестная ошибка при проверке наличия долгов: {ex.Message}", ex);
            }
        }

        public void EnsureDebtExists(int id)
        {
            var debt = GetDebtById(id);
            if (debt == null)
                throw new Exceptions.DebtNotFoundException($"Долг с ID {id} не существует");
        }
    }
}