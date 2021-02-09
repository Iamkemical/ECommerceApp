using System;
using System.Collections.Generic;
using System.IO;
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
    public class CouponController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        public CouponController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _dbContext.Coupon.ToListAsync());
        }

        //GET - CREATE
        public IActionResult Create()
        {
            return View();
        }

        //POST - CREATE
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CouponModel coupons)
        {
            if (ModelState.IsValid)
            { 
                var files = HttpContext.Request.Form.Files;
                if (files.Count > 0)
                {
                    byte[] p1 = null;
                    using (var fs1 = files[0].OpenReadStream())
                    {
                        using(var ms1 = new MemoryStream())
                        {
                            fs1.CopyTo(ms1);
                            p1 = ms1.ToArray();
                        }
                    }
                    coupons.Picture = p1;
                }
                _dbContext.Coupon.Add(coupons);
                await _dbContext.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(coupons);
        }

        //GET - EDIT
        public async Task<IActionResult> Edit(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            var couponFromDb = await _dbContext.Coupon.FindAsync(id);

            if (couponFromDb == null)
            {
                return NotFound();
            }

            return View(couponFromDb);
        }

        //POST - EDIT
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CouponModel model, int? id)
        {
            if (id is null)
            {
                return NotFound();
            }

            var couponFromDb = await _dbContext.Coupon.SingleOrDefaultAsync(m => m.Id == id);

            if (couponFromDb is null)
            {
                return NotFound();
            }

            var files = HttpContext.Request.Form.Files;
            if (files.Count > 0)
            {
                byte[] p1 = null;
                using (var fs1 = files[0].OpenReadStream())
                {
                    using (var ms1 = new MemoryStream())
                    {
                        fs1.CopyTo(ms1);
                        p1 = ms1.ToArray();
                    }
                }
                model.Picture = p1;
            }

            couponFromDb.Name = model.Name;
            couponFromDb.Discount = model.Discount;
            couponFromDb.CouponType = model.CouponType;
            couponFromDb.MinimumAmount = model.MinimumAmount;
            couponFromDb.IsActive = model.IsActive;
            couponFromDb.Picture = model.Picture;

            _dbContext.Coupon.Update(couponFromDb);

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

            var couponFromDb = await _dbContext.Coupon.FindAsync(id);

            if (couponFromDb is null)
            {
                return NotFound();
            }

            return View(couponFromDb);
        }

        //GET - DELETE
        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null)
            {
                return NotFound();
            }

            var couponFromDb = await _dbContext.Coupon.FindAsync(id);

            if (couponFromDb is null)
            {
                return NotFound();
            }

            return View(couponFromDb);
        }

        //POST - DELETE
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            if (id is null)
            {
                return NotFound();
            }

            var couponFromDb = await _dbContext.Coupon.FindAsync(id);

            if (couponFromDb is null)
            {
                return NotFound();
            }

            _dbContext.Coupon.Remove(couponFromDb);
            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
