using DebtTracker.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;  // ← using для EF6
using System.Linq;

namespace DebtTracker.DataAccessLayer
{
    public class EntityFrameworkRepository<T> : IRepository<T> where T : class, IDomainObject
    {
        private readonly DebtContext _context;

        public EntityFrameworkRepository()
        {
            _context = new DebtContext();
        }

        public IEnumerable<T> GetAll()
        {
            return _context.Set<T>().ToList();
        }

        public T GetById(int id)
        {
            return _context.Set<T>().Find(id);
        }

        public void Create(T item)
        {
            _context.Set<T>().Add(item);
            Save();  // Сохраняем сразу
        }

        public void Update(T item)
        {
            // ИСПРАВЛЕНО: используйте полное имя
            _context.Entry(item).State = System.Data.Entity.EntityState.Modified;
            Save();
        }

        public void Delete(int id)
        {
            var item = GetById(id);
            if (item != null)
            {
                _context.Set<T>().Remove(item);
                Save();
            }
        }

        public void Save()
        {
            _context.SaveChanges();
        }

    }
}