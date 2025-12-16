using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DebtTracker.DataAccessLayer.Exceptions
{
    public class DataAccessException : Exception
    {
        public DataAccessException(string message) : base(message) { }
        public DataAccessException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}
