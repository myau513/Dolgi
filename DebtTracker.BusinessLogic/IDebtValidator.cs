using DebtTracker.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DebtTracker.BusinessLogic
{
    public interface IDebtValidator
    {
        bool ValidateDebt(Debt debt);
        bool ValidateSubject(string subject);
        bool ValidateDescription(string description);
        bool ValidateDeadline(string dateString, out DateTime deadline); 
        bool ValidateStatus(int statusValue); 
        bool ValidateStatusEnum(DebtStatus status); 
    }
}
