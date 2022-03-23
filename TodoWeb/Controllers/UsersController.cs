#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TodoWeb.Data.Services;
using TodoWeb.Dtos;

namespace TodoWeb.Controllers
{
    [Authorize(Policy = "IsAdmin")]
    public class UsersController : ControllerWithErrors
    {
        private readonly IUsersService _usersService;

        public UsersController(IUsersService usersService)
        {
            _usersService = usersService;
        }
        // GET: Users
        public async Task<IActionResult> Index()
        {
            return View(await _usersService.GetAllAsync());
        }
        public IActionResult PasswordReset(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ViewData["id"] = id;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PasswordReset(PasswordResetArgs args)
        {
            if (!ModelState.IsValid)
            {
                return View(args);
            }
            var commandResult = await _usersService.PasswordReset(args);
            if (commandResult.IsValid)
            {
                return RedirectToAction("Index", "Users");
            }
            ViewData["id"] = args.UserId;
            return ShowErrors<PasswordResetArgs>(commandResult, args);
        }
    }
}
