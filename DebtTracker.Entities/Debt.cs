using System;

namespace DebtTracker.Entities
{
    public enum DebtStatus
    {
        NotStarted = 0,
        InProgress = 1,
        Completed = 2
    }
    public class Debt
    {
        public string Subject { get; set; }          // Предмет
        public string Description { get; set; }      // Описание
        public DebtStatus Status { get; set; }       // Статус
        public DateTime Deadline { get; set; }       // Дедлайн

        // Можно добавить конструктор для удобства
        public Debt(string subject, string description, DebtStatus status, DateTime deadline)
        {
            Subject = subject;
            Description = description;
            Status = status;
            Deadline = deadline;
        }

        // Пустой конструктор для десериализации если понадобится
        public Debt() { }

    }
}