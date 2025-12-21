using System;
using System.Collections.Generic;
using System.Linq;
using DebtTracker.Entities;

namespace DebtTracker._Shared
{
    public class ConsoleDebtView : IMainDebtView
    {
        public event Action LoadView;
        public event Action AddRequested;
        public event Action EditRequested;
        public event Action DeleteRequested;
        public event Action RefreshRequested;

        public int SelectedDebtId { get; private set; }

        public void Run()
        {
            LoadView?.Invoke();

            bool exit = false;

            while (!exit)
            {
                Console.Clear();
                Console.WriteLine("=== УПРАВЛЕНИЕ ДОЛГАМИ ===");
                Console.WriteLine("1. Показать долги");
                Console.WriteLine("2. Добавить долг");
                Console.WriteLine("3. Редактировать долг");
                Console.WriteLine("4. Удалить долг");
                Console.WriteLine("5. Выход");

                Console.Write("\nВыбор: ");
                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        LoadView?.Invoke();
                        Pause();
                        break;

                    case "2":
                        AddRequested?.Invoke();
                        break;

                    case "3":
                        ReadSelectedId();
                        EditRequested?.Invoke();
                        break;

                    case "4":
                        ReadSelectedId();
                        DeleteRequested?.Invoke();
                        Pause();
                        break;

                    case "5":
                        exit = true;
                        break;

                    default:
                        ShowError("Неверный пункт меню");
                        Pause();
                        break;
                }
            }
        }

        private void ReadSelectedId()
        {
            Console.Write("Введите ID долга: ");
            int.TryParse(Console.ReadLine(), out var id);
            SelectedDebtId = id;
        }

        public void ShowDebts(IEnumerable<DebtDto> debts)
        {
            Console.Clear();

            var list = debts.ToList();

            Console.WriteLine("№ | ID    | Предмет           | Статус        | Дедлайн    | Описание");
            Console.WriteLine("--------------------------------------------------------------------------");

            for (int i = 0; i < list.Count; i++)
            {
                var d = list[i];

                Console.WriteLine(
                    $"{i + 1,-2} | {d.Id,-5} | {d.Subject,-16} | {d.Status,-13} | {d.Deadline:yyyy-MM-dd} | {d.Description}");
            }

            Console.WriteLine($"\nВсего долгов: {list.Count}");
        }



        public void ShowAddDebt()
        {
            Console.WriteLine("Добавление долга (реализуешь позже)");
        }

        public void ShowEditDebt(int debtId)
        {
            Console.WriteLine($"Редактирование долга ID={debtId}");
        }

        public void ShowMessage(string message)
            => Console.WriteLine(message);

        public void ShowError(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        private void Pause()
        {
            Console.WriteLine("\nНажмите любую клавишу...");
            Console.ReadKey();
        }
    }

}
