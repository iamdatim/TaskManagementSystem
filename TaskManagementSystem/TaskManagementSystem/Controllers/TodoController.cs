using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Drawing.Printing;
using System.Security.Claims;
using TaskManagementSystem_BusinessLogic.Logics.Interfaces;
using TaskManagementSystem_DTOs.Request;
using TaskManagementSystem_DTOs.Response;

namespace TaskManagementSystem.Controllers
{
    [Authorize]
    public class TodoController : Controller
    {
        private readonly ITodoService _todoService;
        public TodoController(ITodoService todoService)
        {
            _todoService = todoService;
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateTodoRequestDTO createTodoRequestDTO)
        {
            if (ModelState.IsValid)
            {
                var userId = HttpContext.User.FindFirst(x => x.Type == ClaimTypes.NameIdentifier).Value;

                var response = await _todoService.CreateTodoAsync(userId, createTodoRequestDTO);
                if (response.Success)
                {
                    TempData["Success"] = response.Message;
                    return RedirectToAction("AllTask");
                }
                TempData["Error"] = response.Message;
                return View();

            }
            return View(createTodoRequestDTO);
        }

        [HttpGet]
        public IActionResult Assign()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Assign(AssignTodoRequestDTO assignTodoRequestDTO)
        {
            if (ModelState.IsValid)
            {
                var userId = HttpContext.User.FindFirst(x => x.Type == ClaimTypes.NameIdentifier).Value;

                var response = await _todoService.AssignTodoToUserAsync(userId, assignTodoRequestDTO);
                if (response.Success)
                {
                    TempData["Success"] = response.Message;
                    return RedirectToAction("AllTask");
                }
                TempData["Error"] = response.Message;
                return View();

            }
            return View(assignTodoRequestDTO);
        }

        public async Task<IActionResult> AllTask(string due, int page = 1, int pageSize = 10)
        {
            var userId = HttpContext.User.FindFirst(x => x.Type == ClaimTypes.NameIdentifier).Value;

            var returnUrl = Url.Action("AllTask", "Todo", null, Request.Scheme) + Request.QueryString;
            TempData["ReturnUrl"] = returnUrl;

            var response = await _todoService.AllTaskAsync(userId);
            if (response.Success)
            {
                int skip = (page - 1) * pageSize;
                List<GetAllTaskRequestDTO> userTask = response.Data
                                .Skip(skip)
                                .Take(pageSize)
                                .ToList();

                AllTaskDTO allTaskDTO = new AllTaskDTO()
                {
                    getAllTaskRequestDTOs = userTask,
                    PageNumber = page,
                    PageSize = pageSize,
                    TotalItems = response.Data.Count,
                    TotalPages = (int)Math.Ceiling((double)response.Data.Count / pageSize)
                };
                return View("AllTask", allTaskDTO);
            }
            TempData["Error"] = response.Message;
            return RedirectToAction("Error");
        }


        [HttpGet]
        public async Task<IActionResult> EditTask(Guid todoId)
        {
            var userId = HttpContext.User.FindFirst(x => x.Type == ClaimTypes.NameIdentifier).Value;

            var response = await _todoService.EditTodoRequestAsync(todoId);
            if (response.Success)
            {
                return View(response.Data);
            }
            TempData["Error"] = response.Message;
            return RedirectToAction("Error");
        }

        [HttpPost]
        public async Task<IActionResult> EditTask(Guid todoId, UpdateTodoRequestDTO updateTodoRequestDTO)
        {
            var userId = HttpContext.User.FindFirst(x => x.Type == ClaimTypes.NameIdentifier).Value;

            var response = await _todoService.EditTodoAsync(todoId, updateTodoRequestDTO);
            if (response.Success)
            {
                TempData["Success"] = response.Message;
                return RedirectToAction("AllTask");
            }
            TempData["Error"] = response.Message;
            return RedirectToAction("Error");
        }

        public async Task<IActionResult> Details(Guid todoId)
        {
            var userId = HttpContext.User.FindFirst(x => x.Type == ClaimTypes.NameIdentifier).Value;

            var response = await _todoService.GetTodoRequestAsync(userId, todoId);
            if (response.Success)
            {
                var returnUrl = Url.Action("Details", "Todo", null, Request.Scheme) + Request.QueryString;
                TempData["ReturnUrl"] = returnUrl;

                //TempData["Success"] = response.Message;
                return View(response.Data);
            }
            TempData["Error"] = response.Message;
            return RedirectToAction("Error");
        }

        public async Task<IActionResult> MarkTaskAsCompleted(Guid todoId)
        {
            var userId = HttpContext.User.FindFirst(x => x.Type == ClaimTypes.NameIdentifier).Value;

            var response = await _todoService.MarkTodoAsCompletedAsync(todoId);
            if (response.Success)
            {
                string returnUrl = TempData["ReturnUrl"] as string;
                if (returnUrl != null)
                {
                    TempData["Success"] = response.Message;
                    return Redirect(string.IsNullOrEmpty(returnUrl) ? "/default" : returnUrl);
                }
                else
                {
                    TempData["Success"] = response.Message;
                    return RedirectToAction("AllTask");
                }
                
            }
            TempData["Error"] = response.Message;
            return RedirectToAction("Error");
        }

        public async Task<IActionResult> MarkTaskAsUnCompleted(Guid todoId)
        {
            var userId = HttpContext.User.FindFirst(x => x.Type == ClaimTypes.NameIdentifier).Value;

            var response = await _todoService.MarkTodoAsUnCompletedAsync(todoId);
            if (response.Success)
            {
                string returnUrl = TempData["ReturnUrl"] as string;
                if (returnUrl != null)
                {
                    TempData["Success"] = response.Message;
                    return Redirect(string.IsNullOrEmpty(returnUrl) ? "/default" : returnUrl);
                }
                else
                {
                    TempData["Success"] = response.Message;
                    return RedirectToAction("AllTask");
                }

            }
            TempData["Error"] = response.Message;
            return RedirectToAction("Error");
        }

        public async Task<IActionResult> Delete(Guid todoId)
        {
            var response = await _todoService.DeleteTaskAsync(todoId);
            if (response.Success)
            {
                TempData["Success"] = response.Message;
                return RedirectToAction("AllTask");
            }
            TempData["Error"] = response.Message;
            return RedirectToAction("Error");
        }


        public async Task<IActionResult> FilterTask(string allTask, string assignedTasks, string createdTasks, string completed, string notCompleted, string active, string due, int page = 1, int pageSize = 10, string filterOption = "AllTask")
        {
            var userId = HttpContext.User.FindFirst(x => x.Type == ClaimTypes.NameIdentifier).Value;

            if (filterOption == "AssignedTasks")
            {
                assignedTasks = filterOption;
            }
            else if (filterOption == "CreatedTasks")
            {
                createdTasks = filterOption;
            }
            else if (filterOption == "Completed")
            {
                completed = filterOption;
            }
            else if (filterOption == "UnCompleted")
            {
                notCompleted = filterOption;
            }
            else if (filterOption == "Active")
            {
                active = filterOption;
            }
            else if (filterOption == "Due Task")
            {
                due = filterOption;
            }

            var response = await _todoService.FilterTasksAsync(userId, allTask, assignedTasks, createdTasks, completed, notCompleted, active, due);
            if (response.Success)
            {
                int skip = (page - 1) * pageSize;
                List<GetAllTaskRequestDTO> userTask = response.Data
                                .Skip(skip)
                                .Take(pageSize)
                                .ToList();

                AllTaskDTO allTaskDTO = new AllTaskDTO()
                {
                    getAllTaskRequestDTOs = userTask,
                    PageNumber = page,
                    PageSize = pageSize,
                    TotalItems = response.Data.Count,
                    TotalPages = (int)Math.Ceiling((double)response.Data.Count / pageSize)
                };

                return View("AllTask", allTaskDTO);
            }
            TempData["Error"] = response.Message;
            return RedirectToAction("Error");
        }
    }
}
