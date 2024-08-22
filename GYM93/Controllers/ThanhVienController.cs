using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GYM93.Data;
using GYM93.Models;

namespace GYM93.Controllers
{
    public class ThanhVienController : Controller
    {
        private readonly AppDbContext _context;

        public ThanhVienController(AppDbContext context)
        {
            _context = context;
        }

        // GET: ThanhVien
        public async Task<IActionResult> Index()
        {
            return View(await _context.ThanhViens.ToListAsync());
        }

        // GET: ThanhVien/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var thanhVien = await _context.ThanhViens
                .FirstOrDefaultAsync(m => m.ThanhVienId == id);
            if (thanhVien == null)
            {
                return NotFound();
            }

            return View(thanhVien);
        }

        // GET: ThanhVien/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ThanhVien/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ThanhVienId,HoVaTenDem,Ten,Sđt,GioiTinh,Cmnd,NgayThamGia,HinhAnhTv")] ThanhVien thanhVien)
        {
            if (ModelState.IsValid)
            {
                _context.Add(thanhVien);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(thanhVien);
        }

        // GET: ThanhVien/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var thanhVien = await _context.ThanhViens.FindAsync(id);
            if (thanhVien == null)
            {
                return NotFound();
            }
            return View(thanhVien);
        }

        // POST: ThanhVien/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ThanhVienId,HoVaTenDem,Ten,Sđt,GioiTinh,Cmnd,NgayThamGia,HinhAnhTv")] ThanhVien thanhVien)
        {
            if (id != thanhVien.ThanhVienId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(thanhVien);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ThanhVienExists(thanhVien.ThanhVienId))
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
            return View(thanhVien);
        }

        // GET: ThanhVien/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var thanhVien = await _context.ThanhViens
                .FirstOrDefaultAsync(m => m.ThanhVienId == id);
            if (thanhVien == null)
            {
                return NotFound();
            }

            return View(thanhVien);
        }

        // POST: ThanhVien/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var thanhVien = await _context.ThanhViens.FindAsync(id);
            if (thanhVien != null)
            {
                _context.ThanhViens.Remove(thanhVien);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ThanhVienExists(int id)
        {
            return _context.ThanhViens.Any(e => e.ThanhVienId == id);
        }
    }
}
