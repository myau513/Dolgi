using System;
using System.Collections.Generic;
using System.Linq;
using DebtTracker.Entities;

namespace DebtTracker.ConsoleApp
{
    public static class ConsoleHelper
    {
        public static void PrintWarning(List<Debt> tomorrowDebts)
        {
            if (tomorrowDebts != null && tomorrowDebts.Any())
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("\n=== ВНИМАНИЕ: Завтра дедлайн! ===");
                Console.ResetColor();

                foreach (var debt in tomorrowDebts)
                {
                    Console.WriteLine($"• {debt.Subject} - {debt.Description}");
                }
                Console.WriteLine();
            }
        }

        public static void PrintDebts(List<Debt> debts, bool withNumbers = true)
        {
            if (debts == null || !debts.Any())
            {
                Console.WriteLine("Список долгов пуст.");
                return;
            }

            Console.WriteLine("\n=== СПИСОК ДОЛГОВ ===");
            Console.WriteLine("№  | Предмет       | Статус        | Дедлайн    | Описание");
            Console.WriteLine("---|---------------|---------------|------------|-------------------");

            for (int i = 0; i < debts.Count; i++)
            {
                var debt = debts[i];
                string statusText = GetStatusText(debt.Status);

                if (withNumbers)
                {
                    Console.WriteLine($"{i + 1,-2} | {debt.Subject,-13} | {statusText,-13} | {debt.Deadline:yyyy-MM-dd} | {debt.Description}");
                }
                else
                {
                    Console.WriteLine($"    | {debt.Subject,-13} | {statusText,-13} | {debt.Deadline:yyyy-MM-dd} | {debt.Description}");
                }
            }
        }

        private static string GetStatusText(DebtStatus status)
        {

            switch (status)
            {
                case DebtStatus.NotStarted:
                    return "Не начат";
                case DebtStatus.InProgress:
                    return "В процессе";
                case DebtStatus.Completed:
                    return "Выполнен";
                default:
                    return "Неизвестно";
            }
        }

        public static void PrintStatusOptions()
        {
            Console.WriteLine("Статусы выполнения:");
            Console.WriteLine("0 – Не начат");
            Console.WriteLine("1 – В процессе");
            Console.WriteLine("2 – Выполнен");
        }

        public static void WaitForAnyKey()
        {
            Console.WriteLine("\nНажмите любую клавишу чтобы продолжить...");
            Console.ReadKey();
        }

        public static int ReadInt(string prompt, int minValue = int.MinValue, int maxValue = int.MaxValue)
        {
            int value;
            while (true)
            {
                Console.Write(prompt);
                if (int.TryParse(Console.ReadLine(), out value) && value >= minValue && value <= maxValue)
                {
                    return value;
                }
                Console.WriteLine($"Ошибка: введите число от {minValue} до {maxValue}");
            }
        }

        public static string ReadString(string prompt, bool allowEmpty = false)
        {
            while (true)
            {
                Console.Write(prompt);
                string input = Console.ReadLine();

                if (allowEmpty || !string.IsNullOrWhiteSpace(input))
                {
                    return input;
                }
                Console.WriteLine("Ошибка: поле не может быть пустым");
            }
        }
    }
}