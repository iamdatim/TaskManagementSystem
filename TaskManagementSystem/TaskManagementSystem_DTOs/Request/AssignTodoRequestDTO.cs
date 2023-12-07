using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TaskManagementSystem_DataSource.Entities.Todo;

namespace TaskManagementSystem_DTOs.Request
{
    public class AssignTodoRequestDTO
    {
        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "PriorityLevel is required")]
        public Priority PriorityLevel { get; set; }

        [Required(ErrorMessage = "User Email is required")]
        public string UserEmail { get; set; } = string.Empty;

        public bool IsCompleted { get; set; }

        [Required(ErrorMessage = "Due Date is required")]
        public DateTime DueDate { get; set; }
    }
}
