using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DebtTracker.DataAccessLayer
{
    //интерфейс для конфигурации подключения чтобы не была жестко зашитая строка подключения
    public interface IDbConnectionFactory
    {
        IDbConnection CreateConnection();
    }
}
