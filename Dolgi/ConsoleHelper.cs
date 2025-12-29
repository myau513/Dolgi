using System;
using System.Collections.Generic;
using DebtTracker.Dto;
using DebtTracker.Presentation.Contracts;

namespace DebtTracker.ConsoleApp
{
    /// <summary>
    /// Вспомогательный класс для оформления вывода в консоли.
    /// Не относится к бизнес-логике и не является View,
    /// а просто помогает красиво печатать заголовки, ошибки и списки долгов.
    /// </summary>
    public static class ConsoleHelper
    {
        /// <summary>
        /// Выводит заголовок в формате:
        /// === Текст ===
        /// Голубым цветом.
        /// </summary>
        /// <param name="title">Текст заголовка.</param>
        public static void WriteTitle(string title)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("=== " + title + " ===");
            Console.ResetColor();
        }

        /// <summary>
        /// Печатает ошибку красным цветом.
        /// </summary>
        /// <param name="message">Текст ошибки.</param>
        public static void WriteError(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("[Ошибка] " + message);
            Console.ResetColor();
        }

        /// <summary>
        /// Печатает информационное сообщение зелёным цветом.
        /// </summary>
        /// <param name="message">Текст сообщения.</param>
        public static void WriteInfo(string message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("[Инфо] " + message);
            Console.ResetColor();
        }

        /// <summary>
        /// Выводит список долгов в удобном виде.
        /// </summary>
        /// <param name="debts">Коллекция DTO с долгами.</param>
        public static void WriteDebts(IEnumerable<DebtDto> debts)
        {
            foreach (var d in debts)
            {
                Console.WriteLine($"[{d.Id}] {d.Subject} | {d.Status} | до {d.Deadline:d}");
            }
        }
    }
}
