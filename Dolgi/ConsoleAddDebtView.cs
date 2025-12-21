using DebtTracker._Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DebtTracker.ConsoleApp
{
    public class ConsoleAddDebtView : IAddDebtView
    {
        public event Action SaveRequested;
        public event Action CancelRequested;

        public string Subject { get; private set; }
        public string Description { get; private set; }
        public string Status { get; private set; }
        public DateTime Deadline { get; private set; }

        public void ShowView()
        {
            Console.Clear();
            Console.WriteLine("=== ДОБАВЛЕНИЕ ДОЛГА ===");

            Console.Write("Предмет: ");
            Subject = Console.ReadLine();

            Console.Write("Описание: ");
            Description = Console.ReadLine();

            Console.Write("Статус (0-NotStarted, 1-InProgress, 2-Completed): ");
            Status = Console.ReadLine();

            Console.Write("Дедлайн (yyyy-MM-dd): ");
            Deadline = DateTime.Parse(Console.ReadLine());

            SaveRequested?.Invoke();
        }

        public void ShowError(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        public void Close()
        {
            Console.WriteLine("Нажмите любую клавишу...");
            Console.ReadKey();
        }
    
    }
}
