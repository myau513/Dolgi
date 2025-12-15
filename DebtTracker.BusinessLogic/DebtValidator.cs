using System;
using DebtTracker.Entities;

namespace DebtTracker.BusinessLogic
{
    public class DebtValidator : IDebtValidator
    {
        public bool ValidateDebt(Debt debt)
        {
            return ValidateSubject(debt.Subject) &&
                   ValidateDescription(debt.Description) &&
                   ValidateDeadline(debt.Deadline) &&
                   ValidateStatusEnum(debt.Status);
        }

        public bool ValidateSubject(string subject)
        {
            return !string.IsNullOrWhiteSpace(subject);
        }

        public bool ValidateDescription(string description)
        {
            return description != null; 
        }

        // Два варианта валидации дедлайна:
        public bool ValidateDeadline(string dateString, out DateTime deadline)
        {
            return DateTime.TryParse(dateString, out deadline);
        }

        public bool ValidateDeadline(DateTime deadline)
        {
            return deadline > DateTime.MinValue && deadline != DateTime.MaxValue;
        }

        public bool ValidateStatus(int statusValue)
        {
            return Enum.IsDefined(typeof(DebtStatus), statusValue);
        }

        public bool ValidateStatusEnum(DebtStatus status)
        {
            return Enum.IsDefined(typeof(DebtStatus), status);
        }
    }
}