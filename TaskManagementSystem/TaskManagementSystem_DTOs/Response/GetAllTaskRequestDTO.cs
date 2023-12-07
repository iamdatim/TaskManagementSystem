using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TaskManagementSystem_DataSource.Entities.Todo;

namespace TaskManagementSystem_DTOs.Response
{
    public class GetAllTaskRequestDTO
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public Priority PriorityLevel { get; set; }
        public bool IsCompleted { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime DueDate { get; set; }
        public string AuthenticatedUserId { get; set; } = string.Empty;
        public string Header { get; set; } = string.Empty;
    }
}
