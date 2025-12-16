using DebtTracker.Entities;
using System.Collections.Generic;

namespace DebtTracker.BusinessLogic
{
    /// <summary>
    /// Интерфейс сервиса для управления бизнес-логикой операций с долгами.
    /// Определяет контракт для всех операций CRUD и дополнительных бизнес-функций.
    /// </summary>
    public interface IDebtService
    {
        /// <summary>
        /// Добавляет новый долг в систему.
        /// </summary>
        /// <param name="debt">Объект долга для добавления.</param>
        /// <exception cref="System.ArgumentNullException">Если debt равен null.</exception>
        /// <exception cref="DebtValidationException">Если данные долга не проходят валидацию.</exception>
        /// <exception cref="DebtOperationException">Если возникает ошибка при сохранении.</exception>
        void AddDebt(Debt debt);

        /// <summary>
        /// Возвращает все долги, отсортированные по дате дедлайна (от ближайших к дальним).
        /// </summary>
        /// <returns>Список долгов, отсортированных по дедлайну.</returns>
        /// <exception cref="DebtOperationException">Если возникает ошибка при получении данных.</exception>
        List<Debt> GetAllDebtsSorted();

        /// <summary>
        /// Возвращает долг по указанному идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор искомого долга.</param>
        /// <returns>Объект долга с указанным ID.</returns>
        /// <exception cref="System.ArgumentException">Если id меньше или равен 0.</exception>
        /// <exception cref="DebtNotFoundException">Если долг с указанным ID не найден.</exception>
        /// <exception cref="DebtOperationException">Если возникает ошибка при получении данных.</exception>
        Debt GetDebtById(int id);

        /// <summary>
        /// Возвращает долги, у которых дедлайн наступает завтра.
        /// </summary>
        /// <returns>Список долгов с завтрашним дедлайном, отсортированных по времени.</returns>
        /// <exception cref="DebtOperationException">Если возникает ошибка при получении данных.</exception>
        List<Debt> GetDebtsWithTomorrowDeadline();

        /// <summary>
        /// Проверяет, есть ли в системе хотя бы один долг.
        /// </summary>
        /// <returns>true, если в системе есть долги; иначе false.</returns>
        /// <exception cref="DebtOperationException">Если возникает ошибка при проверке.</exception>
        bool HasDebts();

        /// <summary>
        /// Обновляет существующий долг.
        /// </summary>
        /// <param name="debt">Объект долга с обновленными данными.</param>
        /// <exception cref="System.ArgumentNullException">Если debt равен null.</exception>
        /// <exception cref="DebtValidationException">Если данные долга не проходят валидацию.</exception>
        /// <exception cref="DebtNotFoundException">Если долг с указанным ID не найден.</exception>
        /// <exception cref="DebtOperationException">Если возникает ошибка при обновлении.</exception>
        void UpdateDebt(Debt debt);

        /// <summary>
        /// Удаляет долг по указанному идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор удаляемого долга.</param>
        /// <exception cref="System.ArgumentException">Если id меньше или равен 0.</exception>
        /// <exception cref="DebtNotFoundException">Если долг с указанным ID не найден.</exception>
        /// <exception cref="DebtOperationException">Если возникает ошибка при удалении.</exception>
        void DeleteDebt(int id);

        /// <summary>
        /// Возвращает позицию долга в отсортированном по дедлайну списке.
        /// </summary>
        /// <param name="debt">Объект долга для поиска позиции.</param>
        /// <returns>Индекс позиции долга в отсортированном списке (начиная с 0).</returns>
        /// <exception cref="System.ArgumentNullException">Если debt равен null.</exception>
        /// <exception cref="DebtNotFoundException">Если долг не найден в отсортированном списке.</exception>
        /// <exception cref="DebtOperationException">Если возникает ошибка при получении данных.</exception>
        int GetSortedPosition(Debt debt);

        /// <summary>
        /// Проверяет существование долга с указанным идентификатором.
        /// </summary>
        /// <param name="id">Идентификатор проверяемого долга.</param>
        /// <exception cref="System.ArgumentException">Если id меньше или равен 0.</exception>
        /// <exception cref="DebtNotFoundException">Если долг с указанным ID не существует.</exception>
        /// <exception cref="DebtOperationException">Если возникает ошибка при проверке.</exception>
        void EnsureDebtExists(int id);
    }
}