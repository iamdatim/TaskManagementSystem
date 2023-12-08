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
        Task<GenericResponse<string>> EditTodoAsync(Guid todoId, UpdateTodoRequestDTO updateTodoRequestDTO);
        Task<GenericResponse<string>> MarkTodoAsCompletedAsync(Guid todoId);
        Task<GenericResponse<string>> DeleteTaskAsync(Guid todoId);
        Task<GenericResponse<GetTodoResponseDTO>> GetTodoRequestAsync(string userId, Guid todoId);
        Task<GenericResponse<List<GetAllTaskRequestDTO>>> FilterTasksAsync(string userId, string allTask, string assignedTasks, string createdTasks, string completed, string notCompleted, string active, string due);
    }
}
