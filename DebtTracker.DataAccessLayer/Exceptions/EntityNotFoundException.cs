using System;

namespace DebtTracker.DataAccessLayer.Exceptions
{
    /// <summary>
    /// Исключение, выбрасываемое при попытке доступа к несуществующей сущности в базе данных.
    /// Наследуется от <see cref="DataAccessException"/>.
    /// </summary>
    /// <remarks>
    /// Возникает в следующих случаях:
    /// <list type="bullet">
    /// <item>Попытка получить сущность по несуществующему идентификатору</item>
    /// <item>Попытка обновить или удалить сущность, которая была удалена или никогда не существовала</item>
    /// <item>Ссылка на сущность в связанных данных недействительна</item>
    /// </list>
    /// Это исключение преобразуется в <see cref="BusinessLogic.Exceptions.DebtNotFoundException"/> на уровне бизнес-логики.
    /// </remarks>
    public class EntityNotFoundException : DataAccessException
    {
        /// <summary>
        /// Инициализирует новый экземпляр исключения с указанным сообщением об ошибке.
        /// </summary>
        /// <param name="message">Сообщение, описывающее причину исключения (обычно содержит ID несуществующей сущности).</param>
        public EntityNotFoundException(string message) : base(message) { }

        /// <summary>
        /// Инициализирует новый экземпляр исключения с указанным сообщением об ошибке и ссылкой на внутреннее исключение.
        /// </summary>
        /// <param name="message">Сообщение, описывающее причину исключения.</param>
        /// <param name="innerException">Исключение, вызвавшее текущее исключение.</param>
        public EntityNotFoundException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}