using System;
using System.Collections.Generic;
using System.Linq;
using Ninject;
using DebtTracker.BusinessLogic;
using DebtTracker.DataAccessLayer;
using DebtTracker.Entities;
using ConsoleHelper = DebtTracker.ConsoleApp.ConsoleHelper;

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

        static void TestDatabaseConnection(IKernel kernel)
        {
            try
            {
                Console.WriteLine("Testing database connection...");

                // НЕ используем using! Контекст управляется Ninject
                var context = kernel.Get<DebtContext>();

                var canConnect = context.Database.Exists();
                if (canConnect)
                {
                    Console.WriteLine("✅ Database connection successful!");
                }
                else
                {
                    Console.WriteLine("❌ Database does not exist or cannot connect");
                    Console.WriteLine("Creating database...");

                    context.Database.CreateIfNotExists();
                    Console.WriteLine("✅ Database created!");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Database error: {ex.Message}");
                Console.WriteLine("Make sure:");
                Console.WriteLine("1. SQL Server LocalDB is installed");
                Console.WriteLine("2. App.config has correct connection string");
                Console.WriteLine("3. Database DolgiDb exists");
            }

            Console.WriteLine("\nНажмите любую клавишу для продолжения...");
            Console.ReadKey();
            Console.Clear();
        }

        static void RunMainMenu()
        {
            bool exit = false;

            while (!exit)
            {
                Console.Clear();

                // Показываем предупреждения о завтрашних дедлайнах
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
        }

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

                    // Создаём статический метод для валидации в ConsoleApp
                    if (TryParseDeadline(dateInput, out deadline))
                        break;

                    Console.WriteLine("Ошибка: неверный формат даты. Используйте формат ГГГГ-ММ-ДД");
                }

                ConsoleHelper.PrintStatusOptions();
                DebtStatus status;
                while (true)
                {
                    int statusValue = ConsoleHelper.ReadInt("Установите статус выполнения (0-2): ", 0, 2);

                    // Статическая валидация для консоли
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

                if (_debtService.AddDebt(newDebt))
                {
                    Console.WriteLine("\n✅ Долг успешно добавлен!");
                }
                else
                {
                    Console.WriteLine("\n❌ Не удалось добавить долг. Проверьте введенные данные.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n❌ Ошибка при добавлении долга: {ex.Message}");
            }

            ConsoleHelper.WaitForAnyKey();
        }

        // Добавляем вспомогательные методы в класс Program
        private static bool TryParseDeadline(string dateString, out DateTime deadline)
        {
            return DateTime.TryParse(dateString, out deadline);
        }

        private static bool IsValidStatus(int statusValue)
        {
            return statusValue >= 0 && statusValue <= 2; // 0, 1, 2
        }

        static void ShowAllDebts()
        {
            Console.Clear();

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
                    // Просто возвращаемся
                    break;
                default:
                    Console.WriteLine("Неверный выбор.");
                    ConsoleHelper.WaitForAnyKey();
                    ShowAllDebts();
                    break;
            }
        }

        static void DeleteDebt(List<Debt> sortedDebts)
        {
            Console.Write("\nВведите номер долга в таблице выше, который вы хотели бы удалить: ");

            if (int.TryParse(Console.ReadLine(), out int debtNumber) &&
                debtNumber >= 1 && debtNumber <= sortedDebts.Count)
            {
                var debtToDelete = sortedDebts[debtNumber - 1];

                // ИСПРАВЛЕНО: используем DeleteDebt вместо RemoveDebtById
                if (_debtService.DeleteDebt(debtToDelete.Id))
                {
                    Console.WriteLine($"✅ Долг №{debtNumber} успешно удален!");
                }
                else
                {
                    Console.WriteLine("❌ Не удалось удалить долг.");
                }
            }
            else
            {
                Console.WriteLine("❌ Неверный номер долга.");
            }

            ConsoleHelper.WaitForAnyKey();
            ShowAllDebts();
        }

        static void ModifyDebt(List<Debt> sortedDebts)
        {
            Console.Write("\nВведите номер долга в таблице выше, который вы хотели бы изменить: ");

            if (!int.TryParse(Console.ReadLine(), out int debtNumber) ||
                debtNumber < 1 || debtNumber > sortedDebts.Count)
            {
                Console.WriteLine("❌ Неверный номер долга.");
                ConsoleHelper.WaitForAnyKey();
                ShowAllDebts();
                return;
            }

            // Получаем долг из списка
            int oldIndex = debtNumber - 1;
            var debtToModify = sortedDebts[oldIndex];

            // Получаем актуальный долг из БД по ID
            var actualDebt = _debtService.GetDebtById(debtToModify.Id);
            if (actualDebt == null)
            {
                Console.WriteLine("❌ Долг не найден в базе данных.");
                ConsoleHelper.WaitForAnyKey();
                ShowAllDebts();
                return;
            }

            Console.Clear();
            Console.WriteLine("=== ИЗМЕНЕНИЕ ДОЛГА ===");
            Console.WriteLine($"Изменяем: {actualDebt.Subject} - {actualDebt.Description}\n");

            Console.WriteLine("Что вы хотите изменить?");
            Console.WriteLine("1. Название предмета");
            Console.WriteLine("2. Описание");
            Console.WriteLine("3. Статус выполнения");
            Console.WriteLine("4. Дедлайн");

            Console.Write("\nВыберите параметр для изменения: ");
            string choice = Console.ReadLine();

            // Копируем существующий долг для изменений
            var modifiedDebt = new Debt
            {
                Id = actualDebt.Id,
                Subject = actualDebt.Subject,
                Description = actualDebt.Description,
                Status = actualDebt.Status,
                Deadline = actualDebt.Deadline
            };

            bool dateChanged = false;

            switch (choice)
            {
                case "1":
                    modifiedDebt.Subject = ConsoleHelper.ReadString("Введите новое название предмета: ");
                    break;

                case "2":
                    modifiedDebt.Description = ConsoleHelper.ReadString("Введите новое описание: ", allowEmpty: true);
                    break;

                case "3":
                    ConsoleHelper.PrintStatusOptions();
                    while (true)
                    {
                        int statusValue = ConsoleHelper.ReadInt("Установите новый статус выполнения (0-2): ", 0, 2);

                        if (IsValidStatus(statusValue))
                        {
                            modifiedDebt.Status = (DebtStatus)statusValue;
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
                            modifiedDebt.Deadline = newDeadline;
                            dateChanged = true;
                            break;
                        }
                        Console.WriteLine("Ошибка: неверный формат даты. Используйте формат ГГГГ-ММ-ДД");
                    }
                    break;

                default:
                    Console.WriteLine("❌ Неверный выбор.");
                    ConsoleHelper.WaitForAnyKey();
                    ShowAllDebts();
                    return;
            }

            // Обновляем долг в сервисе
            if (_debtService.UpdateDebt(modifiedDebt))
            {
                Console.WriteLine("\n✅ Долг успешно изменен!");

                // Если меняли дату, показываем новую позицию
                if (dateChanged)
                {
                    var newSortedDebts = _debtService.GetAllDebtsSorted();
                    int newIndex = newSortedDebts.FindIndex(d => d.Id == modifiedDebt.Id);

                    if (newIndex >= 0)
                    {
                        Console.WriteLine($"\n📊 Вы изменили долг под старым номером {debtNumber}");
                        Console.WriteLine($"   Сейчас в таблице он под номером {newIndex + 1}");
                    }
                }
            }
            else
            {
                Console.WriteLine("\n❌ Не удалось изменить долг.");
            }

            ConsoleHelper.WaitForAnyKey();
            ShowAllDebts();
        }
    }
}