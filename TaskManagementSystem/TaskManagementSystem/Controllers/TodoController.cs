using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TaskManagementSystem_BusinessLogic.Logics.Interfaces;
using TaskManagementSystem_DTOs.Request;

namespace TaskManagementSystem.Controllers
{
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

        public async Task<IActionResult> AllTask()
        {
            var userId = HttpContext.User.FindFirst(x => x.Type == ClaimTypes.NameIdentifier).Value;

            var response = await _todoService.AllTaskAsync(userId);
            if (response.Success)
            {
                return View("AllTask", response.Data);
            }
            TempData["Error"] = response.Message;
            return RedirectToAction("Error");
        }

        public async Task<IActionResult> AssignedTasks()
        {
            var userId = HttpContext.User.FindFirst(x => x.Type == ClaimTypes.NameIdentifier).Value;

            var response = await _todoService.AssignedTasksAsync(userId);
            if (response.Success)
            {
                return View("AllTask", response.Data);
            }
            TempData["Error"] = response.Message;
            return RedirectToAction("Error");
        }

        public async Task<IActionResult> CreatedTasks()
        {
            var userId = HttpContext.User.FindFirst(x => x.Type == ClaimTypes.NameIdentifier).Value;

            var response = await _todoService.CreatedTasksAsync(userId);
            if (response.Success)
            {
                return View("AllTask", response.Data);
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

        public async Task<IActionResult> MarkTaskAsCompleted(Guid todoId)
        {
            var userId = HttpContext.User.FindFirst(x => x.Type == ClaimTypes.NameIdentifier).Value;

            var response = await _todoService.MarkTodoAsCompletedAsync(todoId);
            if (response.Success)
            {
                TempData["Success"] = response.Message;
                return RedirectToAction("AllTask");
            }
            TempData["Error"] = response.Message;
            return RedirectToAction("Error");
        }
    }
}
