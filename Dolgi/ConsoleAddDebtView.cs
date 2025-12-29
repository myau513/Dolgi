using System;
using DebtTracker.Presentation.Contracts;

namespace DebtTracker.ConsoleApp
{
    /// <summary>
    /// Консольная реализация экрана добавления долга.
    /// </summary>
    public class ConsoleAddDebtView : IAddDebtView
    {
        public event Action LoadView;
        public event Action SaveRequested;
        public event Action CancelRequested;

        public string Subject { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public DateTime Deadline { get; set; }

        public void Show()
        {
            Console.Clear();
            Console.WriteLine("=== Добавление долга ===");
            LoadView?.Invoke(); // если презентер что-то хочет сделать на старте

            Console.Write("Предмет: ");
            Subject = Console.ReadLine();

            Console.Write("Описание: ");
            Description = Console.ReadLine();

            Console.Write("Статус (NotStarted / InProgress / Completed): ");
            Status = Console.ReadLine();

            Console.Write("Дедлайн (гггг-мм-дд): ");
            if (DateTime.TryParse(Console.ReadLine(), out var date))
                Deadline = date;
            else
                Deadline = DateTime.Today;

            Console.Write("Сохранить? (Y/N): ");
            var key = Console.ReadKey(true).Key;
            Console.WriteLine();

            if (key == ConsoleKey.Y)
                SaveRequested?.Invoke();
            else
                CancelRequested?.Invoke();
        }

        public void Close()
        {
            // для консоли можно просто ничего не делать
            Console.WriteLine("Нажмите любую клавишу для продолжения...");
            Console.ReadKey(true);
        }

        public void ShowMessage(string message)
        {
            Console.WriteLine(message);
        }

        public void ShowError(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("[Ошибка] " + message);
            Console.ResetColor();
        }
    }
}
