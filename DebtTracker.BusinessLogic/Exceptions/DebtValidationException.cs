using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DebtTracker.BusinessLogic.Exceptions
{
    /// <summary>
    /// Исключение, выбрасываемое при нарушении бизнес-правил валидации данных долга.
    /// Наследуется от <see cref="BusinessLogicException"/>.
    /// </summary>
    /// <remarks>
    /// Возникает при попытке создания или обновления долга с некорректными данными:
    /// <list type="bullet">
    /// <item>Пустое или некорректное название предмета</item>
    /// <item>Недопустимый статус выполнения</item>
    /// <item>Некорректная или просроченная дата дедлайна</item>
    /// <item>Нарушение других бизнес-правил валидации</item>
    /// </list>
    /// Отличается от <see cref="DebtOperationException"/> тем, что связано с проверкой входных данных, а не с ошибками выполнения операций.
    /// </remarks>
    public class DebtValidationException : BusinessLogicException
    {
        /// <summary>
        /// Инициализирует новый экземпляр исключения с указанным сообщением об ошибке валидации.
        /// </summary>
        /// <param name="message">Сообщение, описывающее нарушение правил валидации.</param>
        public DebtValidationException(string message) : base(message) { }

        /// <summary>
        /// Инициализирует новый экземпляр исключения с указанным сообщением об ошибке валидации и ссылкой на внутреннее исключение.
        /// </summary>
        /// <param name="message">Сообщение, описывающее нарушение правил валидации.</param>
        /// <param name="innerException">Исключение, вызвавшее текущее исключение.</param>
        public DebtValidationException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}
