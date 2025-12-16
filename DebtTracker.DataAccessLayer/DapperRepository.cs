using Dapper;
using DebtTracker.DataAccessLayer.Exceptions;
using DebtTracker.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace DebtTracker.DataAccessLayer
{
    /// <summary>
    /// Реализация репозитория для работы с базой данных с использованием микро-ORM Dapper.
    /// Реализует интерфейс <see cref="IRepository{T}"/> для обобщенного типа T, который должен наследоваться от IDomainObject.
    /// </summary>
    /// <typeparam name="T">Тип сущности, с которой работает репозиторий (должен наследоваться от IDomainObject).</typeparam>
    /// <remarks>
    /// Ключевые особенности:
    /// <list type="bullet">
    /// <item>Использует Dapper для высокопроизводительного доступа к данным</item>
    /// <item>Автоматическое управление подключениями через фабрику</item>
    /// <item>Полная обработка исключений с преобразованием в специализированные исключения DAL</item>
    /// <item>Поддержка специфичных для долгов операций</item>
    /// </list>
    /// </remarks>
    public class DapperRepository<T> : IRepository<T> where T : class, IDomainObject
    {
        private readonly IDbConnectionFactory _connectionFactory;

        /// <summary>
        /// Инициализирует новый экземпляр DapperRepository с указанной фабрикой подключений.
        /// </summary>
        /// <param name="connectionFactory">Фабрика для создания подключений к базе данных.</param>
        /// <exception cref="ArgumentNullException">Если connectionFactory равен null.</exception>
        public DapperRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
        }

        /// <summary>
        /// Создает новую запись в базе данных.
        /// </summary>
        /// <param name="item">Объект сущности для создания.</param>
        /// <exception cref="ArgumentNullException">Если item равен null.</exception>
        /// <exception cref="UniqueConstraintException">При нарушении ограничения уникальности.</exception>
        /// <exception cref="ForeignKeyConstraintException">При нарушении ссылочной целостности.</exception>
        /// <exception cref="DataAccessException">При других ошибках доступа к данным.</exception>
        public void Create(T item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            try
            {
                using (var connection = _connectionFactory.CreateConnection())
                {
                    var sqlQuery = @"INSERT INTO Debts (Subject, Description, Status, Deadline) 
                                   VALUES (@Subject, @Description, @Status, @Deadline);
                                   SELECT CAST(SCOPE_IDENTITY() as int)";

                    var id = connection.Query<int>(sqlQuery, item).FirstOrDefault();

                    if (id <= 0)
                        throw new DataAccessException("Не удалось создать запись в базе данных");

                    var property = typeof(T).GetProperty("Id");
                    if (property != null && property.CanWrite)
                    {
                        property.SetValue(item, id);
                    }
                }
            }
            catch (System.Data.SqlClient.SqlException sqlEx) when (sqlEx.Number == 2627) // Нарушение уникальности
            {
                throw new UniqueConstraintException("Нарушение уникальности: запись с такими данными уже существует", sqlEx);
            }
            catch (System.Data.SqlClient.SqlException sqlEx) when (sqlEx.Number == 547) // Нарушение внешнего ключа
            {
                throw new ForeignKeyConstraintException("Нарушение ссылочной целостности", sqlEx);
            }
            catch (Exception ex)
            {
                throw new DataAccessException($"Ошибка при создании записи: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Возвращает все записи из таблицы Debts, отсортированные по дате дедлайна.
        /// </summary>
        /// <returns>Коллекция всех сущностей из базы данных.</returns>
        /// <exception cref="DataAccessException">При ошибках доступа к данным.</exception>
        public IEnumerable<T> GetAll()
        {
            try
            {
                using (var connection = _connectionFactory.CreateConnection())
                {
                    return connection.Query<T>("SELECT * FROM Debts ORDER BY Deadline").ToList();
                }
            }
            catch (Exception ex)
            {
                throw new DataAccessException($"Ошибка при получении всех записей: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Возвращает сущность по указанному идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор искомой сущности.</param>
        /// <returns>Сущность с указанным идентификатором.</returns>
        /// <exception cref="ArgumentException">Если id меньше или равен 0.</exception>
        /// <exception cref="EntityNotFoundException">Если сущность с указанным ID не найдена.</exception>
        /// <exception cref="DataAccessException">При других ошибках доступа к данным.</exception>
        public T GetById(int id)
        {
            if (id <= 0)
                throw new ArgumentException("ID должен быть положительным числом", nameof(id));

            try
            {
                using (var connection = _connectionFactory.CreateConnection())
                {
                    var result = connection.Query<T>("SELECT * FROM Debts WHERE Id = @Id", new { Id = id }).FirstOrDefault();

                    if (result == null)
                        throw new EntityNotFoundException($"Запись с ID {id} не найдена");

                    return result;
                }
            }
            catch (EntityNotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new DataAccessException($"Ошибка при получении записи с ID {id}: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Обновляет существующую запись в базе данных.
        /// </summary>
        /// <param name="item">Объект сущности с обновленными данными.</param>
        /// <exception cref="ArgumentNullException">Если item равен null.</exception>
        /// <exception cref="EntityNotFoundException">Если сущность с указанным ID не найдена.</exception>
        /// <exception cref="UniqueConstraintException">При нарушении ограничения уникальности.</exception>
        /// <exception cref="ForeignKeyConstraintException">При нарушении ссылочной целостности.</exception>
        /// <exception cref="DataAccessException">При других ошибках доступа к данным.</exception>
        public void Update(T item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            try
            {
                using (var connection = _connectionFactory.CreateConnection())
                {
                    var sqlQuery = @"UPDATE Debts 
                                   SET Subject = @Subject, 
                                       Description = @Description, 
                                       Status = @Status, 
                                       Deadline = @Deadline 
                                   WHERE Id = @Id";

                    int affectedRows = connection.Execute(sqlQuery, item);

                    if (affectedRows == 0)
                        throw new EntityNotFoundException($"Запись с ID {item.Id} не найдена для обновления");
                }
            }
            catch (System.Data.SqlClient.SqlException sqlEx) when (sqlEx.Number == 2627)
            {
                throw new UniqueConstraintException("Нарушение уникальности при обновлении", sqlEx);
            }
            catch (System.Data.SqlClient.SqlException sqlEx) when (sqlEx.Number == 547)
            {
                throw new ForeignKeyConstraintException("Нарушение ссылочной целостности при обновлении", sqlEx);
            }
            catch (EntityNotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new DataAccessException($"Ошибка при обновлении записи с ID {item.Id}: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Удаляет запись по указанному идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор удаляемой сущности.</param>
        /// <exception cref="ArgumentException">Если id меньше или равен 0.</exception>
        /// <exception cref="EntityNotFoundException">Если сущность с указанным ID не найдена.</exception>
        /// <exception cref="ForeignKeyConstraintException">При нарушении ссылочной целостности (невозможно удалить).</exception>
        /// <exception cref="DataAccessException">При других ошибках доступа к данным.</exception>
        public void Delete(int id)
        {
            if (id <= 0)
                throw new ArgumentException("ID должен быть положительным числом", nameof(id));

            try
            {
                using (var connection = _connectionFactory.CreateConnection())
                {
                    var sqlQuery = "DELETE FROM Debts WHERE Id = @Id";
                    int affectedRows = connection.Execute(sqlQuery, new { Id = id });

                    if (affectedRows == 0)
                        throw new EntityNotFoundException($"Запись с ID {id} не найдена для удаления");
                }
            }
            catch (System.Data.SqlClient.SqlException sqlEx) when (sqlEx.Number == 547)
            {
                throw new ForeignKeyConstraintException("Невозможно удалить запись: существуют связанные данные", sqlEx);
            }
            catch (EntityNotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new DataAccessException($"Ошибка при удалении записи с ID {id}: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Возвращает долги с дедлайном на завтра.
        /// </summary>
        /// <returns>Коллекция долгов с завтрашним дедлайном, отсортированная по времени.</returns>
        /// <exception cref="DataAccessException">При ошибках доступа к данным.</exception>
        public IEnumerable<T> GetDebtsWithDeadlineTomorrow()
        {
            try
            {
                using (var connection = _connectionFactory.CreateConnection())
                {
                    var tomorrow = DateTime.Today.AddDays(1);
                    return connection.Query<T>(
                        "SELECT * FROM Debts WHERE CONVERT(date, Deadline) = @Tomorrow ORDER BY Deadline",
                        new { Tomorrow = tomorrow.Date }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new DataAccessException($"Ошибка при получении долгов с завтрашним дедлайном: {ex.Message}", ex);
            }
        }
    }
}