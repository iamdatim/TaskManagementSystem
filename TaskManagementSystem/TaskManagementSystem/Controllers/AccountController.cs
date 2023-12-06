using Microsoft.AspNetCore.Mvc;
using TaskManagementSystem_BusinessLogic.Logics.Interfaces;
using TaskManagementSystem_DTOs.Request;

namespace TaskManagementSystem.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserService _userService;
        public AccountController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterUserRequestDTO model)
        {
            if (ModelState.IsValid)
            {
                var response = await _userService.RegisterUserAsync(model);
                if (response.Success)
                {
                    TempData["Message"] = response.Message;
                    return RedirectToAction("Index", "TimeEntry");
                }
                TempData["Message"] = response.Message;
                string Message = TempData["Message"] as string;
                ViewBag.Message = Message;
                return View(model);
            }
            return View(model);
        }
    }
}
