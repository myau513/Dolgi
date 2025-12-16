using DebtTracker.DataAccessLayer.Exceptions;
using DebtTracker.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;

namespace DebtTracker.DataAccessLayer
{
    public class EntityFrameworkRepository<T> : IRepository<T> where T : class, IDomainObject
    {
        private readonly DebtContext _context;

        public EntityFrameworkRepository(DebtContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public void Create(T item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            try
            {
                _context.Set<T>().Add(item);
                _context.SaveChanges(); // ДОБАВИТЬ ЭТО!
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

        public void Update(T item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            try
            {
                // Безопасный способ обновления для EF
                var existing = _context.Set<T>().Find(item.Id);
                if (existing == null)
                    throw new EntityNotFoundException($"Запись с ID {item.Id} не найдена для обновления");

                _context.Entry(existing).CurrentValues.SetValues(item);
                _context.SaveChanges(); // ДОБАВИТЬ ЭТО!
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
                    _context.SaveChanges(); // ДОБАВИТЬ ЭТО!
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

        private bool IsUniqueConstraintViolation(DbUpdateException ex)
        {
            var sqlException = ex.GetBaseException() as System.Data.SqlClient.SqlException;
            return sqlException?.Number == 2627; // Нарушение UNIQUE KEY constraint
        }

        private bool IsForeignKeyConstraintViolation(DbUpdateException ex)
        {
            var sqlException = ex.GetBaseException() as System.Data.SqlClient.SqlException;
            return sqlException?.Number == 547; // Нарушение FOREIGN KEY constraint
        }
    }
}