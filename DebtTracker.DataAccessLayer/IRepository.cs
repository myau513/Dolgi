using System.Collections.Generic;
using DebtTracker.Entities;

namespace DebtTracker.DataAccessLayer
{
    /// <summary>
    /// Обобщенный интерфейс репозитория для операций CRUD (Создание, Чтение, Обновление, Удаление) с сущностями.
    /// Определяет базовый контракт для всех репозиториев в системе.
    /// </summary>
    /// <typeparam name="T">Тип сущности, должен реализовывать интерфейс <see cref="IDomainObject"/>.</typeparam>
    /// <remarks>
    /// Интерфейс реализует паттерн Репозиторий, который:
    /// <list type="bullet">
    /// <item>Инкапсулирует логику доступа к данным</item>
    /// <item>Предоставляет абстракцию над хранилищем данных</item>
    /// <item>Упрощает тестирование бизнес-логики</item>
    /// <item>Поддерживает принцип единственной ответственности</item>
    /// </list>
    /// Ограничение where T : IDomainObject гарантирует, что сущности имеют идентификатор (Id).
    /// </remarks>
    public interface IRepository<T> where T : IDomainObject
    {
        /// <summary>
        /// Создает новую сущность в хранилище данных.
        /// </summary>
        /// <param name="entity">Сущность для создания.</param>
        /// <exception cref="System.ArgumentNullException">Если entity равен null.</exception>
        /// <exception cref="DataAccessException">При ошибках доступа к данным.</exception>
        void Create(T entity);

        /// <summary>
        /// Возвращает все сущности из хранилища данных.
        /// </summary>
        /// <returns>Коллекция всех сущностей типа T.</returns>
        /// <exception cref="DataAccessException">При ошибках доступа к данным.</exception>
        IEnumerable<T> GetAll();

        /// <summary>
        /// Возвращает сущность по указанному идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор сущности.</param>
        /// <returns>Сущность с указанным идентификатором или null, если не найдена.</returns>
        /// <exception cref="System.ArgumentException">Если id меньше или равен 0.</exception>
        /// <exception cref="EntityNotFoundException">Если сущность с указанным ID не найдена.</exception>
        /// <exception cref="DataAccessException">При других ошибках доступа к данным.</exception>
        T GetById(int id);

        /// <summary>
        /// Обновляет существующую сущность в хранилище данных.
        /// </summary>
        /// <param name="entity">Сущность с обновленными данными.</param>
        /// <exception cref="System.ArgumentNullException">Если entity равен null.</exception>
        /// <exception cref="EntityNotFoundException">Если сущность с указанным ID не найдена.</exception>
        /// <exception cref="DataAccessException">При ошибках доступа к данным.</exception>
        void Update(T entity);

        /// <summary>
        /// Удаляет сущность по указанному идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор удаляемой сущности.</param>
        /// <exception cref="System.ArgumentException">Если id меньше или равен 0.</exception>
        /// <exception cref="EntityNotFoundException">Если сущность с указанным ID не найдена.</exception>
        /// <exception cref="DataAccessException">При ошибках доступа к данным.</exception>
        void Delete(int id);
    }
}