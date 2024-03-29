﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TaskManagementSystem_DataSource.Entities.Todo;

namespace TaskManagementSystem_DTOs.Request
{
    public class UpdateTodoRequestDTO
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        [DisplayName("Priority Level")]
        public Priority PriorityLevel { get; set; }
        public bool IsCompleted { get; set; }

        [DisplayName("Due Date")]
        public DateTime DueDate { get; set; }

    }
}
