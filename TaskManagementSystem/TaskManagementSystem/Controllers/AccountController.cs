using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol.Plugins;
using TaskManagementSystem_BusinessLogic.Logics.Interfaces;
using TaskManagementSystem_DataSource.Entities;
using TaskManagementSystem_DTOs.Request;

namespace TaskManagementSystem.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserService _userService;
        private readonly SignInManager<ApplicationUser> _signInManager;


        public AccountController(IUserService userService, SignInManager<ApplicationUser> signInManager)
        {
            _userService = userService;
            _signInManager = signInManager;
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
                    TempData["Success"] = response.Message;
                    return RedirectToAction("Login", "Account");
                }
                TempData["Error"] = response.Message;
                string Message = TempData["Error"] as string;
                ViewBag.Message = Message;
                return View(model);
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Login()
        {
            string Message = TempData["Success"] as string;
            ViewBag.Message = Message;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginRequestDTO model)
        {
            if (ModelState.IsValid)
            {
                //var user = await _userManager.FindByEmailAsync(login.Email);
                //if (user != null)
                //{
                    var result = await _signInManager.PasswordSignInAsync(model.Email.Split('@')[0], model.Password, model.RememberMe, lockoutOnFailure: false);

                    if (result.Succeeded)
                    {
                        TempData["Success"] = "Login Successful";
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        TempData["Error"] = "Wrong credentials. Please, try again!";
                        return View(model);
                    }
                //}
                //TempData["Error"] = "Wrong credentials. Please, try again!";
                //return View(model);
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }
    }
}
