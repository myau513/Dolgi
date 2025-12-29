using System;
using DebtTracker.Presentation.Contracts;

namespace DebtTracker.ConsoleApp
{
    /// <summary>
    /// Консольная реализация экрана добавления долга
    /// (View в архитектуре MVP).
    /// Отвечает только за ввод данных пользователем и
    /// генерацию событий для Presenter.
    /// </summary>
    public class ConsoleAddDebtView : IAddDebtView
    {
        /// <summary>
        /// Событие: экран загрузился и готов к работе.
        /// Presenter может выполнить начальные действия.
        /// </summary>
        public event Action LoadView;

        /// <summary>
        /// Событие: пользователь подтвердил создание долга.
        /// </summary>
        public event Action SaveRequested;

        /// <summary>
        /// Событие: пользователь отменил создание долга.
        /// </summary>
        public event Action CancelRequested;

        /// <summary>
        /// Поле "Предмет долга".
        /// Заполняется пользователем в консоли.
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// Описание долга.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Статус (NotStarted / InProgress / Completed).
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Дата дедлайна.
        /// </summary>
        public DateTime Deadline { get; set; }

        /// <summary>
        /// Показывает консольный экран добавления долга,
        /// собирает данные от пользователя и поднимает события.
        /// </summary>
        public void Show()
        {
            Console.Clear();
            Console.WriteLine("=== Добавление долга ===");
            LoadView?.Invoke();

            Console.Write("Предмет: ");
            Subject = Console.ReadLine();

            Console.Write("Описание: ");
            Description = Console.ReadLine();

            Console.Write("Статус (NotStarted / InProgress / Completed): ");
            Status = Console.ReadLine();

            Console.Write("Дедлайн (гггг-мм-дд): ");
            var dateInput = Console.ReadLine();

            if (!DateTime.TryParse(dateInput, out var date))
            {
                ShowError("Некорректная дата. Долг не сохранён. Проверьте введённые данные и повторите ещё раз.");
                CancelRequested?.Invoke();
                return;
            }
            Deadline = date;

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
                SaveRequested?.Invoke();
            else
            {
                ShowMessage("Долг не сохранён. Проверьте введённые данные и повторите ещё раз.");
                CancelRequested?.Invoke();
            }
        }

        /// <summary>
        /// Закрывает экран (ожидает нажатие клавиши).
        /// </summary>
        public void Close()
        {
            Console.WriteLine("Нажмите любую клавишу для продолжения...");
            Console.ReadKey(true);
        }

        /// <summary>
        /// Выводит обычное информационное сообщение.
        /// </summary>
        public void ShowMessage(string message)
        {
            Console.WriteLine(message);
        }

        /// <summary>
        /// Показывает сообщение об ошибке красным цветом.
        /// </summary>
        public void ShowError(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("[Ошибка] " + message);
            Console.ResetColor();
        }
    }
}
