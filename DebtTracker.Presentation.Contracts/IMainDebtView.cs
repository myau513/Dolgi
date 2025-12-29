using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using DebtTracker.Dto;

namespace DebtTracker.Presentation.Contracts
{
    /// <summary>
    /// Контракт главного экрана списка долгов.
    /// </summary>
    public interface IMainDebtView : IView
    {
        /// <summary>
        /// Событие: пользователь нажал "Добавить долг".
        /// </summary>
        event Action AddRequested;

        /// <summary>
        /// Событие: пользователь нажал "Редактировать долг".
        /// </summary>
        event Action EditRequested;

        /// <summary>
        /// Событие: пользователь нажал "Удалить долг".
        /// </summary>
        event Action DeleteRequested;

        /// <summary>
        /// Событие: пользователь запросил обновление списка.
        /// </summary>
        event Action RefreshRequested;

        /// <summary>
        /// Событие: пользователь запросил долги со сроком "на завтра".
        /// </summary>
        //event Action TomorrowDebtsRequested;

        /// <summary>
        /// Id выбранного долга в списке.
        /// null, если ничего не выбрано.
        /// </summary>
        int? SelectedDebtId { get; }

        /// <summary>
        /// Показать список долгов на основном экране.
        /// </summary>
        void ShowDebts(IEnumerable<DebtDto> debts);

        /// <summary>
        /// Показать предупреждение о долгах на завтра.
        /// Как именно — решает конкретная View (MessageBox, отдельный список и т.п.).
        /// </summary>
        void ShowTomorrowWarning(IEnumerable<DebtDto> debts);
    }
}

