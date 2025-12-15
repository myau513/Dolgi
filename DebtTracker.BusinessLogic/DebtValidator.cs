using System;
using DebtTracker.Entities;

namespace DebtTracker.BusinessLogic
{
    public static class DebtValidator
    {
        public static bool ValidateSubject(string subject)
        {
            return !string.IsNullOrWhiteSpace(subject);
        }

        public static bool ValidateDescription(string description)
        {
            // Описание может быть пустым по ТЗ, проверяем только на null
            return description != null;
        }

        public static bool ValidateStatus(int statusValue)
        {
            return Enum.IsDefined(typeof(DebtStatus), statusValue);
        }

        public static bool ValidateDeadline(string dateString, out DateTime deadline)
        {
            return DateTime.TryParse(dateString, out deadline);
        }

        public static bool ValidateDebt(Debt debt)
        {
            return ValidateSubject(debt.Subject) &&
                   ValidateDescription(debt.Description) &&
                   debt.Deadline != DateTime.MinValue;
        }
    }
}