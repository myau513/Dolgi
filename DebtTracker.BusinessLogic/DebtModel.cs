using System;
using System.Collections.Generic;
using System.Linq;
using DebtTracker.Dto;
using DebtTracker.Entities;

namespace DebtTracker.BusinessLogic
{
    /// <summary>
    /// Модель (Model) в архитектуре MVP.
    /// Отвечает за бизнес-логику работы с долгами и связывает Presenter с сервисом доступа к данным.
    /// </summary>
    public class DebtModel : IDebtModel
    {
        private readonly IDebtService _service;

        /// <summary>
        /// Событие вызывается, когда успешно загружен список всех долгов.
        /// </summary>
        public event Action<IEnumerable<DebtDto>> DebtsLoaded;

        /// <summary>
        /// Событие вызывается, когда загружены долги с дедлайном на завтра.
        /// </summary>
        public event Action<IEnumerable<DebtDto>> TomorrowDebtsLoaded;

        /// <summary>
        /// Событие вызывается при любой ошибке бизнес-логики.
        /// </summary>
        public event Action<string> ErrorOccurred;

        /// <summary>
        /// Создаёт модель и получает зависимость сервиса через DI.
        /// </summary>
        /// <param name="service">Сервис работы с долговыми записями.</param>
        /// <exception cref="ArgumentNullException">Если сервис не был передан.</exception>
        public DebtModel(IDebtService service)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
        }

        /// <summary>
        /// Загружает все долги и уведомляет Presenter через событие <see cref="DebtsLoaded"/>.
        /// </summary>
        public void LoadDebts()
        {
            try
            {
                var debts = _service.GetAllDebtsSorted();
                var dtos = debts.Select(MapToDto).ToList();
                DebtsLoaded?.Invoke(dtos);
            }
            catch (Exception ex)
            {
                ErrorOccurred?.Invoke(ex.Message);
            }
        }

        /// <summary>
        /// Загружает долги, у которых дедлайн наступает завтра.
        /// </summary>
        public void LoadTomorrowDebts()
        {
            try
            {
                var debts = _service.GetDebtsWithTomorrowDeadline();
                var dtos = debts.Select(MapToDto).ToList();
                TomorrowDebtsLoaded?.Invoke(dtos);
            }
            catch (Exception ex)
            {
                ErrorOccurred?.Invoke(ex.Message);
            }
        }

        /// <summary>
        /// Добавляет новый долг и обновляет список долгов.
        /// </summary>
        /// <param name="dto">Данные нового долга.</param>
        public void AddDebt(DebtDto dto)
        {
            try
            {
                var entity = MapToEntity(dto);
                _service.AddDebt(entity);
                LoadDebts();
            }
            catch (Exception ex)
            {
                ErrorOccurred?.Invoke(ex.Message);
            }
        }

        /// <summary>
        /// Обновляет существующий долг и перезагружает список.
        /// </summary>
        /// <param name="dto">Обновлённые данные долга.</param>
        public void UpdateDebt(DebtDto dto)
        {
            try
            {
                var entity = MapToEntity(dto);
                _service.UpdateDebt(entity);
                LoadDebts();
            }
            catch (Exception ex)
            {
                ErrorOccurred?.Invoke(ex.Message);
            }
        }

        /// <summary>
        /// Удаляет долг по идентификатору и обновляет список.
        /// </summary>
        /// <param name="id">Идентификатор удаляемого долга.</param>
        public void DeleteDebt(int id)
        {
            try
            {
                _service.DeleteDebt(id);
                LoadDebts();
            }
            catch (Exception ex)
            {
                ErrorOccurred?.Invoke(ex.Message);
            }
        }

        /// <summary>
        /// Возвращает долг по идентификатору.
        /// </summary>
        /// <param name="id">ID долга.</param>
        /// <returns>DTO с данными долга.</returns>
        public DebtDto GetById(int id)
        {
            var debt = _service.GetDebtById(id);
            return MapToDto(debt);
        }

        /// <summary>
        /// Преобразует сущность базы данных в DTO для передачи слою Presentation.
        /// </summary>
        private DebtDto MapToDto(Debt debt)
        {
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
        /// Преобразует DTO обратно в сущность для хранения в базе.
        /// </summary>
        /// <param name="dto">Переданные данные долга.</param>
        private Debt MapToEntity(DebtDto dto)
        {
            return new Debt
            {
                Id = dto.Id,
                Subject = dto.Subject,
                Description = dto.Description,
                Status = (DebtStatus)Enum.Parse(typeof(DebtStatus), dto.Status),
                Deadline = dto.Deadline
            };
        }
    }
}
