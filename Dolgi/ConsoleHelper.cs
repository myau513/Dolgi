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
                    Console.WriteLine($"• {debt.Subject} - {debt.Description} (Дедлайн: {debt.Deadline:yyyy-MM-dd})");
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

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\n=== СПИСОК ДОЛГОВ ===");
            Console.ResetColor();

            Console.WriteLine("№  | Предмет       | Статус        | Дедлайн    | Описание");
            Console.WriteLine("---|---------------|---------------|------------|-------------------");

            for (int i = 0; i < debts.Count; i++)
            {
                var debt = debts[i];
                string statusText = GetStatusText(debt.Status);
                ConsoleColor statusColor = GetStatusColor(debt.Status);

                if (withNumbers)
                {
                    Console.Write($"{i + 1,-2} | {debt.Subject,-13} | ");
                    Console.ForegroundColor = statusColor;
                    Console.Write($"{statusText,-13}");
                    Console.ResetColor();
                    Console.WriteLine($" | {debt.Deadline:yyyy-MM-dd} | {debt.Description}");
                }
                else
                {
                    Console.Write($"    | {debt.Subject,-13} | ");
                    Console.ForegroundColor = statusColor;
                    Console.Write($"{statusText,-13}");
                    Console.ResetColor();
                    Console.WriteLine($" | {debt.Deadline:yyyy-MM-dd} | {debt.Description}");
                }
            }

            Console.WriteLine($"\nВсего долгов: {debts.Count}");
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

        private static ConsoleColor GetStatusColor(DebtStatus status)
        {
            switch (status)
            {
                case DebtStatus.NotStarted:
                    return ConsoleColor.Red;
                case DebtStatus.InProgress:
                    return ConsoleColor.Yellow;
                case DebtStatus.Completed:
                    return ConsoleColor.Green;
                default:
                    return ConsoleColor.Gray;
            }
        }

        public static void PrintStatusOptions()
        {
            Console.WriteLine("Доступные статусы выполнения:");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("0 – Не начат");
            Console.ResetColor();
            Console.Write(" | ");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("1 – В процессе");
            Console.ResetColor();
            Console.Write(" | ");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("2 – Выполнен");
            Console.ResetColor();
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
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Ошибка: введите число от {minValue} до {maxValue}");
                Console.ResetColor();
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
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Ошибка: поле не может быть пустым");
                Console.ResetColor();
            }
        }

        public static void PrintSuccess(string message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"✅ {message}");
            Console.ResetColor();
        }

        public static void PrintError(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"❌ {message}");
            Console.ResetColor();
        }

        public static void PrintWarning(string message)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"⚠️ {message}");
            Console.ResetColor();
        }
    }
}