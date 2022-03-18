#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TodoWeb.Data;
using TodoWeb.Data.Repositories;
using TodoWeb.Data.Services;
using TodoWeb.Dtos;
using TodoWeb.Models;

namespace TodoWeb.Controllers
{
    public class TodosController : Controller
    {
        private readonly ITodoService _todoService;
        // Don't use automapper
        private readonly IMapper _mapper;
        public TodosController(ITodoService todoService, IMapper mapper)
        {
            _todoService = todoService;
            _mapper = mapper;
        }

        // GET: Todos
        public async Task<IActionResult> Index()
        {
            // return View(await _context.Todos.ToListAsync());
            return View(await _todoService.GetAllAsync());
        }

        // GET: Todos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            TodoViewModel todoViewModel = await _todoService.GetByIdAsync(id ?? -1);
            if (todoViewModel == null)
            {
                return NotFound();
            }

            return View(todoViewModel);
        }

        // GET: Todos/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Todos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateTodoArgs todo)
        {
            var commandResult = await _todoService.CreateAsync(todo);
            if (commandResult.IsValid)
            {
                return RedirectToAction(nameof(Index));
            }
            var errors = commandResult.Errors;
            foreach (var error in errors)
            {
                ModelState.AddModelError(error.Key, error.Value);
            }
            return View(todo);
        }

        // GET: Todos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var todo = await _todoService.GetByIdAsync(id ?? -1);
            if (todo == null)
            {
                return NotFound();
            }
            return View(todo);
        }

        // POST: Todos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UpdateTodoArgs args)
        {
            if (!ModelState.IsValid) 
            {
                return View(_mapper.Map<TodoViewModel>(args));
            }

            var commandResult = await _todoService.UpdateAsync(id, args);

            return commandResult.IsValid ?
                RedirectToAction(nameof(Index)) : 
                ShowErrors(
                    commandResult,
                    _mapper.Map<TodoViewModel>(args)
                );
        }

        private IActionResult ShowErrors(CommandResult commandResult, object model)
        {
            foreach (var error in commandResult.Errors)
            {
                ModelState.AddModelError(error.Key, error.Value);
            }

            return View("Edit", model);
        }

        // GET: Todos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var todo = await _todoService.GetByIdAsync(id ?? -1);
            if (todo == null)
            {
                return NotFound();
            }

            return View(todo);
        }

        // POST: Todos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _todoService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleStatus (List<int> IdList)
        {
            var commandResult = await _todoService.ToggleStatus(IdList);
            if (commandResult.IsValid)
            {
                return Ok();
            }
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}
