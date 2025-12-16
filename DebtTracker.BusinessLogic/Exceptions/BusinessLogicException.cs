using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DebtTracker.BusinessLogic.Exceptions
{
    /// <summary>
    /// Базовое исключение для всех ошибок бизнес-логики приложения.
    /// Наследуется от System.Exception и служит родительским классом для специализированных исключений бизнес-уровня.
    /// </summary>
    /// <remarks>
    /// Используется для:
    /// 1. Обработки ошибок, связанных с бизнес-правилами и валидацией
    /// 2. Создания иерархии исключений бизнес-уровня
    /// 3. Отделения бизнес-ошибок от технических исключений
    /// </remarks>
    public class BusinessLogicException : Exception
    {
        /// <summary>
        /// Инициализирует новый экземпляр исключения с указанным сообщением об ошибке.
        /// </summary>
        /// <param name="message">Сообщение, описывающее ошибку.</param>
        public BusinessLogicException(string message) : base(message) { }

        /// <summary>
        /// Инициализирует новый экземпляр исключения с указанным сообщением об ошибке и ссылкой на внутреннее исключение.
        /// </summary>
        /// <param name="message">Сообщение, описывающее ошибку.</param>
        /// <param name="innerException">Исключение, вызвавшее текущее исключение.</param>
        public BusinessLogicException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}
