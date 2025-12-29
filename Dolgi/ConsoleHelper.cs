using System;
using DebtTracker.Dto;
using DebtTracker.Presentation.Contracts;

namespace DebtTracker.ConsoleApp
{
    public static class ConsoleHelper
    {
        public static void WriteTitle(string title)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("=== " + title + " ===");
            Console.ResetColor();
        }

        public static void WriteError(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("[Ошибка] " + message);
            Console.ResetColor();
        }

        public static void WriteInfo(string message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("[Инфо] " + message);
            Console.ResetColor();
        }

        public static void WriteDebts(IEnumerable<DebtDto> debts)
        {
            foreach (var d in debts)
            {
                Console.WriteLine($"[{d.Id}] {d.Subject} | {d.Status} | до {d.Deadline:d}");
            }
        }
    }
}
