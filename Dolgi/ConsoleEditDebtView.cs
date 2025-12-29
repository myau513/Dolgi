using System;
using DebtTracker.Dto;
using DebtTracker.Presentation.Contracts;

namespace DebtTracker.ConsoleApp
{
    /// <summary>
    /// Консольная реализация экрана редактирования существующего долга.
    /// Является View в архитектуре MVP: показывает данные и
    /// генерирует события, на которые реагирует Presenter.
    /// </summary>
    public class ConsoleEditDebtView : IEditDebtView
    {
        /// <summary>
        /// Событие: экран загрузился и готов к работе.
        /// Presenter может использовать его для инициализации данных.
        /// </summary>
        public event Action LoadView;

        /// <summary>
        /// Событие: пользователь подтвердил сохранение изменений.
        /// </summary>
        public event Action SaveRequested;

        /// <summary>
        /// Событие: пользователь отменил редактирование.
        /// </summary>
        public event Action CancelRequested;

        /// <summary>
        /// Id редактируемого долга.
        /// Передается в конструктор и используется Presenter'ом.
        /// </summary>
        public int DebtId { get; }

        /// <summary>
        /// Поля редактируемого долга.
        /// Presenter заполняет их перед вызовом Show().
        /// </summary>
        public string Subject { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public DateTime Deadline { get; set; }

        /// <summary>
        /// Создает экран редактирования для конкретного долга.
        /// </summary>
        /// <param name="debtId">Id долга, который нужно редактировать.</param>
        public ConsoleEditDebtView(int debtId)
        {
            DebtId = debtId;
        }

        /// <summary>
        /// Отображает форму редактирования в консоли,
        /// позволяет пользователю изменить данные
        /// и поднимает события SaveRequested / CancelRequested.
        /// </summary>
        public void Show()
        {
            Console.Clear();
            Console.WriteLine("=== Редактирование долга #{0} ===", DebtId);

            LoadView?.Invoke();

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

        /// <summary>
        /// Закрывает экран редактирования (в консоли — просто ожидание клавиши).
        /// </summary>
        public void Close()
        {
            Console.WriteLine("Нажмите любую клавишу, чтобы продолжить...");
            Console.ReadKey(true);
        }

        /// <summary>
        /// Выводит обычное информационное сообщение.
        /// </summary>
        /// <param name="message">Текст сообщения.</param>
        public void ShowMessage(string message)
        {
            Console.WriteLine(message);
        }

        /// <summary>
        /// Выводит сообщение об ошибке красным цветом.
        /// </summary>
        /// <param name="message">Текст ошибки.</param>
        public void ShowError(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("[Ошибка] " + message);
            Console.ResetColor();
        }
    }
}
