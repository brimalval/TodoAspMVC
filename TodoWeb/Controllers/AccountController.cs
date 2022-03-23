using Microsoft.AspNetCore.Mvc;
using TodoWeb.Data;
using TodoWeb.Data.Services;
using TodoWeb.Dtos;
using TodoWeb.Models;

namespace TodoWeb.Controllers
{
    // OnActionExecuting
    // ValidateInput 
    public class AccountController : ControllerWithErrors
    {
        private readonly IAccountService _accountService;
        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }
        public async Task<bool> IsLoggedIn()
        {
            User? currentUser = await _accountService.GetCurrentUser();
            return currentUser != null;
        }
        public async Task<IActionResult> Login()
        {
            if (await IsLoggedIn())
            {
                return RedirectToAction("Index", "Todos");
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginArgs args)
        {
            if (!ModelState.IsValid)
            {
                return View(args);
            }

            var commandResult = await _accountService.Login(args);
            return commandResult.IsValid ? 
                RedirectToAction("Index", "Todos") : 
                ShowErrors<LoginArgs>(commandResult, args);
        }

        public async Task<IActionResult> Register()
        {
            if (await IsLoggedIn())
            {
                return RedirectToAction("Index", "Todos");
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterArgs args)
        {
            if (!ModelState.IsValid)
            {
                return View(args);
            }

            var commandResult = await _accountService.Register(args);
            return (commandResult.IsValid) ?
                RedirectToAction(nameof(Login)) :
                ShowErrors<RegisterArgs>(commandResult, args);
        }

        public async Task<IActionResult> Logout()
        {
            await _accountService.Logout();
            return RedirectToAction("Index", "Home");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
