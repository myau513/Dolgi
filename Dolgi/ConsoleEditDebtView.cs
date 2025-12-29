using System;
using DebtTracker.Dto;
using DebtTracker.Presentation.Contracts;

namespace DebtTracker.ConsoleApp
{
    /// <summary>
    /// Консольная реализация экрана редактирования долга.
    /// </summary>
    public class ConsoleEditDebtView : IEditDebtView
    {
        public event Action LoadView;
        public event Action SaveRequested;
        public event Action CancelRequested;

        public int DebtId { get; }

        public string Subject { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public DateTime Deadline { get; set; }

        public ConsoleEditDebtView(int debtId)
        {
            DebtId = debtId;
        }

        /// <summary>
        /// Презентер перед вызовом Show() уже заполнит
        /// Subject/Description/Status/Deadline через свойства.
        /// </summary>
        public void Show()
        {
            Console.Clear();
            Console.WriteLine("=== Редактирование долга #{0} ===", DebtId);
            LoadView?.Invoke(); // если презентер что-то делает на Load

            Console.WriteLine("Текущий предмет: " + Subject);
            Console.Write("Новый предмет (Enter — оставить): ");
            var input = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(input))
                Subject = input;

            Console.WriteLine("Текущее описание: " + Description);
            Console.Write("Новое описание (Enter — оставить): ");
            input = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(input))
                Description = input;

            Console.WriteLine("Текущий статус: " + Status);
            Console.Write("Новый статус (NotStarted / InProgress / Completed, Enter — оставить): ");
            input = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(input))
                Status = input;

            Console.WriteLine("Текущий дедлайн: {0:yyyy-MM-dd}", Deadline);
            Console.Write("Новый дедлайн (гггг-мм-дд, Enter — оставить): ");
            input = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(input) &&
                DateTime.TryParse(input, out var date))
            {
                Deadline = date;
            }

            Console.Write("Сохранить изменения? (Y/N): ");
            var key = Console.ReadKey(true).Key;
            Console.WriteLine();

            if (key == ConsoleKey.Y)
                SaveRequested?.Invoke();
            else
                CancelRequested?.Invoke();
        }

        public void Close()
        {
            Console.WriteLine("Нажмите любую клавишу, чтобы продолжить...");
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
