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

        /// <summary>
        /// Инициализирует новый экземпляр DebtService с указанными зависимостями.
        /// </summary>
        /// <param name="repository">Репозиторий для доступа к данным долгов.</param>
        /// <param name="validator">Валидатор для проверки данных долгов.</param>
        /// <exception cref="ArgumentNullException">Если repository или validator равны null.</exception>
        
        public DebtService(IRepository<Debt> repository, IDebtValidator validator)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _validator = validator ?? throw new ArgumentNullException(nameof(validator));
        }

        /// <summary>
        /// Добавляет новый долг в систему после валидации данных.
        /// </summary>
        /// <param name="debt">Объект долга для добавления.</param>
        /// <exception cref="ArgumentNullException">Если debt равен null.</exception>
        /// <exception cref="DebtValidationException">Если данные долга не проходят валидацию.</exception>
        /// <exception cref="DebtOperationException">Если возникает ошибка при сохранении долга.</exception>
        public void AddDebt(Debt debt)
        {
            if (!_validator.ValidateDebt(debt))
                throw new Exceptions.DebtValidationException("Некорректные данные долга");

            try
            {
                _repository.Create(debt);
            }
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
        /// <summary>
        /// Возвращает все долги, отсортированные по дате дедлайна (от ближайших к дальним).
        /// </summary>
        /// <returns>Список долгов, отсортированных по дедлайну.</returns>
        /// <exception cref="DebtOperationException">Если возникает ошибка при получении данных.</exception>
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
        /// <summary>
        /// Возвращает долги, у которых дедлайн наступает завтра.
        /// </summary>
        /// <returns>Список долгов с завтрашним дедлайном, отсортированных по времени.</returns>
        /// <exception cref="DebtOperationException">Если возникает ошибка при получении данных.</exception>
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
        /// <summary>
        /// Удаляет долг по указанному идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор удаляемого долга.</param>
        /// <exception cref="ArgumentException">Если id меньше или равен 0.</exception>
        /// <exception cref="DebtNotFoundException">Если долг с указанным ID не найден.</exception>
        /// <exception cref="DebtOperationException">Если возникает ошибка при удалении.</exception>
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
        /// <summary>
        /// Возвращает долг по указанному идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор искомого долга.</param>
        /// <returns>Объект долга с указанным ID.</returns>
        /// <exception cref="ArgumentException">Если id меньше или равен 0.</exception>
        /// <exception cref="DebtNotFoundException">Если долг с указанным ID не найден.</exception>
        /// <exception cref="DebtOperationException">Если возникает ошибка при получении данных.</exception>
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
        /// <summary>
        /// Обновляет существующий долг после валидации данных.
        /// </summary>
        /// <param name="debt">Объект долга с обновленными данными.</param>
        /// <exception cref="ArgumentNullException">Если debt равен null.</exception>
        /// <exception cref="DebtValidationException">Если данные долга не проходят валидацию.</exception>
        /// <exception cref="DebtNotFoundException">Если долг с указанным ID не найден.</exception>
        /// <exception cref="DebtOperationException">Если возникает ошибка при обновлении.</exception>
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
        /// <summary>
        /// Возвращает позицию долга в отсортированном по дедлайну списке.
        /// </summary>
        /// <param name="debt">Объект долга для поиска позиции.</param>
        /// <returns>Индекс позиции долга в отсортированном списке (начиная с 0).</returns>
        /// <exception cref="ArgumentNullException">Если debt равен null.</exception>
        /// <exception cref="DebtNotFoundException">Если долг не найден в отсортированном списке.</exception>
        /// <exception cref="DebtOperationException">Если возникает ошибка при получении данных.</exception>
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
        /// <summary>
        /// Проверяет, есть ли в системе хотя бы один долг.
        /// </summary>
        /// <returns>true, если в системе есть долги; иначе false.</returns>
        /// <exception cref="DebtOperationException">Если возникает ошибка при проверке.</exception>
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
        /// <summary>
        /// Проверяет существование долга с указанным идентификатором.
        /// </summary>
        /// <param name="id">Идентификатор проверяемого долга.</param>
        /// <exception cref="ArgumentException">Если id меньше или равен 0.</exception>
        /// <exception cref="DebtNotFoundException">Если долг с указанным ID не существует.</exception>
        /// <exception cref="DebtOperationException">Если возникает ошибка при проверке.</exception>
        public void EnsureDebtExists(int id)
        {
            var debt = GetDebtById(id);
            if (debt == null)
                throw new Exceptions.DebtNotFoundException($"Долг с ID {id} не существует");
        }
    }
}