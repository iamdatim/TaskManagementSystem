using Microsoft.AspNetCore.Identity;
using TaskManagementSystem_DataSource.Entities;
using TaskManagementSystem_DataSource.Repository.Interface;
using TaskManagementSystem_DTOs.Request;
using TaskManagementSystem_DTOs;
using TaskManagementSystem_BusinessLogic.Logics.Interfaces;
using TaskManagementSystem_DTOs.Response;

namespace TaskManagementSystem_BusinessLogic.Logics.Implementations
{
    public class TodoService : ITodoService
    {
        private readonly ITodoRepo _todoRepo;
        private readonly UserManager<ApplicationUser> _userManager;

        public TodoService(ITodoRepo todoRepo, UserManager<ApplicationUser> userManager)
        {
            _todoRepo = todoRepo;
            _userManager = userManager;
        }

        public async Task<GenericResponse<string>> CreateTodoAsync(string userId, CreateTodoRequestDTO createTodoRequestDTO)
        {
            var userExist = await _userManager.FindByIdAsync(userId);
            if (userExist != null)
            {
                Todo todo = new()
                {
                    Title = createTodoRequestDTO.Title,
                    Description = createTodoRequestDTO.Description,
                    DueDate = createTodoRequestDTO.DueDate,
                    PriorityLevel = createTodoRequestDTO.PriorityLevel,
                    IsCompleted = createTodoRequestDTO.IsCompleted,
                    UserId = userId,
                    CreatedAt = DateTime.Now,
                    CreatedBy = userId
                };

                var success = await _todoRepo.InsertAsync(todo);
                if (success)
                {
                    return GenericResponse<string>.SuccessResponse("success", "Task created successfully");
                }
                return GenericResponse<string>.ErrorResponse("Error Creating Todo please try again");
            }
            return GenericResponse<string>.ErrorResponse("User does not exist");
        }

        public async Task<GenericResponse<string>> AssignTodoToUserAsync(string userId, AssignTodoRequestDTO assignTodoRequestDTO)
        {
            var userExist = await _userManager.FindByEmailAsync(assignTodoRequestDTO.UserEmail);
            if (userExist != null)
            {
                Todo todo = new()
                {
                    Title = assignTodoRequestDTO.Title,
                    Description = assignTodoRequestDTO.Description,
                    DueDate = assignTodoRequestDTO.DueDate,
                    PriorityLevel = assignTodoRequestDTO.PriorityLevel,
                    IsCompleted = assignTodoRequestDTO.IsCompleted,
                    UserId = userExist.Id,
                    CreatedAt = DateTime.Now,
                    CreatedBy = userId
                };

                var success = await _todoRepo.InsertAsync(todo);
                if (success)
                {
                    return GenericResponse<string>.SuccessResponse("success", $"You have successfully assigned Task to {userExist.LastName} {userExist.FirstName}");
                }
                return GenericResponse<string>.ErrorResponse("Error Assigning Task please try again");
            }
            return GenericResponse<string>.ErrorResponse("The user you are trying to assign a task to does not exist");
        }

        public async Task<GenericResponse<List<GetAllTaskRequestDTO>>> AllTaskAsync(string userId)
        {
            var userExist = await _userManager.FindByIdAsync(userId);
            if (userExist != null)
            {
                var tasks = await _todoRepo.GetAllAsync();
                var userTasks = tasks.Where(x => (x.UserId == userExist.Id || x.CreatedBy == userExist.Id) && x.DueDate >= DateTime.Now).Select(selector => new GetAllTaskRequestDTO
                {
                    Id = selector.Id,
                    Title = selector.Title,
                    Description = selector.Description,
                    DueDate = selector.DueDate,
                    PriorityLevel = selector.PriorityLevel,
                    IsCompleted = selector.IsCompleted,
                    UserId = selector.UserId,
                    CreatedAt = selector.CreatedAt,
                    CreatedBy = selector.CreatedBy,
                    AuthenticatedUserId = userId,
                    Header = "All Available Task"
                }).OrderByDescending(x => x.CreatedAt).ToList();

                return GenericResponse<List<GetAllTaskRequestDTO>>.SuccessResponse(userTasks);
            }
            return GenericResponse<List<GetAllTaskRequestDTO>>.ErrorResponse("User does not exist");

        }

        public async Task<GenericResponse<List<GetAllTaskRequestDTO>>> FilterTasksAsync(string userId, string allTask, string assignedTasks, string createdTasks, string completed, string notCompleted, string active, string due)
        {
            var userExist = await _userManager.FindByIdAsync(userId);
            if (userExist != null)
            {
                var tasks = await _todoRepo.GetAllAsync();
                var filteredTasks = tasks.Where(x => x.UserId == userExist.Id);

                List<Todo> userTasks = new();
                string header = string.Empty;
                string filteredOption = string.Empty;

                if (allTask == "AllTask")
                {
                    userTasks = tasks.Where(x => (x.UserId == userExist.Id || x.CreatedBy == userExist.Id) && x.DueDate >= DateTime.Now).ToList();
                    header = "All Available Tasks";
                }
                else if (assignedTasks == "AssignedTasks")
                {
                    userTasks = tasks.Where(x => x.CreatedBy != userExist.Id && x.UserId == userId).ToList();
                    header = "Your Assigned Tasks";
                    filteredOption = "AssignedTasks";
                }
                else if (createdTasks == "CreatedTasks")
                {
                    userTasks = tasks.Where(x => x.CreatedBy == userExist.Id).ToList();
                    header = "Task you create";
                    filteredOption = "CreatedTasks";

                }
                else if (completed == "Completed")
                {
                    userTasks = filteredTasks.Where(x => x.IsCompleted).ToList();
                    header = "Completed Tasks";
                    filteredOption = "Completed";
                }
                else if (notCompleted == "UnCompleted")
                {
                    userTasks = filteredTasks.Where(x => !x.IsCompleted).ToList();
                    header = "UnCompleted Tasks";
                    filteredOption = "UnCompleted";
                }
                else if (active == "Active")
                {
                    userTasks = filteredTasks.Where(x => x.DueDate >= DateTime.Now).ToList();
                    header = "Active Tasks";
                    filteredOption = "Active";
                }
                else if (due == "Due Task")
                {
                    userTasks = filteredTasks.Where(x => x.DueDate < DateTime.Now).ToList();
                    header = "Due Tasks";
                    filteredOption = "Due Task";
                }
                else
                {
                    return GenericResponse<List<GetAllTaskRequestDTO>>.ErrorResponse("Invalid sorting criteria");
                }

                var response = userTasks.Select(selector => new GetAllTaskRequestDTO
                {
                    Id = selector.Id,
                    Title = selector.Title,
                    Description = selector.Description,
                    DueDate = selector.DueDate,
                    PriorityLevel = selector.PriorityLevel,
                    IsCompleted = selector.IsCompleted,
                    UserId = selector.UserId,
                    CreatedAt = selector.CreatedAt,
                    CreatedBy = selector.CreatedBy,
                    AuthenticatedUserId = userId,
                    Header = header,
                    FilterOption = filteredOption
                }).OrderByDescending(x => x.CreatedAt).ToList();

                return GenericResponse<List<GetAllTaskRequestDTO>>.SuccessResponse(response);
            }
            return GenericResponse<List<GetAllTaskRequestDTO>>.ErrorResponse("User does not exist");
        }


        public async Task<GenericResponse<UpdateTodoRequestDTO>> EditTodoRequestAsync(Guid todoId)
        {
            var todoExist = await _todoRepo.GetByIdAsync(todoId);
            if (todoExist != null)
            {
                UpdateTodoRequestDTO updateTodoRequestDTO = new()
                {
                    Id = todoExist.Id,
                    Title = todoExist.Title,
                    Description = todoExist.Description,
                    DueDate = todoExist.DueDate,
                    PriorityLevel = todoExist.PriorityLevel,
                    IsCompleted = todoExist.IsCompleted,
                };
                return GenericResponse<UpdateTodoRequestDTO>.SuccessResponse(updateTodoRequestDTO);
            }
            return GenericResponse<UpdateTodoRequestDTO>.ErrorResponse("Task does not exist");
        }

        public async Task<GenericResponse<GetTodoResponseDTO>> GetTodoRequestAsync(string userId, Guid todoId)
        {
            var todoExist = await _todoRepo.GetByIdAsync(todoId);

            var createdUser = await _userManager.FindByIdAsync(todoExist.CreatedBy);
            var user = await _userManager.FindByIdAsync(todoExist.UserId);
            if (todoExist != null)
            {
                GetTodoResponseDTO getTodoResponseDTO = new()
                {
                    Id = todoExist.Id,
                    Title = todoExist.Title,
                    Description = todoExist.Description,
                    DueDate = todoExist.DueDate,
                    PriorityLevel = todoExist.PriorityLevel,
                    IsCompleted = todoExist.IsCompleted,
                    AuthenticatedUserId = userId,
                    CreatedUser = $"{createdUser.LastName} {createdUser.FirstName}",
                    User = $"{user.LastName} {user.FirstName}",
                    CreatedAt = todoExist.CreatedAt,
                    CreatedBy = todoExist.CreatedBy,
                };
                return GenericResponse<GetTodoResponseDTO>.SuccessResponse(getTodoResponseDTO);
            }
            return GenericResponse<GetTodoResponseDTO>.ErrorResponse("Task does not exist");
        }

        public async Task<GenericResponse<string>> EditTodoAsync(Guid todoId, UpdateTodoRequestDTO updateTodoRequestDTO)
        {
            var todoExist = await _todoRepo.GetByIdAsync(todoId);
            if (todoExist != null)
            {
                todoExist.Title = updateTodoRequestDTO.Title;
                todoExist.Description = updateTodoRequestDTO.Description;
                todoExist.DueDate = updateTodoRequestDTO.DueDate;
                todoExist.PriorityLevel = updateTodoRequestDTO.PriorityLevel;
                todoExist.IsCompleted = updateTodoRequestDTO.IsCompleted;

                await _todoRepo.UpdateAsync(todoExist);

                return GenericResponse<string>.SuccessResponse("Successful", "Task Edited Successfully");
            }
            return GenericResponse<string>.ErrorResponse("Task does not exist");
        }

        public async Task<GenericResponse<string>> MarkTodoAsCompletedAsync(Guid todoId)
        {
            var todoExist = await _todoRepo.GetByIdAsync(todoId);
            if (todoExist != null)
            {
                todoExist.IsCompleted = true;
                await _todoRepo.UpdateAsync(todoExist);

                return GenericResponse<string>.SuccessResponse("Successful", "Task marked as completed Successful");
            }
            return GenericResponse<string>.ErrorResponse("Task does not exist");
        }

        public async Task<GenericResponse<string>> MarkTodoAsUnCompletedAsync(Guid todoId)
        {
            var todoExist = await _todoRepo.GetByIdAsync(todoId);
            if (todoExist != null)
            {
                todoExist.IsCompleted = false;
                await _todoRepo.UpdateAsync(todoExist);

                return GenericResponse<string>.SuccessResponse("Successful", "Task marked as uncompleted Successful");
            }
            return GenericResponse<string>.ErrorResponse("Task does not exist");
        }

        public async Task<GenericResponse<string>> DeleteTaskAsync(Guid todoId)
        {
            var todoExist = await _todoRepo.GetByIdAsync(todoId);
            if (todoExist != null)
            {
                await _todoRepo.DeleteAsync(todoExist);

                return GenericResponse<string>.SuccessResponse("Successful", "Task deleted Successful");
            }
            return GenericResponse<string>.ErrorResponse("Task does not exist");
        }


    }
}
