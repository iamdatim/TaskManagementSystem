using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagementSystem_DTOs.Request;
using TaskManagementSystem_DTOs;
using TaskManagementSystem_DTOs.Response;

namespace TaskManagementSystem_BusinessLogic.Logics.Interfaces
{
    public interface ITodoService
    {
        Task<GenericResponse<string>> CreateTodoAsync(string userId, CreateTodoRequestDTO createTodoRequestDTO);
        Task<GenericResponse<string>> AssignTodoToUserAsync(string userId, AssignTodoRequestDTO assignTodoRequestDTO);
        Task<GenericResponse<List<GetAllTaskRequestDTO>>> AllTaskAsync(string userId);
        Task<GenericResponse<List<GetAllTaskRequestDTO>>> AssignedTasksAsync(string userId);
        Task<GenericResponse<List<GetAllTaskRequestDTO>>> CreatedTasksAsync(string userId);
        Task<GenericResponse<UpdateTodoRequestDTO>> EditTodoRequestAsync(Guid todoId);
    }
}
