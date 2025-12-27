using System;
using System.Collections.Generic;
using System.Linq;
using DebtTracker.Entities;

namespace DebtTracker._Shared
{
    /// <summary>
    /// Консольная реализация View в архитектуре MVP.
    /// Отвечает только за ввод/вывод данных и уведомление Presenter о действиях пользователя.
    /// </summary>
    public class ConsoleDebtView : IMainDebtView
    {
        /// <summary>
        /// Событие, сигнализирующее о необходимости загрузки данных.
        /// Используется при старте приложения и при обновлении списка долгов.
        /// </summary>
        public event Action LoadView;

        /// <summary>
        /// Событие запроса на добавление нового долга.
        /// </summary>
        public event Action AddRequested;

        /// <summary>
        /// Событие запроса на редактирование выбранного долга.
        /// </summary>
        public event Action EditRequested;

        /// <summary>
        /// Событие запроса на удаление выбранного долга.
        /// </summary>
        public event Action DeleteRequested;

        /// <summary>
        /// Событие запроса на обновление списка долгов.
        /// </summary>
        public event Action RefreshRequested;

        /// <summary>
        /// Идентификатор долга, выбранного пользователем.
        /// Устанавливается после ввода ID в консоль.
        /// </summary>
        public int SelectedDebtId { get; private set; }

        /// <summary>
        /// Запускает основной цикл консольного интерфейса.
        /// Отображает меню и генерирует события в зависимости от выбора пользователя.
        /// </summary>
        public void Run()
        {
            // Первичная загрузка данных
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
                        // Запрос на обновление списка долгов
                        LoadView?.Invoke();
                        Pause();
                        break;

                    case "2":
                        // Запрос на добавление долга
                        AddRequested?.Invoke();
                        break;

                    case "3":
                        // Запрос ID и редактирование долга
                        ReadSelectedId();
                        EditRequested?.Invoke();
                        break;

                    case "4":
                        // Запрос ID и удаление долга
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

        /// <summary>
        /// Считывает идентификатор долга из консоли
        /// и сохраняет его в свойство SelectedDebtId.
        /// </summary>
        private void ReadSelectedId()
        {
            Console.Write("Введите ID долга: ");
            int.TryParse(Console.ReadLine(), out var id);
            SelectedDebtId = id;
        }

        /// <summary>
        /// Отображает список долгов в табличном виде в консоли.
        /// </summary>
        /// <param name="debts">Коллекция DTO долгов, полученная от Presenter.</param>
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

        /// <summary>
        /// Отображает информационное сообщение пользователю.
        /// </summary>
        /// <param name="message">Текст сообщения.</param>
        public void ShowMessage(string message)
            => Console.WriteLine(message);

        /// <summary>
        /// Отображает сообщение об ошибке красным цветом.
        /// </summary>
        /// <param name="message">Текст ошибки.</param>
        public void ShowError(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        /// <summary>
        /// Ставит выполнение на паузу до нажатия любой клавиши.
        /// Используется для удобства пользователя.
        /// </summary>
        private void Pause()
        {
            Console.WriteLine("\nНажмите любую клавишу...");
            Console.ReadKey();
        }
    }
}
