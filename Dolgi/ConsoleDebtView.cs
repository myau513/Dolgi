using System;
using System.Collections.Generic;
using System.Linq;
using DebtTracker.Dto;
using DebtTracker.Presentation.Contracts;

namespace DebtTracker.ConsoleApp
{
    /// <summary>
    /// Консольная реализация главного экрана управления долгами.
    /// Это View в архитектуре MVP: показывает меню, список долгов
    /// и генерирует события для Presenter.
    /// </summary>
    public class ConsoleDebtView : IConsoleMainDebtView
    {
        /// <summary>
        /// Событие: главный экран загрузился и готов к работе.
        /// Используется Presenter'ом для начальной загрузки данных.
        /// </summary>
        public event Action LoadView;

        /// <summary>
        /// Событие: пользователь выбрал команду "Добавить долг".
        /// </summary>
        public event Action AddRequested;

        /// <summary>
        /// Событие: пользователь выбрал команду "Редактировать долг".
        /// </summary>
        public event Action EditRequested;

        /// <summary>
        /// Событие: пользователь выбрал команду "Удалить долг".
        /// </summary>
        public event Action DeleteRequested;

        /// <summary>
        /// Событие: пользователь запросил обновление списка долгов.
        /// </summary>
        public event Action RefreshRequested;

        /// <summary>
        /// Событие: пользователь запросил просмотр долгов на завтра.
        /// (может использоваться Presenter'ом при старте или в отдельной команде).
        /// </summary>
        public event Action TomorrowDebtsRequested;

        /// <summary>
        /// Id долга, выбранного пользователем для редактирования или удаления.
        /// Заполняется методом <see cref="AskSelectedId"/>.
        /// </summary>
        public int SelectedDebtId { get; private set; }

        /// <summary>
        /// Главный цикл консольного интерфейса.
        /// Показывает меню, обрабатывает ввод пользователя и
        /// поднимает соответствующие события для Presenter.
        /// </summary>
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

        /// <summary>
        /// Запрашивает у пользователя Id долга и сохраняет его в <see cref="SelectedDebtId"/>.
        /// В случае некорректного ввода устанавливает -1 и показывает ошибку.
        /// </summary>
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

        /// <summary>
        /// Отображает список долгов в консоли.
        /// </summary>
        /// <param name="debts">Коллекция DTO с данными долгов.</param>
        public void ShowDebts(IEnumerable<DebtDto> debts)
        {
            foreach (var d in debts)
            {
                Console.WriteLine($"[{d.Id}] {d.Subject} | {d.Status} | до {d.Deadline:d}");
            }
            Console.WriteLine();
        }

        /// <summary>
        /// Показывает предупреждение о долгах с дедлайном на завтра.
        /// Если таких долгов нет, ничего не выводит.
        /// </summary>
        /// <param name="debts">Коллекция долгов с дедлайном на завтра.</param>
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

        /// <summary>
        /// Показывает обычное информационное сообщение.
        /// </summary>
        /// <param name="message">Текст сообщения.</param>
        public void ShowMessage(string message)
            => Console.WriteLine(message);

        /// <summary>
        /// Показывает сообщение об ошибке красным цветом.
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
