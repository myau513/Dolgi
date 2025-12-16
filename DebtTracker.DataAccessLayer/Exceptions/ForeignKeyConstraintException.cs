using System;

namespace DebtTracker.DataAccessLayer.Exceptions
{
    /// <summary>
    /// Исключение, выбрасываемое при нарушении ограничения внешнего ключа (FOREIGN KEY constraint) в базе данных.
    /// Наследуется от <see cref="DataAccessException"/>.
    /// </summary>
    /// <remarks>
    /// Возникает в следующих случаях:
    /// <list type="bullet">
    /// <item>Попытка удалить запись, на которую ссылаются другие записи</item>
    /// <item>Попытка создать запись с несуществующим внешним ключом</item>
    /// <item>Попытка обновить внешний ключ на несуществующее значение</item>
    /// </list>
    /// Обычно соответствует SQL Server ошибке с кодом 547.
    /// </remarks>
    public class ForeignKeyConstraintException : DataAccessException
    {
        /// <summary>
        /// Инициализирует новый экземпляр исключения с указанным сообщением об ошибке.
        /// </summary>
        /// <param name="message">Сообщение, описывающее нарушение ограничения внешнего ключа.</param>
        public ForeignKeyConstraintException(string message) : base(message) { }

        /// <summary>
        /// Инициализирует новый экземпляр исключения с указанным сообщением об ошибке и ссылкой на внутреннее исключение.
        /// </summary>
        /// <param name="message">Сообщение, описывающее нарушение ограничения внешнего ключа.</param>
        /// <param name="innerException">Исключение, вызвавшее текущее исключение (например, SqlException с кодом 547).</param>
        public ForeignKeyConstraintException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}