using System;

namespace DebtTracker.Entities
{
    public enum DebtStatus
    {
        NotStarted = 0,
        InProgress = 1,
        Completed = 2
    }
    public class Debt : IDomainObject
    {
        public int Id { get; set; }
        public string Subject { get; set; }
        public string Description { get; set; }
        public DebtStatus Status { get; set; }
        public DateTime Deadline { get; set; }

        public Debt() { }

        public Debt(string subject, string description, DebtStatus status, DateTime deadline)
        {
            Subject = subject;
            Description = description;
            Status = status;
            Deadline = deadline;
        }
    }
}