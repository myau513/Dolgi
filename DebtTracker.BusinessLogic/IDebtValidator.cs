using DebtTracker.Entities;
using System;

namespace DebtTracker.BusinessLogic
{
    /// <summary>
    /// Интерфейс валидатора для проверки данных долгов на соответствие бизнес-правилам.
    /// Определяет контракт для всех операций валидации атрибутов долга.
    /// </summary>
    public interface IDebtValidator
    {
        /// <summary>
        /// Проверяет все атрибуты объекта долга на соответствие бизнес-правилам.
        /// </summary>
        /// <param name="debt">Объект долга для валидации.</param>
        /// <returns>true, если все атрибуты долга валидны; иначе false.</returns>
        bool ValidateDebt(Debt debt);

        /// <summary>
        /// Проверяет валидность названия предмета долга.
        /// </summary>
        /// <param name="subject">Название предмета для проверки.</param>
        /// <returns>true, если название не null и не состоит только из пробелов; иначе false.</returns>
        bool ValidateSubject(string subject);

        /// <summary>
        /// Проверяет валидность описания долга.
        /// </summary>
        /// <param name="description">Описание долга для проверки.</param>
        /// <returns>true, если описание не равно null; иначе false.</returns>
        bool ValidateDescription(string description);

        /// <summary>
        /// Пытается преобразовать строку в дату дедлайна и проверяет ее валидность.
        /// </summary>
        /// <param name="dateString">Строка с датой для парсинга.</param>
        /// <param name="deadline">Выходной параметр - преобразованная дата.</param>
        /// <returns>true, если строка успешно преобразована в дату; иначе false.</returns>
        bool ValidateDeadline(string dateString, out DateTime deadline);

        /// <summary>
        /// Проверяет, является ли числовое значение допустимым статусом долга.
        /// </summary>
        /// <param name="statusValue">Числовое значение статуса для проверки.</param>
        /// <returns>true, если значение является допустимым статусом; иначе false.</returns>
        bool ValidateStatus(int statusValue);

        /// <summary>
        /// Проверяет, является ли перечисление допустимым статусом долга.
        /// </summary>
        /// <param name="status">Значение перечисления статуса для проверки.</param>
        /// <returns>true, если значение является допустимым статусом; иначе false.</returns>
        bool ValidateStatusEnum(DebtStatus status);
    }
}