using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Spice.Data;
using Spice.Models;
using Spice.Models.ViewModels;
using Spice.Utility;

namespace Spice.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.ManagerUser)]
    public class SubCategoryController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        [TempData]
        public string StatusMessage { get; set; }
        public SubCategoryController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        //GET - INDEX
        public async Task<IActionResult> Index()
        {
            var subCategories = await _dbContext.SubCategory.Include(s => s.Category).ToListAsync();

            return View(subCategories);
        }

        //GET - CREATE
        public async Task<IActionResult> Create()
        {
            SubCategoryAndCategoryViewModel model = new SubCategoryAndCategoryViewModel()
            {
                CategoryList = await _dbContext.Category.ToListAsync(),
                SubCategory = new Models.SubCategoryModel(),
                SubCategoryList = await _dbContext.SubCategory.OrderBy(p => p.Name).Select(p => p.Name).Distinct().ToListAsync()
            };
            return View(model);
        }

        //POST - CREATE 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SubCategoryAndCategoryViewModel model)
        {
            if(ModelState.IsValid)
            {
                var doesSubCategoryExist = _dbContext.SubCategory.Include(s => s.Category).Where(s => s.Name == model.SubCategory.Name && s.Category.Id == model.SubCategory.CategoryId);

                if (doesSubCategoryExist.Count()>0)
                {
                    //Error
                    StatusMessage = "Error: Sub Category exist under " + doesSubCategoryExist.First().Category.Name + " category. Please use another name"; 
                }
                else
                {
                    _dbContext.SubCategory.Add(model.SubCategory);
                    await _dbContext.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            SubCategoryAndCategoryViewModel modelVM = new SubCategoryAndCategoryViewModel()
            {
                CategoryList = await _dbContext.Category.ToListAsync(),
                SubCategory = model.SubCategory,
                SubCategoryList = await _dbContext.SubCategory.OrderBy(p => p.Name).Select(p => p.Name).ToListAsync(),
                StatusMessage = StatusMessage
            };
            return View(modelVM);
        }

        [ActionName("GetSubCategory")]
        public async Task<IActionResult> GetSubCategory(int id)
        {
            List<SubCategoryModel> subCategories = new List<SubCategoryModel>();

            subCategories = await (from subCategory in _dbContext.SubCategory
                                   where subCategory.CategoryId == id
                                   select subCategory).ToListAsync();
            return Json(new SelectList(subCategories, "Id", "Name"));
        }

        //GET - Edit
        public async Task<IActionResult> Edit(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }
            var subCategory = await _dbContext.SubCategory.SingleOrDefaultAsync(m => m.Id == id);

            if(subCategory is null)
            {
                return NotFound();
            }
            SubCategoryAndCategoryViewModel model = new SubCategoryAndCategoryViewModel()
            {
                CategoryList = await _dbContext.Category.ToListAsync(),
                SubCategory = subCategory,
                SubCategoryList = await _dbContext.SubCategory.OrderBy(p => p.Name).Select(p => p.Name).Distinct().ToListAsync()
            };
            return View(model);
        }

        //POST - EDIT 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(SubCategoryAndCategoryViewModel model)
        {
            if (ModelState.IsValid)
            {
                var doesSubCategoryExist = _dbContext.SubCategory.Include(s => s.Category).Where(s => s.Name == model.SubCategory.Name && s.Category.Id == model.SubCategory.CategoryId);

                if (doesSubCategoryExist.Count() > 0)
                {
                    //Error
                    StatusMessage = "Error: Sub Category exist under " + doesSubCategoryExist.First().Category.Name + " category. Please use another name";
                }
                else
                {
                    var subCatFromDb = await _dbContext.SubCategory.FindAsync(model.SubCategory.Id);
                    subCatFromDb.Name = model.SubCategory.Name;

                    await _dbContext.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            SubCategoryAndCategoryViewModel modelVM = new SubCategoryAndCategoryViewModel()
            {
                CategoryList = await _dbContext.Category.ToListAsync(),
                SubCategory = model.SubCategory,
                SubCategoryList = await _dbContext.SubCategory.OrderBy(p => p.Name).Select(p => p.Name).ToListAsync(),
                StatusMessage = StatusMessage
            };
            return View(modelVM);
        }

        //GET - DETAILS
        public async Task<IActionResult> Details(int? id)
        {
            if (id is null)
            {
                return NotFound();
            }
            var subCatDetails = await _dbContext.SubCategory.Include(s => s.Category).SingleOrDefaultAsync(m => m.Id == id);
            if(subCatDetails is null)
            {
                return NotFound();
            }
            return View(subCatDetails);
        }

        //GET - DELETE
        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null)
            {
                return NotFound();
            }
            var subCatAndCatDetails = await _dbContext.SubCategory.Include(m => m.Category).SingleOrDefaultAsync(m => m.Id == id);
            if (subCatAndCatDetails is null)
            {
                return NotFound();
            }
            return View(subCatAndCatDetails);
        }

        //POST - DELETE
        [ValidateAntiForgeryToken]
        [HttpPost]
        [ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            if (id is null)
            {
                return NotFound();
            }

            var subCatFromDb = await _dbContext.SubCategory.FindAsync(id);
            
            if (subCatFromDb is null)
            {
                return NotFound();
            }
            var removeSubCat = _dbContext.Remove(subCatFromDb);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
