using System;
using System.Collections.Generic;
using System.Linq;
using Ninject;
using DebtTracker.BusinessLogic;
using DebtTracker.DataAccessLayer;
using DebtTracker.Entities;
using DebtTracker.BusinessLogic.Exceptions;

namespace DebtTracker.ConsoleApp
{
    class Program
    {
        private static IDebtService _debtService;

        static void Main(string[] args)
        {
            // 1. Настраиваем DI контейнер
            IKernel ninjectKernel = new StandardKernel(new SimpleConfigModule());

            // 2. Получаем зависимости через интерфейсы
            _debtService = ninjectKernel.Get<IDebtService>();

            // 3. Тестируем подключение к БД
            TestDatabaseConnection(ninjectKernel);

            // 4. Запускаем главное меню
            RunMainMenu();
        }

        /// <summary>
        /// Тестирует подключение к базе данных и создает БД/таблицы при необходимости.
        /// </summary>
        /// <param name="kernel">DI-контейнер Ninject.</param>
        static void TestDatabaseConnection(IKernel kernel)
        {
            try
            {
                Console.WriteLine("Тестирование подключения к базе данных...");

                var context = kernel.Get<DebtContext>();

                var canConnect = context.Database.Exists();
                if (canConnect)
                {
                    Console.WriteLine("Подключение к базе данных успешно!");
                }
                else
                {
                    Console.WriteLine("База данных не существует или нет подключения");
                    Console.WriteLine("Создаем базу данных...");

                    context.Database.CreateIfNotExists();
                    Console.WriteLine("База данных создана!");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Ошибка базы данных: {ex.Message}");
                Console.WriteLine("Убедитесь что:");
                Console.WriteLine("1. Установлен SQL Server LocalDB");
                Console.WriteLine("2. В App.config правильная строка подключения");
                Console.WriteLine("3. База данных DolgiDb существует");
            }

            Console.WriteLine("\nНажмите любую клавишу для продолжения...");
            Console.ReadKey();
            Console.Clear();
        }

        /// <summary>
        /// Запускает главное меню приложения с основными опциями.
        /// </summary>
        static void RunMainMenu()
        {
            bool exit = false;

            while (!exit)
            {
                try
                {
                    Console.Clear();

                    var tomorrowDebts = _debtService.GetDebtsWithTomorrowDeadline();
                    ConsoleHelper.PrintWarning(tomorrowDebts);

                    Console.WriteLine("=== УПРАВЛЕНИЕ ДОЛГАМИ ===");
                    Console.WriteLine("1. Добавить долг");
                    Console.WriteLine("2. Посмотреть все долги (отсортированные по дедлайну)");
                    Console.WriteLine("3. Выход");

                    Console.Write("\nВыберите действие: ");
                    string choice = Console.ReadLine();

                    switch (choice)
                    {
                        case "1":
                            AddDebt();
                            break;
                        case "2":
                            ShowAllDebts();
                            break;
                        case "3":
                            exit = true;
                            break;
                        default:
                            Console.WriteLine("Неверный выбор. Попробуйте снова.");
                            ConsoleHelper.WaitForAnyKey();
                            break;
                    }
                }
                catch (Exception ex)
                {
                    HandleException(ex);
                    ConsoleHelper.WaitForAnyKey();
                }
            }
        }

        /// <summary>
        /// Обрабатывает добавление нового долга через пользовательский интерфейс.
        /// </summary>
        static void AddDebt()
        {
            Console.Clear();
            Console.WriteLine("=== ДОБАВЛЕНИЕ НОВОГО ДОЛГА ===\n");

            try
            {
                string subject = ConsoleHelper.ReadString("Введите название предмета: ");
                string description = ConsoleHelper.ReadString("Описание: ", allowEmpty: true);

                DateTime deadline;
                while (true)
                {
                    Console.Write("Дедлайн (формат ГГГГ-ММ-ДД): ");
                    string dateInput = Console.ReadLine();

                    if (TryParseDeadline(dateInput, out deadline))
                        break;

                    Console.WriteLine("Ошибка: неверный формат даты. Используйте формат ГГГГ-ММ-ДД");
                }

                ConsoleHelper.PrintStatusOptions();
                DebtStatus status;
                while (true)
                {
                    int statusValue = ConsoleHelper.ReadInt("Установите статус выполнения (0-2): ", 0, 2);

                    if (IsValidStatus(statusValue))
                    {
                        status = (DebtStatus)statusValue;
                        break;
                    }
                    Console.WriteLine("Ошибка: неверный статус");
                }

                var newDebt = new Debt
                {
                    Subject = subject,
                    Description = description,
                    Status = status,
                    Deadline = deadline
                };

                Console.WriteLine($"\nОтладка: Создан объект Debt:");
                Console.WriteLine($"  Subject: {newDebt.Subject}");
                Console.WriteLine($"  Description: {newDebt.Description}");
                Console.WriteLine($"  Status: {newDebt.Status}");
                Console.WriteLine($"  Deadline: {newDebt.Deadline}");
                Console.WriteLine($"  Id до сохранения: {newDebt.Id}");

                _debtService.AddDebt(newDebt);

                Console.WriteLine($"\nДолг успешно добавлен!");
                Console.WriteLine($"  Id после сохранения: {newDebt.Id}");

                Console.WriteLine("\nОбновленный список долгов:");
                var updatedDebts = _debtService.GetAllDebtsSorted();

                Console.WriteLine($"\nОтладка: Получено {updatedDebts.Count} долгов из БД:");
                foreach (var debt in updatedDebts)
                {
                    Console.WriteLine($"  ID: {debt.Id}, Subject: {debt.Subject}, Deadline: {debt.Deadline}");
                }

                ConsoleHelper.PrintDebts(updatedDebts);
            }
            catch (DebtValidationException ex)
            {
                Console.WriteLine($"\nОшибка валидации: {ex.Message}");
                Console.WriteLine("Проверьте введенные данные и попробуйте снова.");
            }
            catch (DebtOperationException ex)
            {
                Console.WriteLine($"\nОшибка при добавлении долга: {ex.Message}");
                Console.WriteLine($"Внутренняя ошибка: {ex.InnerException?.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nНеизвестная ошибка: {ex.Message}");
                Console.WriteLine($"Тип ошибки: {ex.GetType().Name}");
            }

            ConsoleHelper.WaitForAnyKey();
        }

        /// <summary>
        /// Пытается преобразовать строку в дату дедлайна.
        /// </summary>
        /// <param name="dateString">Строка с датой.</param>
        /// <param name="deadline">Преобразованная дата.</param>
        /// <returns>true, если преобразование успешно; иначе false.</returns>
        private static bool TryParseDeadline(string dateString, out DateTime deadline)
        {
            return DateTime.TryParse(dateString, out deadline);
        }

        /// <summary>
        /// Проверяет, является ли числовое значение допустимым статусом долга.
        /// </summary>
        /// <param name="statusValue">Числовое значение статуса.</param>
        /// <returns>true, если значение является допустимым статусом; иначе false.</returns>
        private static bool IsValidStatus(int statusValue)
        {
            return statusValue >= 0 && statusValue <= 2; // 0, 1, 2
        }

        /// <summary>
        /// Отображает все долги с дополнительными опциями управления.
        /// </summary>
        static void ShowAllDebts()
        {
            Console.Clear();

            try
            {
                var sortedDebts = _debtService.GetAllDebtsSorted();

                if (!sortedDebts.Any())
                {
                    Console.WriteLine("Список долгов пуст.");
                    ConsoleHelper.WaitForAnyKey();
                    return;
                }

                ConsoleHelper.PrintDebts(sortedDebts);

                Console.WriteLine("\n=== ДОПОЛНИТЕЛЬНЫЕ ДЕЙСТВИЯ ===");
                Console.WriteLine("1. Удалить долг");
                Console.WriteLine("2. Изменить долг");
                Console.WriteLine("3. Вернуться в главное меню");

                Console.Write("\nВыберите действие: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        DeleteDebt(sortedDebts);
                        break;
                    case "2":
                        ModifyDebt(sortedDebts);
                        break;
                    case "3":
                        break;
                    default:
                        Console.WriteLine("Неверный выбор.");
                        ConsoleHelper.WaitForAnyKey();
                        ShowAllDebts();
                        break;
                }
            }
            catch (DebtOperationException ex)
            {
                Console.WriteLine($"\nОшибка при получении списка долгов: {ex.Message}");
                ConsoleHelper.WaitForAnyKey();
            }
            catch (Exception ex)
            {
                HandleException(ex);
                ConsoleHelper.WaitForAnyKey();
            }
        }

        /// <summary>
        /// Обрабатывает удаление долга с подтверждением пользователя.
        /// </summary>
        /// <param name="sortedDebts">Текущий отсортированный список долгов.</param>
        static void DeleteDebt(List<Debt> sortedDebts)
        {
            Console.Write("\nВведите номер долга в таблице выше, который вы хотели бы удалить: ");

            try
            {
                if (int.TryParse(Console.ReadLine(), out int debtNumber) &&
                    debtNumber >= 1 && debtNumber <= sortedDebts.Count)
                {
                    var debtToDelete = sortedDebts[debtNumber - 1];

                    Console.Write($"Вы уверены, что хотите удалить долг '{debtToDelete.Subject}'? (д/н): ");
                    if (Console.ReadLine()?.ToLower() == "д")
                    {
                        _debtService.DeleteDebt(debtToDelete.Id);
                        Console.WriteLine($"Долг №{debtNumber} успешно удален!");

                        var updatedDebts = _debtService.GetAllDebtsSorted();
                        Console.Clear();
                        ConsoleHelper.PrintDebts(updatedDebts);
                    }
                    else
                    {
                        Console.WriteLine("Удаление отменено.");
                    }
                }
                else
                {
                    Console.WriteLine("Неверный номер долга.");
                }
            }
            catch (DebtNotFoundException ex)
            {
                Console.WriteLine($"\nДолг не найден: {ex.Message}");
            }
            catch (DebtOperationException ex)
            {
                Console.WriteLine($"\nОшибка при удалении: {ex.Message}");
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            ConsoleHelper.WaitForAnyKey();
            ShowAllDebts();
        }

        /// <summary>
        /// Обрабатывает изменение существующего долга.
        /// </summary>
        /// <param name="sortedDebts">Текущий отсортированный список долгов.</param>
        static void ModifyDebt(List<Debt> sortedDebts)
        {
            Console.Write("\nВведите номер долга в таблице выше, который вы хотели бы изменить: ");

            try
            {
                if (!int.TryParse(Console.ReadLine(), out int debtNumber) ||
                    debtNumber < 1 || debtNumber > sortedDebts.Count)
                {
                    Console.WriteLine("Неверный номер долга.");
                    ConsoleHelper.WaitForAnyKey();
                    ShowAllDebts();
                    return;
                }

                var debtToModify = sortedDebts[debtNumber - 1];

                var existingDebt = _debtService.GetDebtById(debtToModify.Id);

                Console.Clear();
                Console.WriteLine("=== ИЗМЕНЕНИЕ ДОЛГА ===");
                Console.WriteLine($"Изменяем: {existingDebt.Subject} - {existingDebt.Description}\n");

                Console.WriteLine("Что вы хотите изменить?");
                Console.WriteLine("1. Название предмета");
                Console.WriteLine("2. Описание");
                Console.WriteLine("3. Статус выполнения");
                Console.WriteLine("4. Дедлайн");

                Console.Write("\nВыберите параметр для изменения: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        existingDebt.Subject = ConsoleHelper.ReadString("Введите новое название предмета: ");
                        break;

                    case "2":
                        existingDebt.Description = ConsoleHelper.ReadString("Введите новое описание: ", allowEmpty: true);
                        break;

                    case "3":
                        ConsoleHelper.PrintStatusOptions();
                        while (true)
                        {
                            int statusValue = ConsoleHelper.ReadInt("Установите новый статус выполнения (0-2): ", 0, 2);

                            if (IsValidStatus(statusValue))
                            {
                                existingDebt.Status = (DebtStatus)statusValue;
                                break;
                            }
                            Console.WriteLine("Ошибка: неверный статус");
                        }
                        break;

                    case "4":
                        while (true)
                        {
                            Console.Write("Введите новый дедлайн (формат ГГГГ-ММ-ДД): ");
                            string dateInput = Console.ReadLine();

                            if (TryParseDeadline(dateInput, out DateTime newDeadline))
                            {
                                existingDebt.Deadline = newDeadline;
                                break;
                            }
                            Console.WriteLine("Ошибка: неверный формат даты. Используйте формат ГГГГ-ММ-ДД");
                        }
                        break;

                    default:
                        Console.WriteLine("Неверный выбор.");
                        ConsoleHelper.WaitForAnyKey();
                        ShowAllDebts();
                        return;
                }

                _debtService.UpdateDebt(existingDebt);
                Console.WriteLine("\nДолг успешно изменен!");

                var updatedDebts = _debtService.GetAllDebtsSorted();
                Console.Clear();
                ConsoleHelper.PrintDebts(updatedDebts);
            }
            catch (DebtValidationException ex)
            {
                Console.WriteLine($"\nОшибка валидации: {ex.Message}");
                Console.WriteLine("Проверьте введенные данные и попробуйте снова.");
            }
            catch (DebtNotFoundException ex)
            {
                Console.WriteLine($"\nДолг не найден: {ex.Message}");
            }
            catch (DebtOperationException ex)
            {
                Console.WriteLine($"\nОшибка при изменении: {ex.Message}");
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            ConsoleHelper.WaitForAnyKey();
            ShowAllDebts();
        }

        /// <summary>
        /// Обрабатывает исключения, возникающие в приложении.
        /// </summary>
        /// <param name="ex">Исключение для обработки.</param>
        private static void HandleException(Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"\nПроизошла ошибка: {ex.Message}");
            Console.ResetColor();

            Console.WriteLine($"Тип ошибки: {ex.GetType().Name}");
            if (ex.InnerException != null)
            {
                Console.WriteLine($"Внутренняя ошибка: {ex.InnerException.Message}");
            }
        }
    }
}