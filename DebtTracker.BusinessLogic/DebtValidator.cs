using System;
using DebtTracker.Entities;

namespace DebtTracker.BusinessLogic
{
    /// <summary>
    /// Реализация валидатора для проверки данных долгов на соответствие бизнес-правилам.
    /// Реализует интерфейс <see cref="IDebtValidator"/>.
    /// </summary>
    public class DebtValidator : IDebtValidator
    {
        /// <summary>
        /// Проверяет все атрибуты объекта долга на соответствие бизнес-правилам.
        /// </summary>
        /// <param name="debt">Объект долга для валидации.</param>
        /// <returns>true, если все атрибуты долга валидны; иначе false.</returns>
        public bool ValidateDebt(Debt debt)
        {
            return ValidateSubject(debt.Subject) &&
                   ValidateDescription(debt.Description) &&
                   ValidateDeadline(debt.Deadline) &&
                   ValidateStatusEnum(debt.Status);
        }

        /// <summary>
        /// Проверяет валидность названия предмета долга.
        /// </summary>
        /// <param name="subject">Название предмета для проверки.</param>
        /// <returns>true, если название не null и не состоит только из пробелов; иначе false.</returns>
        public bool ValidateSubject(string subject)
        {
            return !string.IsNullOrWhiteSpace(subject);
        }

        /// <summary>
        /// Проверяет валидность описания долга.
        /// </summary>
        /// <param name="description">Описание долга для проверки.</param>
        /// <returns>true, если описание не равно null; иначе false.</returns>
        /// <remarks>Описание может быть пустой строкой, но не должно быть null.</remarks>
        public bool ValidateDescription(string description)
        {
            return description != null;
        }

        /// <summary>
        /// Пытается преобразовать строку в дату дедлайна и проверяет ее валидность.
        /// </summary>
        /// <param name="dateString">Строка с датой для парсинга.</param>
        /// <param name="deadline">Выходной параметр - преобразованная дата.</param>
        /// <returns>true, если строка успешно преобразована в дату; иначе false.</returns>
        public bool ValidateDeadline(string dateString, out DateTime deadline)
        {
            return DateTime.TryParse(dateString, out deadline);
        }

        /// <summary>
        /// Проверяет валидность даты дедлайна.
        /// </summary>
        /// <param name="deadline">Дата дедлайна для проверки.</param>
        /// <returns>true, если дата не является минимальной или максимальной; иначе false.</returns>
        public bool ValidateDeadline(DateTime deadline)
        {
            return deadline > DateTime.MinValue && deadline != DateTime.MaxValue;
        }

        /// <summary>
        /// Проверяет, является ли числовое значение допустимым статусом долга.
        /// </summary>
        /// <param name="statusValue">Числовое значение статуса для проверки.</param>
        /// <returns>true, если значение является допустимым статусом; иначе false.</returns>
        public bool ValidateStatus(int statusValue)
        {
            return Enum.IsDefined(typeof(DebtStatus), statusValue);
        }

        /// <summary>
        /// Проверяет, является ли перечисление допустимым статусом долга.
        /// </summary>
        /// <param name="status">Значение перечисления статуса для проверки.</param>
        /// <returns>true, если значение является допустимым статусом; иначе false.</returns>
        public bool ValidateStatusEnum(DebtStatus status)
        {
            return Enum.IsDefined(typeof(DebtStatus), status);
        }
    }
}