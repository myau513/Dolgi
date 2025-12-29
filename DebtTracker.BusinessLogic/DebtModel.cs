using DebtTracker.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using DebtTracker.Dto;

namespace DebtTracker.BusinessLogic 
{
    /// <summary>
    /// Модель уровня MVP (Model) — адаптер между Presenter'ами и бизнес-логикой (IDebtService).
    /// 
    /// Задачи:
    /// 1) Вызывать бизнес-операции через сервис (CRUD + доп. выборки).
    /// 2) Преобразовывать доменные сущности Debt в DTO (DebtDto) для UI и обратно.
    /// 3) Уведомлять Presenter о результатах через события (DebtsLoaded / ErrorOccurred / TomorrowDebtsLoaded).
    /// 
    /// Важно: модель не знает конкретные формы и не содержит логики отображения.
    /// </summary>
    public class DebtModel : IDebtModel
    {
        /// <summary>
        /// Сервис бизнес-логики, через который выполняются операции с долгами.
        /// Модель не работает напрямую с БД/репозиториями — только через сервис.
        /// </summary>
        private readonly IDebtService _debtService;

        /// <summary>
        /// Событие, отправляющее в Presenter обновлённый список долгов (DTO),
        /// чтобы Presenter мог передать его в View для отображения.
        /// </summary>
        public event Action<IEnumerable<DebtDto>> DebtsLoaded;

        /// <summary>
        /// Событие уведомления об ошибке.
        /// Используется для передачи сообщения об исключениях из бизнес-слоя в Presenter/View.
        /// </summary>
        public event Action<string> ErrorOccurred;

        /// <summary>
        /// Событие, отправляющее список долгов, у которых дедлайн наступает завтра.
        /// Обычно используется для предупреждения пользователя.
        /// </summary>
        public event Action<IEnumerable<DebtDto>> TomorrowDebtsLoaded;

        /// <summary>
        /// Создаёт модель, привязывая её к сервису бизнес-логики.
        /// </summary>
        /// <param name="debtService">Сервис, выполняющий бизнес-операции с долгами.</param>
        public DebtModel(IDebtService debtService)
        {
            _debtService = debtService;
        }

        /// <summary>
        /// Загружает долги с дедлайном на завтра и уведомляет подписчиков через TomorrowDebtsLoaded.
        /// </summary>
        public void LoadTomorrowDebts()
        {
            try
            {
                var debts = _debtService
                    .GetDebtsWithTomorrowDeadline()
                    .Select(MapToDto)
                    .ToList();

                TomorrowDebtsLoaded?.Invoke(debts);
            }
            catch (Exception ex)
            {
                ErrorOccurred?.Invoke(ex.Message);
            }
        }

        /// <summary>
        /// Удаляет долг по идентификатору, затем обновляет список долгов (LoadDebts),
        /// чтобы UI сразу отобразил актуальные данные.
        /// </summary>
        /// <param name="id">Идентификатор удаляемого долга.</param>
        public void DeleteDebt(int id)
        {
            try
            {
                _debtService.DeleteDebt(id);
                LoadDebts();
            }
            catch (Exception ex)
            {
                ErrorOccurred?.Invoke(ex.Message);
            }
        }

        /// <summary>
        /// Обновляет существующий долг на основе DTO, затем перезагружает список долгов.
        /// </summary>
        /// <param name="dto">DTO с новыми данными долга.</param>
        public void UpdateDebt(DebtDto dto)
        {
            try
            {
                var debt = MapToEntity(dto);
                _debtService.UpdateDebt(debt);
                LoadDebts();
            }
            catch (Exception ex)
            {
                ErrorOccurred?.Invoke(ex.Message);
            }
        }

        /// <summary>
        /// Загружает список долгов (обычно отсортированный) и уведомляет подписчиков через DebtsLoaded.
        /// Presenter, получив событие, обновляет View.
        /// </summary>
        public void LoadDebts()
        {
            try
            {
                var debts = _debtService.GetAllDebtsSorted();

                var dtoList = debts.Select(MapToDto).ToList();

                DebtsLoaded?.Invoke(dtoList);
            }
            catch (Exception ex)
            {
                ErrorOccurred?.Invoke(ex.Message);
            }
        }

        /// <summary>
        /// Добавляет новый долг на основе DTO и затем обновляет список долгов.
        /// 
        /// Перезагрузка списка нужна, чтобы UI сразу увидел созданную запись.
        /// </summary>
        /// <param name="debtDto">DTO нового долга (данные из формы/ввода пользователя).</param>
        public void AddDebt(DebtDto debtDto)
        {
            try
            {
                var debt = MapToEntity(debtDto);

                _debtService.AddDebt(debt);

                // Обновляем список, чтобы Presenter получил DebtsLoaded и обновил View.
                LoadDebts();
            }
            catch (Exception ex)
            {
                ErrorOccurred?.Invoke(ex.Message);
            }
        }

        /// <summary>
        /// Возвращает долг по ID в виде DTO (удобно для заполнения формы редактирования).
        /// </summary>
        /// <param name="id">Идентификатор долга.</param>
        /// <returns>DTO долга или null, если произошла ошибка.</returns>
        public DebtDto GetById(int id)
        {
            try
            {
                var debt = _debtService.GetDebtById(id);
                return MapToDto(debt);
            }
            catch (Exception ex)
            {
                ErrorOccurred?.Invoke(ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Преобразует доменную сущность Debt в DTO для передачи в UI.
        /// DTO используется, чтобы UI не зависел от доменного слоя/EF и не получал лишние поля/связи.
        /// </summary>
        /// <param name="debt">Доменная сущность долга.</param>
        /// <returns>DTO долга или null, если входной объект равен null.</returns>
        private static DebtDto MapToDto(Debt debt)
        {
            if (debt == null)
                return null;

            return new DebtDto
            {
                Id = debt.Id,
                Subject = debt.Subject,
                Description = debt.Description,
                Status = debt.Status.ToString(),
                Deadline = debt.Deadline
            };
        }

        /// <summary>
        /// Преобразует DTO (данные из UI) в доменную сущность Debt для бизнес-слоя.
        /// Статус парсится из строки в enum DebtStatus, при ошибке берётся значение NotStarted.
        /// </summary>
        /// <param name="dto">DTO долга.</param>
        /// <returns>Доменная сущность долга или null, если dto равен null.</returns>
        private static Debt MapToEntity(DebtDto dto)
        {
            if (dto == null)
                return null;

            return new Debt
            {
                Id = dto.Id,
                Subject = dto.Subject,
                Description = dto.Description,
                Status = Enum.TryParse<DebtStatus>(dto.Status, out var status)
                    ? status
                    : DebtStatus.NotStarted,
                Deadline = dto.Deadline
            };
        }
    }
}

