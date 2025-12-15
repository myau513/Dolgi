using Dapper;
using DebtTracker.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace DebtTracker.DataAccessLayer
{
    public class DapperRepository<T> : IRepository<T> where T : class, IDomainObject
    {
        private readonly string _connectionString =
            @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=DolgiDb;Integrated Security=True;";

        private readonly IDbConnection _db;

        public DapperRepository()
        {
            _db = new SqlConnection(_connectionString);
        }

        // Можно добавить конструктор с внедрением строки подключения (DIP)
        public DapperRepository(string connectionString)
        {
            _connectionString = connectionString;
            _db = new SqlConnection(_connectionString);
        }

        // CREATE
        public void Create(T item)
        {
            var sqlQuery = @"INSERT INTO Debts (Subject, Description, Status, Deadline) 
                           VALUES (@Subject, @Description, @Status, @Deadline);
                           SELECT CAST(SCOPE_IDENTITY() as int)";

            var id = _db.Query<int>(sqlQuery, item).FirstOrDefault();

            // Устанавливаем сгенерированный ID обратно в объект
            if (item is Debt debt)
            {
                debt.Id = id;
            }
            else
            {
                var property = item.GetType().GetProperty("Id");
                if (property != null && property.CanWrite)
                {
                    property.SetValue(item, id);
                }
            }
        }

        // READ
        public IEnumerable<T> GetAll()
        {
            return _db.Query<T>("SELECT * FROM Debts").ToList();
        }

        public T GetById(int id)
        {
            return _db.Query<T>("SELECT * FROM Debts WHERE Id = @Id", new { Id = id }).FirstOrDefault();
        }

        // UPDATE
        public void Update(T item)
        {
            var sqlQuery = @"UPDATE Debts 
                           SET Subject = @Subject, 
                               Description = @Description, 
                               Status = @Status, 
                               Deadline = @Deadline 
                           WHERE Id = @Id";

            _db.Execute(sqlQuery, item);
        }

        // DELETE
        public void Delete(int id)
        {
            var sqlQuery = "DELETE FROM Debts WHERE Id = @Id";
            _db.Execute(sqlQuery, new { Id = id });
        }
    }
}