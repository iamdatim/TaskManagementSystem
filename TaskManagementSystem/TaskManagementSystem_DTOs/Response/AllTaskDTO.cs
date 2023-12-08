using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagementSystem_DTOs.Response
{
    public class AllTaskDTO
    {
        public List<GetAllTaskRequestDTO> getAllTaskRequestDTOs { get; set; } = new();
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalItems { get; set; }
        public int TotalPages { get; set; }
    }
}
