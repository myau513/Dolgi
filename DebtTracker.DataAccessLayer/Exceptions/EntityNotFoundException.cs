using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DebtTracker.DataAccessLayer.Exceptions
{
    public class EntityNotFoundException : DataAccessException
    {
        public EntityNotFoundException(string message) : base(message) { }
        public EntityNotFoundException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}
