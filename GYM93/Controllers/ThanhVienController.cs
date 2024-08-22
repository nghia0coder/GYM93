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
    public class ThanhVienController : Controller
    {
        private readonly IThanhVienService _thanhVienSerivce;

        public ThanhVienController(IThanhVienService thanhVienSerivce)
        {
            _thanhVienSerivce = thanhVienSerivce;
        }

        // GET: ThanhVien
        public async Task<IActionResult> Index()
        {
            try
            {
                return View(await _thanhVienSerivce.ThanhVienGetAll());
            }
            catch (Exception ex)
            {
                TempData["error"] = ex.Message;
                return View();
            }
            
        }

        // GET: ThanhVien/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var thanhVien = await _thanhVienSerivce.GetThanhVienById(id);
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
        public async Task<IActionResult> Create([Bind("ThanhVienId,HoVaTenDem,Ten,Sđt,GioiTinh,BienSoXe,NgayThamGia,HinhAnhTv")] ThanhVien thanhVien)
        {
            if (ModelState.IsValid)
            {   
                await _thanhVienSerivce.ThanhVienCreate(thanhVien);
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

            var thanhVien = await _thanhVienSerivce.GetThanhVienById(id);
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
        public async Task<IActionResult> Edit(int id, [Bind("ThanhVienId,HoVaTenDem,Ten,Sđt,GioiTinh,BienSoXe,NgayThamGia,HinhAnhTv")] ThanhVien thanhVien)
        {
            if (id != thanhVien.ThanhVienId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _thanhVienSerivce.ThanhVienUpdate(thanhVien);
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

            var thanhVien = await _thanhVienSerivce.GetThanhVienById(id);
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
            var thanhVien =  await _thanhVienSerivce.GetThanhVienById(id);
            if (thanhVien != null)
            {
                await _thanhVienSerivce.ThanhVienDelete(id);
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
