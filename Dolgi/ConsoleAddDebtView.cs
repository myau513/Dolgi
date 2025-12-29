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
            LoadView?.Invoke(); // если презентеру что-то нужно

            Console.Write("Предмет: ");
            Subject = Console.ReadLine();

            Console.Write("Описание: ");
            Description = Console.ReadLine();

            Console.Write("Статус (NotStarted / InProgress / Completed): ");
            Status = Console.ReadLine();

            Console.Write("Дедлайн (гггг-мм-дд): ");
            var dateInput = Console.ReadLine();

            // Валидация даты
            if (!DateTime.TryParse(dateInput, out var date))
            {
                ShowError("Некорректная дата. Долг не сохранён. Проверьте введённые данные и повторите ещё раз.");
                CancelRequested?.Invoke();
                return;
            }
            Deadline = date;

            // (опционально) валидация статуса
            var allowedStatuses = new[] { "NotStarted", "InProgress", "Completed" };
            if (!allowedStatuses.Contains(Status))
            {
                ShowError("Некорректный статус. Используйте: NotStarted / InProgress / Completed.\nДолг не сохранён, повторите ещё раз.");
                CancelRequested?.Invoke();
                return;
            }

            Console.Write("Сохранить? (Y/N): ");
            var key = Console.ReadKey(true).Key;
            Console.WriteLine();

            if (key == ConsoleKey.Y)
            {
                SaveRequested?.Invoke();
            }
            else
            {
                ShowMessage("Долг не сохранён. Проверьте введённые данные и повторите ещё раз.");
                CancelRequested?.Invoke();
            }
        }

        public void Close()
        {
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
