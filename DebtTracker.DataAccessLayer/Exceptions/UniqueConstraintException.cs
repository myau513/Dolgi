using System;

namespace DebtTracker.DataAccessLayer.Exceptions
{
    /// <summary>
    /// Исключение, выбрасываемое при нарушении ограничения уникальности (UNIQUE constraint) в базе данных.
    /// Наследуется от <see cref="DataAccessException"/>.
    /// </summary>
    /// <remarks>
    /// Возникает в следующих случаях:
    /// <list type="bullet">
    /// <item>Попытка вставить запись с дублирующимся значением в поле с ограничением UNIQUE</item>
    /// <item>Попытка обновить запись, создав дубликат уникального значения</item>
    /// <item>Нарушение ограничения первичного ключа (PRIMARY KEY constraint)</item>
    /// </list>
    /// Обычно соответствует SQL Server ошибке с кодом 2627.
    /// </remarks>
    public class UniqueConstraintException : DataAccessException
    {
        /// <summary>
        /// Инициализирует новый экземпляр исключения с указанным сообщением об ошибке.
        /// </summary>
        /// <param name="message">Сообщение, описывающее нарушение ограничения уникальности.</param>
        public UniqueConstraintException(string message) : base(message) { }

        /// <summary>
        /// Инициализирует новый экземпляр исключения с указанным сообщением об ошибке и ссылкой на внутреннее исключение.
        /// </summary>
        /// <param name="message">Сообщение, описывающее нарушение ограничения уникальности.</param>
        /// <param name="innerException">Исключение, вызвавшее текущее исключение (например, SqlException с кодом 2627).</param>
        public UniqueConstraintException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}