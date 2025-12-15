using DebtTracker.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace DebtTracker.DataAccessLayer
{
    public class EntityFrameworkRepository<T> : IRepository<T> where T : class, IDomainObject
    {
        private readonly DbContext _context;

        public EntityFrameworkRepository(DbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        // CREATE
        public void Create(T item)
        {
            _context.Set<T>().Add(item);
            _context.SaveChanges();  // Сохраняем сразу, Save() метод удалён
        }

        // READ
        public IEnumerable<T> GetAll()
        {
            return _context.Set<T>().ToList();
        }

        public T GetById(int id)
        {
            return _context.Set<T>().Find(id);
        }

        // UPDATE
        public void Update(T item)
        {
            _context.Entry(item).State = EntityState.Modified;
            _context.SaveChanges();  // Сохраняем сразу
        }

        // DELETE  
        public void Delete(int id)
        {
            var item = GetById(id);
            if (item != null)
            {
                _context.Set<T>().Remove(item);
                _context.SaveChanges();  // Сохраняем сразу
            }
        }
    }
}