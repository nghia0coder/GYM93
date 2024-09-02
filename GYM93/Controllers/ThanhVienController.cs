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
using GYM93.Service;

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
        public async Task<IActionResult> GetAllThanhVien()
        {
            try
            {
                return Json(new { data = await _thanhVienSerivce.ThanhVienGetAll() });
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
        public async Task<IActionResult> Create([Bind("ThanhVienId,HoVaTenDem,Ten,Sđt,GioiTinh,BienSoXe,Image")] ThanhVien thanhVien)
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
        public async Task<IActionResult> Edit(int id, [Bind("ThanhVienId,HoVaTenDem,NgayThamGia,Ten,Sđt,GioiTinh,BienSoXe,Image")] ThanhVien thanhVien)
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
        [HttpGet]
        public JsonResult SearchThanhVien(string term)
        {
            var thanhviens = _thanhVienSerivce.SearchThanhVien(term);
            return new JsonResult(thanhviens);
        }

        [HttpGet]
        public async Task<JsonResult> GetThanhVienById(int thanhVienId)
        {
            var thanhvien = await _thanhVienSerivce.GetThanhVienById(thanhVienId);

            var result = new
            {
                thanhvien.ThanhVienId,
                thanhvien.Ten,
                thanhvien.HinhAnhTv // Ví dụ: lấy cột hình ảnh thành viên
            };
            return Json(result);
        }

        [HttpGet]
        public async Task<JsonResult> GetMembers(string searchQuery = "", // Chuỗi tìm kiếm
                                    int pageNumber = 1, // Số trang hiện tại
                                    int pageSize = 10, // Số lượng mục trên mỗi trang
                                    string sortBy = "Name", // Cột sắp xếp
                                    bool sortAscending = true)
        {
            try
            {
                var result = await _thanhVienSerivce.GetMembersAsync(searchQuery, pageNumber, pageSize, sortBy, sortAscending);
                return Json(result);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<IActionResult> ThanhVienStatus()
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
        [HttpGet]
        public async Task<JsonResult> GetMembersStatus(string searchQuery = "", // Chuỗi tìm kiếm
                                   int pageNumber = 1, // Số trang hiện tại
                                   int pageSize = 10, // Số lượng mục trên mỗi trang
                                   string sortBy = "Ten", // Cột sắp xếp
                                   bool sortAscending = true)
        {
            try
            {
                var result = await _thanhVienSerivce.GetMembersAsync(searchQuery, pageNumber, pageSize, sortBy, sortAscending);
                return Json(result);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
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
