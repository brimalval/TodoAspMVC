﻿#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TodoWeb.Data;
using TodoWeb.Models;

namespace TodoWeb.Controllers
{
    public class TodosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TodosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Todos
        public async Task<IActionResult> Index()
        {
            return View(await _context.Todos.ToListAsync());
        }

        // GET: Todos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var todo = await _context.Todos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (todo == null)
            {
                return NotFound();
            }

            return View(todo);
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
        public async Task<IActionResult> Create([Bind("Id,Title,Description,Done,CreatedDateTime")] Todo todo)
        {
            Todo duplicate = await _context.Todos.FirstOrDefaultAsync(m => m.Title.Trim() == todo.Title.Trim());
            if (duplicate != null)
            {
                ModelState.AddModelError("Title", "A todo item with that title already exists!");
            }
            if (ModelState.IsValid)
            {
                todo.Title = todo.Title.Trim();
                _context.Add(todo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
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

            var todo = await _context.Todos.FindAsync(id);
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
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,Done,CreatedDateTime")] Todo todo)
        {
            if (id != todo.Id)
            {
                return NotFound();
            }

            Todo duplicate = await _context.Todos.FirstOrDefaultAsync(m => m.Title.Trim() == todo.Title.Trim() && m.Id != todo.Id);
            if (duplicate != null)
            {
                var states = ModelState;
                ModelState.AddModelError("Title", "A todo item with that title already exists!");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    todo.Title = todo.Title.Trim();
                    _context.Update(todo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TodoExists(todo.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(todo);
        }

        // GET: Todos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var todo = await _context.Todos
                .FirstOrDefaultAsync(m => m.Id == id);
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
            var todo = await _context.Todos.FindAsync(id);
            _context.Todos.Remove(todo);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TodoExists(int id)
        {
            return _context.Todos.Any(e => e.Id == id);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleStatus (List<int> IdList)
        {
            if (IdList.Count() != 0)
            {
                try
                {
                    await _context.Todos.Where(todo => IdList.Contains(todo.Id)).ForEachAsync(todo => todo.Done = !todo.Done);
                    await _context.SaveChangesAsync();
                }
                catch
                {
                    return new JsonResult(null) { StatusCode = StatusCodes.Status500InternalServerError };
                }
            }
            return new JsonResult(null){ StatusCode = StatusCodes.Status200OK };
        }
    }
}
