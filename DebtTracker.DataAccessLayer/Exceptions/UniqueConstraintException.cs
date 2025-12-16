using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DebtTracker.DataAccessLayer.Exceptions
{
    public class UniqueConstraintException : DataAccessException
    {
        public UniqueConstraintException(string message) : base(message) { }
        public UniqueConstraintException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}
