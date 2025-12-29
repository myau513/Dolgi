using System;
using DebtTracker.Dto;
using DebtTracker.Presentation.Contracts;

namespace DebtTracker.ConsoleApp
{
    public class ConsoleDebtView : IMainDebtView
    {
        public event Action LoadView;
        public event Action AddRequested;
        public event Action EditRequested;
        public event Action DeleteRequested;
        public event Action RefreshRequested;
        public event Action TomorrowDebtsRequested;

        public int? SelectedDebtId { get; private set; }

        public void Run()
        {
            LoadView?.Invoke(); 

            bool exit = false;
            while (!exit)
            {
                ConsoleHelper.WriteTitle("Меню");
                Console.WriteLine("1 - Показать все долги");
                Console.WriteLine("2 - Добавить долг");
                Console.WriteLine("3 - Редактировать долг");
                Console.WriteLine("4 - Удалить долг");
                Console.WriteLine("5 - Показать долги на завтра");
                Console.WriteLine("0 - Выход");
                Console.Write("Выбор: ");

                var key = Console.ReadLine();

                switch (key)
                {
                    case "1":
                        RefreshRequested?.Invoke();
                        break;
                    case "2":
                        AddRequested?.Invoke();
                        break;
                    case "3":
                        AskSelectedId();
                        EditRequested?.Invoke();
                        break;
                    case "4":
                        AskSelectedId();
                        DeleteRequested?.Invoke();
                        break;
                    case "5":
                        TomorrowDebtsRequested?.Invoke();
                        break;
                    case "0":
                        exit = true;
                        break;
                    default:
                        ShowError("Неизвестная команда.");
                        break;
                }
            }
        }

        private void AskSelectedId()
        {
            Console.Write("Введите Id долга: ");
            if (int.TryParse(Console.ReadLine(), out var id))
            {
                SelectedDebtId = id;
            }
            else
            {
                SelectedDebtId = null;
                ShowError("Некорректный Id.");
            }
        }

        public void ShowDebts(IEnumerable<DebtDto> debts)
        {
            ConsoleHelper.WriteTitle("Список долгов");
            ConsoleHelper.WriteDebts(debts);
            Console.WriteLine();
        }

        public void ShowTomorrowWarning(IEnumerable<DebtDto> debts)
        {
            ConsoleHelper.WriteTitle("Долги на завтра");
            ConsoleHelper.WriteDebts(debts);
            Console.WriteLine();
        }

        public void ShowError(string message)
        {
            ConsoleHelper.WriteError(message);
        }

        public void ShowInfo(string message)
        {
            ConsoleHelper.WriteInfo(message);
        }
    }
}
