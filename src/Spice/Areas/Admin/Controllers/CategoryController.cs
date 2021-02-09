using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Spice.Data;
using Spice.Models;
using Spice.Utility;

namespace Spice.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.ManagerUser)]
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        public CategoryController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        //GET
        public async Task<IActionResult> Index()
        {
            return View(await _dbContext.Category.ToListAsync());
        }

        //GET - CREATE
        public IActionResult Create()
        {
            return View();
        }

        //POST - CREATE
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategoryModel model)
        {
            if (ModelState.IsValid)
            {
                _dbContext.Category.Add(model);
                await _dbContext.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        //GET - EDIT
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var category = await _dbContext.Category.FindAsync(id);
            if (category is null)
            {
                return NotFound();
            }
            return View(category);
        }

        //POST - EDIT
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CategoryModel model)
        {
            if(ModelState.IsValid)
            {
                _dbContext.Update(model);
                await _dbContext.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        //GET - EDIT
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var category = await _dbContext.Category.FindAsync(id);
            if (category is null)
            {
                return NotFound();
            }
            return View(category);
        }

        //POST - DELETE
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            var category = await _dbContext.Category.FindAsync(id);

            if(category is null)
            {
                return View();
            }
            _dbContext.Category.Remove(category);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        //GET - DETAILS
        public async Task<IActionResult> Details(int? id)
        {
            if (id is null)
            {
                return NotFound();
            }
           var category = await _dbContext.Category.FindAsync(id);

            return View(category);
        }
    }
}
