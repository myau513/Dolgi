using DebtTracker._Shared;
using DebtTracker.Entities;
using DebtTracker.BusinessLogic;
using System;
using System.Collections.Generic;
using System.Linq;

public class DebtModel : IDebtModel
{
    private readonly IDebtService _debtService;

    public event Action<IEnumerable<DebtDto>> DebtsLoaded;
    public event Action<string> ErrorOccurred;
    public event Action<IEnumerable<DebtDto>> TomorrowDebtsLoaded;


    public DebtModel(IDebtService debtService)
    {
        _debtService = debtService;
    }
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

    public void AddDebt(DebtDto debtDto)
    {
        try
        {
            var debt = MapToEntity(debtDto);

            _debtService.AddDebt(debt);

            LoadDebts(); // обновляем UI
        }
        catch (Exception ex)
        {
            ErrorOccurred?.Invoke(ex.Message);
        }
    }
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


    private static Debt MapToEntity(DebtDto dto)
    {
        if (dto == null)
            return null;

        return new Debt
        {
            Id = dto.Id,
            Subject = dto.Subject,
            Description = dto.Description,
            Status = Enum.TryParse<DebtStatus>(
            dto.Status,
            out var status)
            ? status
         : DebtStatus.NotStarted,                                 
            Deadline = dto.Deadline
        };
    }

}
