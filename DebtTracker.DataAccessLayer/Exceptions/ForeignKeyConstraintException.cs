using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DebtTracker.DataAccessLayer.Exceptions
{
    public class ForeignKeyConstraintException : DataAccessException
    {
        public ForeignKeyConstraintException(string message) : base(message) { }
        public ForeignKeyConstraintException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}
