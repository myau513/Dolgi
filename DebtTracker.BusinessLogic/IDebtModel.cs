using System;
using System.Collections.Generic;
using DebtTracker.Dto;

namespace DebtTracker.BusinessLogic
{
    /// <summary>
    /// Контракт модели (Model) в архитектуре MVP.
    /// 
    /// Модель выступает посредником между Presenter'ом и бизнес-логикой (IDebtService):
    /// - выполняет операции над долгами,
    /// - преобразует доменные сущности в DTO,
    /// - уведомляет Presenter о результатах через события.
    /// 
    /// Модель не знает о конкретных представлениях (View) и не содержит UI-логики.
    /// </summary>
    public interface IDebtModel
    {
        event Action<IEnumerable<DebtDto>> TomorrowDebtsLoaded;
        /// <summary>
        /// Событие, уведомляющее Presenter о том,
        /// что список долгов был загружен или обновлён.
        /// 
        /// Presenter, получив это событие, передаёт данные в View для отображения.
        /// </summary>
        event Action<IEnumerable<DebtDto>> DebtsLoaded;

        /// <summary>
        /// Событие, уведомляющее Presenter о возникновении ошибки
        /// в процессе выполнения бизнес-операций.
        /// </summary>
        event Action<string> ErrorOccurred;
        void LoadTomorrowDebts();

        /// <summary>
        /// Загружает список долгов (как правило, отсортированный)
        /// и инициирует событие DebtsLoaded.
        /// </summary>
        void LoadDebts();

        /// <summary>
        /// Добавляет новый долг на основе DTO,
        /// после чего обновляет список долгов.
        /// </summary>
        /// <param name="debt">DTO нового долга.</param>
        void AddDebt(DebtDto debt);

        /// <summary>
        /// Обновляет существующий долг на основе DTO,
        /// затем обновляет список долгов.
        /// </summary>
        /// <param name="debt">DTO с обновлёнными данными долга.</param>
        void UpdateDebt(DebtDto debt);

        /// <summary>
        /// Удаляет долг по его идентификатору
        /// и обновляет список долгов.
        /// </summary>
        /// <param name="debtId">Идентификатор удаляемого долга.</param>
        void DeleteDebt(int debtId);

        /// <summary>
        /// Возвращает долг по идентификатору в виде DTO.
        /// Используется, например, для заполнения формы редактирования.
        /// </summary>
        /// <param name="id">Идентификатор долга.</param>
        /// <returns>DTO долга или null, если долг не найден или произошла ошибка.</returns>
        DebtDto GetById(int id);
    }
}
