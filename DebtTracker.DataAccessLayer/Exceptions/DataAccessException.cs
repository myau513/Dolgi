using System;

namespace DebtTracker.DataAccessLayer.Exceptions
{
    /// <summary>
    /// Базовое исключение для всех ошибок слоя доступа к данным (Data Access Layer).
    /// Наследуется от <see cref="Exception"/> и служит родительским классом для специализированных исключений DAL.
    /// </summary>
    /// <remarks>
    /// Используется для:
    /// <list type="bullet">
    /// <item>Обработки ошибок, связанных с операциями базы данных</item>
    /// <item>Создания иерархии исключений уровня доступа к данным</item>
    /// <item>Отделения технических ошибок БД от бизнес-логики</item>
    /// </list>
    /// </remarks>
    public class DataAccessException : Exception
    {
        /// <summary>
        /// Инициализирует новый экземпляр исключения с указанным сообщением об ошибке.
        /// </summary>
        /// <param name="message">Сообщение, описывающее ошибку доступа к данным.</param>
        public DataAccessException(string message) : base(message) { }

        /// <summary>
        /// Инициализирует новый экземпляр исключения с указанным сообщением об ошибке и ссылкой на внутреннее исключение.
        /// </summary>
        /// <param name="message">Сообщение, описывающее ошибку доступа к данным.</param>
        /// <param name="innerException">Исключение, вызвавшее текущее исключение.</param>
        public DataAccessException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}