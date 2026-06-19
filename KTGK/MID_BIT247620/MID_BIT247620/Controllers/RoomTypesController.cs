using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MID_BIT247620.Models;
using System.Threading.Tasks;

namespace MID_BIT247620.Controllers
{
    public class RoomTypesController : Controller
    {
        private readonly RoomDbContext _context;

        public RoomTypesController(RoomDbContext context)
        {
            _context = context;
        }

        // GET: RoomTypes
        public async Task<IActionResult> Index()
        {
            var roomTypes = await _context.RoomTypes_BIT247620
                .Include(t => t.Rooms)
                .ToListAsync();
            return View(roomTypes);
        }

        // GET: RoomTypes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                TempData["ErrorMessage"] = "Yêu cầu mã loại phòng hợp lệ.";
                return RedirectToAction(nameof(Index));
            }

            var roomType = await _context.RoomTypes_BIT247620
                .Include(t => t.Rooms)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (roomType == null)
            {
                TempData["ErrorMessage"] = $"Loại phòng với mã ID {id} không tồn tại.";
                return RedirectToAction(nameof(Index));
            }

            return View(roomType);
        }

        // GET: RoomTypes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: RoomTypes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description")] RoomType_BIT247620 roomType)
        {
            if (ModelState.IsValid)
            {
                _context.Add(roomType);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Thêm loại phòng mới thành công.";
                return RedirectToAction(nameof(Index));
            }
            return View(roomType);
        }

        // GET: RoomTypes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                TempData["ErrorMessage"] = "Yêu cầu mã loại phòng hợp lệ.";
                return RedirectToAction(nameof(Index));
            }

            var roomType = await _context.RoomTypes_BIT247620.FindAsync(id);
            if (roomType == null)
            {
                TempData["ErrorMessage"] = $"Loại phòng với mã ID {id} không tồn tại.";
                return RedirectToAction(nameof(Index));
            }
            return View(roomType);
        }

        // POST: RoomTypes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description")] RoomType_BIT247620 roomType)
        {
            if (id != roomType.Id)
            {
                TempData["ErrorMessage"] = "Mã ID không khớp.";
                return RedirectToAction(nameof(Index));
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(roomType);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Cập nhật loại phòng thành công.";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RoomTypeExists(roomType.Id))
                    {
                        TempData["ErrorMessage"] = "Loại phòng không còn tồn tại.";
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(roomType);
        }

        // GET: RoomTypes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                TempData["ErrorMessage"] = "Yêu cầu mã loại phòng hợp lệ.";
                return RedirectToAction(nameof(Index));
            }

            var roomType = await _context.RoomTypes_BIT247620
                .Include(t => t.Rooms)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (roomType == null)
            {
                TempData["ErrorMessage"] = $"Loại phòng với mã ID {id} không tồn tại.";
                return RedirectToAction(nameof(Index));
            }

            return View(roomType);
        }

        // POST: RoomTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var roomType = await _context.RoomTypes_BIT247620
                .Include(t => t.Rooms)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (roomType == null)
            {
                TempData["ErrorMessage"] = $"Loại phòng với mã ID {id} không tồn tại.";
                return RedirectToAction(nameof(Index));
            }

            // Chức năng 5. Ràng buộc xóa loại phòng đang có phòng sử dụng
            if (roomType.Rooms.Any())
            {
                TempData["ErrorMessage"] = $"Không thể xóa loại phòng '{roomType.Name}' vì hiện tại đang có {roomType.Rooms.Count} phòng thuộc loại này.";
                return RedirectToAction(nameof(Index));
            }

            _context.RoomTypes_BIT247620.Remove(roomType);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Xóa loại phòng thành công.";
            return RedirectToAction(nameof(Index));
        }

        private bool RoomTypeExists(int id)
        {
            return _context.RoomTypes_BIT247620.Any(e => e.Id == id);
        }
    }
}
