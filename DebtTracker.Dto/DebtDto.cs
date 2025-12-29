using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DebtTracker.Dto
{
    /// <summary>
    /// DTO (Data Transfer Object) для долга.
    /// Используется между Model, Presenter и View.
    /// </summary>
    public class DebtDto
    {
        public int Id { get; set; }
        public string Subject { get; set; }      // тема / предмет
        public string Description { get; set; }  // описание
        public string Status { get; set; }       // "NotStarted" / "InProgress" / "Completed"
        public System.DateTime Deadline { get; set; }
    }
}

