using System;
using System.Collections.Generic;
using DebtTracker.Dto;
using DebtTracker.Presentation.Contracts;

namespace DebtTracker.ConsoleApp
{
    public class ConsoleDebtView : IConsoleMainDebtView
    {
        public event Action LoadView;
        public event Action AddRequested;
        public event Action EditRequested;
        public event Action DeleteRequested;
        public event Action RefreshRequested;
        public event Action TomorrowDebtsRequested;

        public int SelectedDebtId { get; private set; }


        public void RunLoop()
        {
           
            LoadView?.Invoke();

            bool exit = false;
            while (!exit)
            {

                Console.WriteLine();
                Console.WriteLine("1 - Показать все долги");
                Console.WriteLine("2 - Добавить долг");
                Console.WriteLine("3 - Редактировать");
                Console.WriteLine("4 - Удалить");
                Console.WriteLine("0 - Выход");
                Console.Write("Выбор: ");

                var key = Console.ReadKey(true).Key;
                Console.WriteLine();

                switch (key)
                {
                    case ConsoleKey.D1:
                        Console.WriteLine("→ Команда: Показать все долги");
                        RefreshRequested?.Invoke();
                        break;

                    case ConsoleKey.D2:
                        Console.WriteLine("→ Команда: Добавить долг");
                        AddRequested?.Invoke();
                        break;

                    case ConsoleKey.D3:
                        Console.WriteLine("→ Команда: Редактировать");
                        AskSelectedId();
                        EditRequested?.Invoke();
                        break;

                    case ConsoleKey.D4:
                        Console.WriteLine("→ Команда: Удалить");
                        AskSelectedId();
                        DeleteRequested?.Invoke();
                        break;

                    case ConsoleKey.D0:
                        Console.WriteLine("→ Выход");
                        exit = true;
                        break;

                    default:
                        ShowError("Неизвестная команда. Повторите ввод.");
                        break;
                }
            }
        }



        private void AskSelectedId()
        {
            Console.Write("Введите Id долга: ");
            if (int.TryParse(Console.ReadLine(), out var id))
                SelectedDebtId = id;
            else
            {
                SelectedDebtId = -1;
                ShowError("Некорректный Id");
            }
        }

        public void ShowDebts(IEnumerable<DebtDto> debts)
        {
            foreach (var d in debts)
            {
                Console.WriteLine($"[{d.Id}] {d.Subject} | {d.Status} | до {d.Deadline:d}");
            }
            Console.WriteLine();
        }

        public void ShowTomorrowWarning(IEnumerable<DebtDto> debts)
        {
            if (debts == null || !debts.Any())
                return;

            Console.WriteLine("=== Долги на завтра ===");
            foreach (var d in debts)
            {
                Console.WriteLine($"{d.Subject} до {d.Deadline:yyyy-MM-dd}");
            }
            Console.WriteLine();
        }


        public void ShowMessage(string message)
            => Console.WriteLine(message);

        public void ShowError(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("[Ошибка] " + message);
            Console.ResetColor();
        }
    }
}
