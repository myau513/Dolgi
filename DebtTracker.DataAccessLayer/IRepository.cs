using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DebtTracker.Entities;

namespace DebtTracker.DataAccessLayer
{
    public interface IRepository<T> where T : class, IDomainObject
    {
        IEnumerable<T> GetAll();        // Получение всех объектов
        T GetById(int id);              // Получение объекта по id
        void Create(T item);            // Создание объекта
        void Update(T item);            // Обновление объекта
        void Delete(int id);            // Удаление объекта по id
        void Save();                    // Сохранение изменений
    }
}
