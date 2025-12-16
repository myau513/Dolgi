using Dapper;
using DebtTracker.DataAccessLayer.Exceptions;
using DebtTracker.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace DebtTracker.DataAccessLayer
{
    public class DapperRepository<T> : IRepository<T> where T : class, IDomainObject
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public DapperRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
        }

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

                    // Устанавливаем сгенерированный ID
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

        // Дополнительные методы для Debt-specific операций
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