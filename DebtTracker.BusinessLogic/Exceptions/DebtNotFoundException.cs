using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DebtTracker.BusinessLogic.Exceptions
{
    /// <summary>
    /// Исключение, выбрасываемое при попытке выполнения операции с несуществующим долгом.
    /// Наследуется от <see cref="DebtOperationException"/>.
    /// </summary>
    /// <remarks>
    /// Возникает в следующих случаях:
    /// <list type="bullet">
    /// <item>Попытка получить, обновить или удалить долг по несуществующему ID</item>
    /// <item>Долг не найден при поиске по заданным критериям</item>
    /// <item>Ссылка на долг в связанных операциях недействительна</item>
    /// </list>
    /// </remarks>
    public class DebtNotFoundException : DebtOperationException
    {
        /// <summary>
        /// Инициализирует новый экземпляр исключения с указанным сообщением об ошибке.
        /// </summary>
        /// <param name="message">Сообщение, описывающее причину исключения (обычно содержит ID несуществующего долга).</param>
        public DebtNotFoundException(string message) : base(message) { }

        /// <summary>
        /// Инициализирует новый экземпляр исключения с указанным сообщением об ошибке и ссылкой на внутреннее исключение.
        /// </summary>
        /// <param name="message">Сообщение, описывающее причину исключения.</param>
        /// <param name="innerException">Исключение, вызвавшее текущее исключение (например, EntityNotFoundException из DAL).</param>
        public DebtNotFoundException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}
