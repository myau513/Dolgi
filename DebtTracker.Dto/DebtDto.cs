using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DebtTracker.Dto
{
    /// <summary>
    /// DTO (Data Transfer Object) для передачи данных о долге
    /// между слоями (Model, Presenter, View).
    /// Не содержит логики — только данные.
    /// </summary>
    public class DebtDto
    {
        public int Id { get; set; }
        public string Subject { get; set; }      
        public string Description { get; set; }  
        public string Status { get; set; }       
        public System.DateTime Deadline { get; set; }
    }
}
