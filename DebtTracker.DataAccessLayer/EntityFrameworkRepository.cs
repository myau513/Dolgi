using DebtTracker.DataAccessLayer.Exceptions;
using DebtTracker.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;

namespace DebtTracker.DataAccessLayer
{
    /// <summary>
    /// Реализация репозитория для работы с базой данных с использованием Entity Framework 6.
    /// Реализует интерфейс <see cref="IRepository{T}"/> для обобщенного типа T, который должен наследоваться от IDomainObject.
    /// </summary>
    /// <typeparam name="T">Тип сущности, с которой работает репозиторий (должен наследоваться от IDomainObject).</typeparam>
    /// <remarks>
    /// Ключевые особенности:
    /// <list type="bullet">
    /// <item>Использует Entity Framework 6 для ORM-операций</item>
    /// <item>Включает вызов SaveChanges() для сохранения изменений в БД</item>
    /// <item>Реализует безопасное обновление через отслеживание существующих сущностей</item>
    /// <item>Обрабатывает специфичные исключения БД с преобразованием в исключения DAL</item>
    /// </list>
    /// </remarks>
    public class EntityFrameworkRepository<T> : IRepository<T> where T : class, IDomainObject
    {
        private readonly DebtContext _context;

        /// <summary>
        /// Инициализирует новый экземпляр EntityFrameworkRepository с указанным контекстом базы данных.
        /// </summary>
        /// <param name="context">Контекст базы данных Entity Framework.</param>
        /// <exception cref="ArgumentNullException">Если context равен null.</exception>
        public EntityFrameworkRepository(DebtContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        /// <summary>
        /// Создает новую запись в базе данных и немедленно сохраняет изменения.
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
                _context.Set<T>().Add(item);
                _context.SaveChanges(); // Немедленное сохранение изменений в БД
            }
            catch (DbUpdateException dbEx) when (IsUniqueConstraintViolation(dbEx))
            {
                throw new UniqueConstraintException("Нарушение уникальности: запись с такими данными уже существует", dbEx);
            }
            catch (DbUpdateException dbEx) when (IsForeignKeyConstraintViolation(dbEx))
            {
                throw new ForeignKeyConstraintException("Нарушение ссылочной целостности", dbEx);
            }
            catch (Exception ex)
            {
                throw new DataAccessException($"Ошибка при создании записи: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Возвращает все записи из таблицы.
        /// </summary>
        /// <returns>Коллекция всех сущностей из базы данных.</returns>
        /// <exception cref="DataAccessException">При ошибках доступа к данным.</exception>
        public IEnumerable<T> GetAll()
        {
            try
            {
                return _context.Set<T>().ToList();
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
                var entity = _context.Set<T>().Find(id);

                if (entity == null)
                    throw new EntityNotFoundException($"Запись с ID {id} не найдена");

                return entity;
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
        /// Обновляет существующую запись в базе данных с безопасным подходом.
        /// </summary>
        /// <param name="item">Объект сущности с обновленными данными.</param>
        /// <exception cref="ArgumentNullException">Если item равен null.</exception>
        /// <exception cref="EntityNotFoundException">Если сущность с указанным ID не найдена.</exception>
        /// <exception cref="UniqueConstraintException">При нарушении ограничения уникальности.</exception>
        /// <exception cref="ForeignKeyConstraintException">При нарушении ссылочной целостности.</exception>
        /// <exception cref="DataAccessException">При других ошибках доступа к данным.</exception>
        /// <remarks>
        /// Использует безопасный подход обновления: сначала находит существующую сущность,
        /// затем обновляет ее значения, что предотвращает проблемы с отслеживанием сущностей в EF.
        /// </remarks>
        public void Update(T item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            try
            {
                // Безопасный способ обновления для EF: находим существующую сущность
                var existing = _context.Set<T>().Find(item.Id);
                if (existing == null)
                    throw new EntityNotFoundException($"Запись с ID {item.Id} не найдена для обновления");

                // Обновляем значения существующей сущности
                _context.Entry(existing).CurrentValues.SetValues(item);
                _context.SaveChanges(); // Немедленное сохранение изменений в БД
            }
            catch (DbUpdateException dbEx) when (IsUniqueConstraintViolation(dbEx))
            {
                throw new UniqueConstraintException("Нарушение уникальности при обновлении", dbEx);
            }
            catch (DbUpdateException dbEx) when (IsForeignKeyConstraintViolation(dbEx))
            {
                throw new ForeignKeyConstraintException("Нарушение ссылочной целостности при обновлении", dbEx);
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
        /// Удаляет запись по указанному идентификатору и немедленно сохраняет изменения.
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
                var entity = GetById(id);
                if (entity != null)
                {
                    _context.Set<T>().Remove(entity);
                    _context.SaveChanges(); // Немедленное сохранение изменений в БД
                }
            }
            catch (DbUpdateException dbEx) when (IsForeignKeyConstraintViolation(dbEx))
            {
                throw new ForeignKeyConstraintException("Невозможно удалить запись: существуют связанные данные", dbEx);
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
        /// Проверяет, является ли исключение DbUpdateException нарушением ограничения уникальности.
        /// </summary>
        /// <param name="ex">Исключение DbUpdateException для проверки.</param>
        /// <returns>true, если исключение вызвано нарушением ограничения уникальности; иначе false.</returns>
        /// <remarks>
        /// Определяет по коду SQL Server ошибки 2627 (нарушение UNIQUE KEY constraint).
        /// </remarks>
        private bool IsUniqueConstraintViolation(DbUpdateException ex)
        {
            var sqlException = ex.GetBaseException() as System.Data.SqlClient.SqlException;
            return sqlException?.Number == 2627; // Нарушение UNIQUE KEY constraint
        }

        /// <summary>
        /// Проверяет, является ли исключение DbUpdateException нарушением ограничения внешнего ключа.
        /// </summary>
        /// <param name="ex">Исключение DbUpdateException для проверки.</param>
        /// <returns>true, если исключение вызвано нарушением ограничения внешнего ключа; иначе false.</returns>
        /// <remarks>
        /// Определяет по коду SQL Server ошибки 547 (нарушение FOREIGN KEY constraint).
        /// </remarks>
        private bool IsForeignKeyConstraintViolation(DbUpdateException ex)
        {
            var sqlException = ex.GetBaseException() as System.Data.SqlClient.SqlException;
            return sqlException?.Number == 547; // Нарушение FOREIGN KEY constraint
        }
    }
}