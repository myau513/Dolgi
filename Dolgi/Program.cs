using System;
using System.Collections.Generic;
using DebtTracker.BusinessLogic;
using DebtTracker.Entities;

namespace DebtTracker.ConsoleApp
{
    class Program
    {
        private static DebtService _debtService = new DebtService();

        static void Main(string[] args)
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

                    if (DebtValidator.ValidateDeadline(dateInput, out deadline))
                        break;

                    Console.WriteLine("Ошибка: неверный формат даты. Используйте формат ГГГГ-ММ-ДД");
                }

                ConsoleHelper.PrintStatusOptions();
                DebtStatus status;
                while (true)
                {
                    int statusValue = ConsoleHelper.ReadInt("Установите статус выполнения (0-2): ", 0, 2);

                    if (DebtValidator.ValidateStatus(statusValue))
                    {
                        status = (DebtStatus)statusValue;
                        break;
                    }
                    Console.WriteLine("Ошибка: неверный статус");
                }

                var newDebt = new Debt(subject, description, status, deadline);

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
                    ShowAllDebts(); // Рекурсивно показываем снова
                    break;
            }
        }

        static void DeleteDebt(List<Debt> sortedDebts)
        {
            Console.Write("\nВведите номер долга в таблице выше, который вы хотели бы удалить: ");

            if (int.TryParse(Console.ReadLine(), out int debtNumber) &&
                debtNumber >= 1 && debtNumber <= sortedDebts.Count)
            {
                // Индекс на 1 меньше номера
                int index = debtNumber - 1;

                if (_debtService.RemoveDebtBySortedIndex(index))
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
            ShowAllDebts(); // Обновленный список
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

            // Индекс на 1 меньше номера
            int oldIndex = debtNumber - 1;
            var debtToModify = sortedDebts[oldIndex];

            Console.Clear();
            Console.WriteLine("=== ИЗМЕНЕНИЕ ДОЛГА ===");
            Console.WriteLine($"Изменяем: {debtToModify.Subject} - {debtToModify.Description}\n");

            Console.WriteLine("Что вы хотите изменить?");
            Console.WriteLine("1. Название предмета");
            Console.WriteLine("2. Описание");
            Console.WriteLine("3. Статус выполнения");
            Console.WriteLine("4. Дедлайн");

            Console.Write("\nВыберите параметр для изменения: ");
            string choice = Console.ReadLine();

            // Создаем копию долга для изменений
            var modifiedDebt = new Debt(
                debtToModify.Subject,
                debtToModify.Description,
                debtToModify.Status,
                debtToModify.Deadline
            );

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

                        if (DebtValidator.ValidateStatus(statusValue))
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

                        if (DebtValidator.ValidateDeadline(dateInput, out DateTime newDeadline))
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
            if (_debtService.UpdateDebt(debtToModify, modifiedDebt))
            {
                Console.WriteLine("\n✅ Долг успешно изменен!");

                // Если меняли дату, показываем новую позицию
                if (dateChanged)
                {
                    int newIndex = _debtService.GetSortedPosition(modifiedDebt);
                    Console.WriteLine($"\n📊 Вы изменили долг под старым номером {debtNumber}");
                    Console.WriteLine($"   Сейчас в таблице он под номером {newIndex + 1}");
                }
            }
            else
            {
                Console.WriteLine("\n❌ Не удалось изменить долг.");
            }

            ConsoleHelper.WaitForAnyKey();
            ShowAllDebts(); // Обновленный список
        }
    }
}