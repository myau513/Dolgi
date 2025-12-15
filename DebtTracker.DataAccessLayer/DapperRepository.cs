using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using Dapper;
using DebtTracker.Entities;

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

        public IEnumerable<T> GetAll()
        {
            return _db.Query<T>("SELECT * FROM Debts").ToList();
        }

        public T GetById(int id)
        {
            return _db.Query<T>("SELECT * FROM Debts WHERE Id = @Id", new { Id = id }).FirstOrDefault();
        }

        public void Create(T item)
        {
            var sqlQuery = @"INSERT INTO Debts (Subject, Description, Status, Deadline) 
                           VALUES (@Subject, @Description, @Status, @Deadline);
                           SELECT CAST(SCOPE_IDENTITY() as int)";

            var id = _db.Query<int>(sqlQuery, item).FirstOrDefault();
            item.Id = id;
        }

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

        public void Delete(int id)
        {
            var sqlQuery = "DELETE FROM Debts WHERE Id = @Id";
            _db.Execute(sqlQuery, new { Id = id });
        }

        public void Save()
        {
            // Для Dapper не требуется явное сохранение
        }
    }
}