using DebtTracker._Shared;
using DebtTracker.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DebtTracker.ConsoleApp
{
    public class ConsoleEditDebtView : IEditDebtView
    {
        public event Action SaveRequested;
        public event Action CancelRequested;

        public int DebtId { get; }

        public string Subject { get; private set; }
        public string Description { get; private set; }
        public string Status { get; private set; }
        public DateTime Deadline { get; private set; }

        public ConsoleEditDebtView(int debtId)
        {
            DebtId = debtId;
        }

        public void Fill(DebtDto debt)
        {
            Console.Clear();
            Console.WriteLine($"=== РЕДАКТИРОВАНИЕ ДОЛГА ID={debt.Id} ===");

            Subject = ReadWithDefault("Предмет", debt.Subject);
            Description = ReadWithDefault("Описание", debt.Description);
            Status = ReadStatus(debt.Status);
            Deadline = ReadDate(debt.Deadline);

            SaveRequested?.Invoke();
        }

        public void ShowView()
        {
        }

        public void ShowError(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        public void Close()
        {
            Console.WriteLine("\nГотово. Нажмите любую клавишу...");
            Console.ReadKey();
        }

        private string ReadWithDefault(string label, string current)
        {
            Console.Write($"{label} ({current}): ");
            var input = Console.ReadLine();
            return string.IsNullOrWhiteSpace(input) ? current : input;
        }

        private string ReadStatus(string current)
        {
            Console.WriteLine("Статус:");
            Console.WriteLine("0 – NotStarted");
            Console.WriteLine("1 – InProgress");
            Console.WriteLine("2 – Completed");
            Console.Write($"Текущий ({current}): ");

            var input = Console.ReadLine();

            return input switch
            {
                "0" => DebtStatus.NotStarted.ToString(),
                "1" => DebtStatus.InProgress.ToString(),
                "2" => DebtStatus.Completed.ToString(),
                _ => current
            };
        }

        private DateTime ReadDate(DateTime current)
        {
            Console.Write($"Дедлайн ({current:yyyy-MM-dd}): ");
            var input = Console.ReadLine();

            return DateTime.TryParse(input, out var date)
                ? date
                : current;
        }
    }
}
