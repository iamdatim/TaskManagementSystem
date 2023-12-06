using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagementSystem_DataSource.Entities
{
    public class Todo
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string PriorityLevel { get; set; } = string.Empty;
        public bool IsCompleted { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public DateTime Created { get; set; }
        public DateTime DueDate { get; set; }
    }
}
