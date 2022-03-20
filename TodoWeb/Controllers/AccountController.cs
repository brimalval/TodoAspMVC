using Microsoft.AspNetCore.Mvc;
using TodoWeb.Data.Services;
using TodoWeb.Dtos;

namespace TodoWeb.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;
        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }
        public IActionResult Login()
        {
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

        public IActionResult Register()
        {
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

        public IActionResult ShowErrors<T>(CommandResult commandResult, T args)
        {
            var errors = commandResult.Errors;
            foreach (var error in errors)
            {
                ModelState.AddModelError(error.Key, error.Value);
            }
            return View(args);
        }
    }
}
