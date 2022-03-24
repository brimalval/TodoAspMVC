using Microsoft.AspNetCore.Mvc;
using TodoWeb.Data;
using TodoWeb.Dtos;

namespace TodoWeb.Controllers
{
    public abstract class ControllerWithErrors : Controller
    {
        public IActionResult ShowErrors<T>(CommandResult commandResult, T args) where T : IDto
        {
            var errors = commandResult.Errors;
            foreach (var error in errors)
            {
                ModelState.AddModelError(error.Key, error.Value);
            }
            return View(args);
        }
        public IActionResult ShowErrors(CommandResult commandResult, string viewName)
        {
            var errors = commandResult.Errors;
            foreach (var error in errors)
            {
                ModelState.AddModelError(error.Key, error.Value);
            }
            return View(viewName);
        }
        public void LoadErrors(CommandResult commandResult)
        {
            var errors = commandResult.Errors;
            foreach (var error in errors)
            {
                ModelState.AddModelError(error.Key, error.Value);
            }
        }
    }
}
