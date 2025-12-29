using System;
using System.Collections.Generic;
using System.Linq;
using DebtTracker.Dto;
using DebtTracker.Entities;

namespace DebtTracker.BusinessLogic
{
    public class DebtModel : IDebtModel
    {
        private readonly IDebtService _service;

        public event Action<IEnumerable<DebtDto>> DebtsLoaded;
        public event Action<IEnumerable<DebtDto>> TomorrowDebtsLoaded;
        public event Action<string> ErrorOccurred;

        public DebtModel(IDebtService service)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
        }

        public void LoadDebts()
        {
            try
            {
                // Было: _service.GetAllDebts();
                // В IDebtService есть только GetAllDebtsSorted()
                var debts = _service.GetAllDebtsSorted();
                var dtos = debts.Select(MapToDto).ToList();
                DebtsLoaded?.Invoke(dtos);
            }
            catch (Exception ex)
            {
                ErrorOccurred?.Invoke(ex.Message);
            }
        }

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

        public DebtDto GetById(int id)
        {
            var debt = _service.GetDebtById(id);
            return MapToDto(debt);
        }

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

        private Debt MapToEntity(DebtDto dto)
        {
            return new Debt
            {
                Id = dto.Id,
                Subject = dto.Subject,
                Description = dto.Description,
                // Было: Enum.Parse<DebtStatus>(dto.Status)
                // Делаем старый вариант через typeof
                Status = (DebtStatus)Enum.Parse(typeof(DebtStatus), dto.Status),
                Deadline = dto.Deadline
            };
        }
    }
}
