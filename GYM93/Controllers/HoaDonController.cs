using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GYM93.Data;
using GYM93.Models;
using GYM93.Service.IService;

namespace GYM93.Controllers
{
    public class HoaDonController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IHoaDonService _hoaDonService;

        public HoaDonController(AppDbContext context, IHoaDonService hoaDonService)
        {
            _context = context;
            _hoaDonService = hoaDonService;
        }

        // GET: HoaDon
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.HoaDons.Include(h => h.ThanhVien);
            return View(await appDbContext.ToListAsync());
        }
        [HttpGet]
        public async Task<IActionResult> GetAllHoaDon()
        {
            return Json(new { data = await _hoaDonService.GetAllHoaDon() });
        }
        // GET: HoaDon/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hoaDon = await _context.HoaDons
                .Include(h => h.ThanhVien)
                .FirstOrDefaultAsync(m => m.HoaDonId == id);
            if (hoaDon == null)
            {
                return NotFound();
            }

            return View(hoaDon);
        }

        // GET: HoaDon/Create
        public IActionResult Create()
        {
            ViewData["ThanhVienId"] = new SelectList(_context.ThanhViens, "ThanhVienId", "Ten");
            return View();
        }
        
        // POST: HoaDon/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("HoaDonId,ThanhVienId,NgayThanhToan,TongTien,ThangDangKy")] HoaDon hoaDon)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _hoaDonService.HoaDonCreate(hoaDon);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }

              
            }
            ViewData["ThanhVienId"] = new SelectList(_context.ThanhViens, "ThanhVienId", "Ten", hoaDon.ThanhVienId);
            return View(hoaDon);
        }

        // GET: HoaDon/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hoaDon = await _context.HoaDons.FindAsync(id);
            if (hoaDon == null)
            {
                return NotFound();
            }
            ViewData["ThanhVienId"] = new SelectList(_context.ThanhViens, "ThanhVienId", "BienSoXe", hoaDon.ThanhVienId);
            return View(hoaDon);
        }

        // POST: HoaDon/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("HoaDonId,ThanhVienId,TongTien,NgayThanhToan,ThangDangKy,NgayBatDau,NgayKetThuc")] HoaDon hoaDon)
        {
            if (id != hoaDon.HoaDonId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(hoaDon);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HoaDonExists(hoaDon.HoaDonId))
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
            ViewData["ThanhVienId"] = new SelectList(_context.ThanhViens, "ThanhVienId", "BienSoXe", hoaDon.ThanhVienId);
            return View(hoaDon);
        }

        // GET: HoaDon/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hoaDon = await _context.HoaDons
                .Include(h => h.ThanhVien)
                .FirstOrDefaultAsync(m => m.HoaDonId == id);
            if (hoaDon == null)
            {
                return NotFound();
            }

            return View(hoaDon);
        }

        // POST: HoaDon/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var hoaDon = await _context.HoaDons.FindAsync(id);
            if (hoaDon != null)
            {
                _context.HoaDons.Remove(hoaDon);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HoaDonExists(int id)
        {
            return _context.HoaDons.Any(e => e.HoaDonId == id);
        }
    }
}
