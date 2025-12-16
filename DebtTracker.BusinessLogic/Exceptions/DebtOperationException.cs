using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DebtTracker.BusinessLogic.Exceptions
{
    /// <summary>
    /// Базовое исключение для ошибок, связанных с операциями CRUD (создание, чтение, обновление, удаление) над долгами.
    /// Наследуется от <see cref="BusinessLogicException"/> и служит родительским классом для более специфичных исключений операций с долгами.
    /// </summary>
    /// <remarks>
    /// Используется для обработки ошибок на уровне бизнес-операций с долгами:
    /// <list type="bullet">
    /// <item>Ошибки базы данных при выполнении операций с долгами</item>
    /// <item>Проблемы с подключением к хранилищу данных</item>
    /// <item>Общие ошибки операций, не покрытые более специфичными исключениями</item>
    /// </list>
    /// </remarks>
    public class DebtOperationException : BusinessLogicException
    {
        /// <summary>
        /// Инициализирует новый экземпляр исключения с указанным сообщением об ошибке.
        /// </summary>
        /// <param name="message">Сообщение, описывающее ошибку операции с долгом.</param>
        public DebtOperationException(string message) : base(message) { }

        /// <summary>
        /// Инициализирует новый экземпляр исключения с указанным сообщением об ошибке и ссылкой на внутреннее исключение.
        /// </summary>
        /// <param name="message">Сообщение, описывающее ошибку операции с долгом.</param>
        /// <param name="innerException">Исключение, вызвавшее текущее исключение (например, DataAccessException из DAL).</param>
        public DebtOperationException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}
