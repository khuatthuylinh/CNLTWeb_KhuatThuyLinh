using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MID_BIT247620.Models;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace MID_BIT247620.Controllers
{
    public class RoomsController : Controller
    {
        private readonly RoomDbContext _context;

        public RoomsController(RoomDbContext context)
        {
            _context = context;
        }

        // GET: Rooms
        public async Task<IActionResult> Index(string? searchName, int? roomTypeId, bool? isAvailable, decimal? maxPrice, string? sortBy)
        {
            // Build the query starting from Rooms
            IQueryable<Room_BIT247620> roomsQuery = _context.Rooms_BIT247620
                .Include(r => r.RoomType)
                .Include(r => r.RoomImages);

            // Chức năng 2. Lọc và tìm kiếm đồng thời (thực hiện ở tầng Database qua IQueryable)
            if (!string.IsNullOrWhiteSpace(searchName))
            {
                roomsQuery = roomsQuery.Where(r => r.Name.Contains(searchName));
            }

            if (roomTypeId.HasValue)
            {
                roomsQuery = roomsQuery.Where(r => r.RoomTypeId == roomTypeId.Value);
            }

            if (isAvailable.HasValue)
            {
                roomsQuery = roomsQuery.Where(r => r.IsAvailable == isAvailable.Value);
            }

            if (maxPrice.HasValue)
            {
                roomsQuery = roomsQuery.Where(r => r.Price <= maxPrice.Value);
            }

            // Sắp xếp
            roomsQuery = sortBy switch
            {
                "price_asc" => roomsQuery.OrderBy(r => r.Price),
                "price_desc" => roomsQuery.OrderByDescending(r => r.Price),
                "area_desc" => roomsQuery.OrderByDescending(r => r.Area),
                _ => roomsQuery.OrderBy(r => r.Id) // Sắp xếp mặc định theo ID
            };

            // Thực thi truy vấn và trả về kết quả
            var rooms = await roomsQuery.ToListAsync();

            // Lưu trữ điều kiện tìm kiếm để hiển thị lại trên form sau khi reload trang
            ViewBag.SearchName = searchName;
            ViewBag.RoomTypes = new SelectList(await _context.RoomTypes_BIT247620.ToListAsync(), "Id", "Name", roomTypeId);
            ViewBag.SelectedRoomTypeId = roomTypeId;
            ViewBag.IsAvailable = isAvailable;
            ViewBag.MaxPrice = maxPrice;
            ViewBag.SortBy = sortBy;

            return View(rooms);
        }

        // GET: Rooms/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                TempData["ErrorMessage"] = "Yêu cầu mã phòng trọ hợp lệ.";
                return RedirectToAction(nameof(Index));
            }

            // Chức năng 6. Xử lý lỗi RoomId không tồn tại
            var room = await _context.Rooms_BIT247620
                .Include(r => r.RoomType)
                .Include(r => r.RoomImages)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (room == null)
            {
                TempData["ErrorMessage"] = $"Phòng trọ với mã ID {id} không tồn tại.";
                return RedirectToAction(nameof(Index));
            }

            return View(room);
        }

        // GET: Rooms/Create
        public async Task<IActionResult> Create()
        {
            ViewData["RoomTypeId"] = new SelectList(await _context.RoomTypes_BIT247620.ToListAsync(), "Id", "Name");
            return View();
        }

        // POST: Rooms/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Price,Area,IsAvailable,Description,RoomTypeId")] Room_BIT247620 room, string? imageUrl, IFormFile? imageFile)
        {
            // Validation: Tên phòng không được trùng trong cùng một loại phòng
            if (ModelState.IsValid)
            {
                bool nameExists = await _context.Rooms_BIT247620
                    .AnyAsync(r => r.RoomTypeId == room.RoomTypeId && r.Name.Trim().ToLower() == room.Name.Trim().ToLower());

                if (nameExists)
                {
                    ModelState.AddModelError("Name", "Tên phòng trọ đã tồn tại trong loại phòng này.");
                }
            }

            // Validation: RoomTypeId phải tồn tại
            if (ModelState.IsValid)
            {
                bool typeExists = await _context.RoomTypes_BIT247620.AnyAsync(t => t.Id == room.RoomTypeId);
                if (!typeExists)
                {
                    ModelState.AddModelError("RoomTypeId", "Loại phòng được chọn không tồn tại.");
                }
            }

            if (ModelState.IsValid)
            {
                _context.Add(room);
                await _context.SaveChangesAsync();

                string? finalImageUrl = null;
                if (imageFile != null && imageFile.Length > 0)
                {
                    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }
                    var uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(imageFile.FileName);
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await imageFile.CopyToAsync(fileStream);
                    }
                    finalImageUrl = "/uploads/" + uniqueFileName;
                }
                else if (!string.IsNullOrWhiteSpace(imageUrl))
                {
                    finalImageUrl = imageUrl.Trim();
                }

                if (finalImageUrl != null)
                {
                    var newImage = new RoomImage_BIT247620
                    {
                        RoomId = room.Id,
                        ImageUrl = finalImageUrl,
                        IsThumbnail = true
                    };
                    _context.RoomImages_BIT247620.Add(newImage);
                    await _context.SaveChangesAsync();
                }

                TempData["SuccessMessage"] = "Thêm phòng trọ mới thành công.";
                return RedirectToAction(nameof(Index));
            }

            ViewData["RoomTypeId"] = new SelectList(await _context.RoomTypes_BIT247620.ToListAsync(), "Id", "Name", room.RoomTypeId);
            return View(room);
        }

        // GET: Rooms/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                TempData["ErrorMessage"] = "Yêu cầu mã phòng trọ hợp lệ.";
                return RedirectToAction(nameof(Index));
            }

            var room = await _context.Rooms_BIT247620
                .Include(r => r.RoomImages)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (room == null)
            {
                TempData["ErrorMessage"] = $"Phòng trọ với mã ID {id} không tồn tại.";
                return RedirectToAction(nameof(Index));
            }

            ViewData["RoomTypeId"] = new SelectList(await _context.RoomTypes_BIT247620.ToListAsync(), "Id", "Name", room.RoomTypeId);
            return View(room);
        }

        // POST: Rooms/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Price,Area,IsAvailable,Description,RoomTypeId")] Room_BIT247620 room, string? imageUrl, IFormFile? imageFile)
        {
            if (id != room.Id)
            {
                TempData["ErrorMessage"] = "Mã ID không khớp.";
                return RedirectToAction(nameof(Index));
            }

            // Validation: Tên phòng không được trùng trong cùng một loại phòng
            if (ModelState.IsValid)
            {
                bool nameExists = await _context.Rooms_BIT247620
                    .AnyAsync(r => r.RoomTypeId == room.RoomTypeId && r.Name.Trim().ToLower() == room.Name.Trim().ToLower() && r.Id != room.Id);

                if (nameExists)
                {
                    ModelState.AddModelError("Name", "Tên phòng trọ đã tồn tại trong loại phòng này.");
                }
            }

            // Validation: RoomTypeId phải tồn tại
            if (ModelState.IsValid)
            {
                bool typeExists = await _context.RoomTypes_BIT247620.AnyAsync(t => t.Id == room.RoomTypeId);
                if (!typeExists)
                {
                    ModelState.AddModelError("RoomTypeId", "Loại phòng được chọn không tồn tại.");
                }
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(room);
                    await _context.SaveChangesAsync();

                    string? finalImageUrl = null;
                    if (imageFile != null && imageFile.Length > 0)
                    {
                        var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
                        if (!Directory.Exists(uploadsFolder))
                        {
                            Directory.CreateDirectory(uploadsFolder);
                        }
                        var uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(imageFile.FileName);
                        var filePath = Path.Combine(uploadsFolder, uniqueFileName);
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await imageFile.CopyToAsync(fileStream);
                        }
                        finalImageUrl = "/uploads/" + uniqueFileName;
                    }
                    else if (!string.IsNullOrWhiteSpace(imageUrl))
                    {
                        finalImageUrl = imageUrl.Trim();
                    }

                    if (finalImageUrl != null)
                    {
                        // Find current thumbnail image
                        var currentThumbnail = await _context.RoomImages_BIT247620
                            .FirstOrDefaultAsync(i => i.RoomId == room.Id && i.IsThumbnail);

                        if (currentThumbnail != null)
                        {
                            currentThumbnail.ImageUrl = finalImageUrl;
                            _context.Update(currentThumbnail);
                        }
                        else
                        {
                            // If no thumbnail existed, create a new one
                            var newImage = new RoomImage_BIT247620
                            {
                                RoomId = room.Id,
                                ImageUrl = finalImageUrl,
                                IsThumbnail = true
                            };
                            _context.RoomImages_BIT247620.Add(newImage);
                        }
                        await _context.SaveChangesAsync();
                    }

                    TempData["SuccessMessage"] = "Cập nhật thông tin phòng trọ thành công.";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RoomExists(room.Id))
                    {
                        TempData["ErrorMessage"] = "Phòng trọ không còn tồn tại.";
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            ViewData["RoomTypeId"] = new SelectList(await _context.RoomTypes_BIT247620.ToListAsync(), "Id", "Name", room.RoomTypeId);
            return View(room);
        }

        // GET: Rooms/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                TempData["ErrorMessage"] = "Yêu cầu mã phòng trọ hợp lệ.";
                return RedirectToAction(nameof(Index));
            }

            var room = await _context.Rooms_BIT247620
                .Include(r => r.RoomType)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (room == null)
            {
                TempData["ErrorMessage"] = $"Phòng trọ với mã ID {id} không tồn tại.";
                return RedirectToAction(nameof(Index));
            }

            return View(room);
        }

        // POST: Rooms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var room = await _context.Rooms_BIT247620.FindAsync(id);
            if (room == null)
            {
                TempData["ErrorMessage"] = $"Phòng trọ với mã ID {id} không tồn tại.";
                return RedirectToAction(nameof(Index));
            }

            _context.Rooms_BIT247620.Remove(room);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Xóa phòng trọ thành công.";
            return RedirectToAction(nameof(Index));
        }

        // POST: Rooms/AddImage
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddImage(int roomId, string imageUrl, bool isThumbnail)
        {
            // Chức năng 6. Kiểm tra RoomId tồn tại
            var room = await _context.Rooms_BIT247620
                .Include(r => r.RoomImages)
                .FirstOrDefaultAsync(r => r.Id == roomId);

            if (room == null)
            {
                TempData["ErrorMessage"] = "Phòng trọ không tồn tại để thêm ảnh.";
                return RedirectToAction(nameof(Index));
            }

            if (string.IsNullOrWhiteSpace(imageUrl))
            {
                TempData["ErrorMessage"] = "Đường dẫn ảnh không được để trống.";
                return RedirectToAction(nameof(Details), new { id = roomId });
            }

            // Chức năng 4: Nếu đặt làm ảnh đại diện, đổi ảnh cũ thành false
            if (isThumbnail)
            {
                foreach (var img in room.RoomImages)
                {
                    if (img.IsThumbnail)
                    {
                        img.IsThumbnail = false;
                        _context.Update(img);
                    }
                }
            }
            // Nếu đây là hình ảnh đầu tiên của phòng, tự động đặt làm thumbnail
            else if (!room.RoomImages.Any())
            {
                isThumbnail = true;
            }

            var newImage = new RoomImage_BIT247620
            {
                RoomId = roomId,
                ImageUrl = imageUrl.Trim(),
                IsThumbnail = isThumbnail
            };

            _context.RoomImages_BIT247620.Add(newImage);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Thêm ảnh cho phòng thành công.";
            return RedirectToAction(nameof(Details), new { id = roomId });
        }

        // POST: Rooms/SetThumbnail
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SetThumbnail(int imageId)
        {
            // Chức năng 6. Kiểm tra RoomImageId tồn tại
            var targetImage = await _context.RoomImages_BIT247620.FindAsync(imageId);
            if (targetImage == null)
            {
                TempData["ErrorMessage"] = "Hình ảnh không tồn tại trong hệ thống.";
                return RedirectToAction(nameof(Index));
            }

            var roomId = targetImage.RoomId;

            // Chức năng 4: Chuyển ảnh đại diện cũ thành false, đặt ảnh mới thành true
            var roomImages = await _context.RoomImages_BIT247620.Where(i => i.RoomId == roomId).ToListAsync();
            foreach (var img in roomImages)
            {
                if (img.Id == imageId)
                {
                    img.IsThumbnail = true;
                }
                else
                {
                    img.IsThumbnail = false;
                }
                _context.Update(img);
            }

            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Cập nhật ảnh đại diện của phòng thành công.";
            return RedirectToAction(nameof(Details), new { id = roomId });
        }

        // POST: Rooms/DeleteImage
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteImage(int imageId)
        {
            // Chức năng 6. Kiểm tra RoomImageId tồn tại
            var image = await _context.RoomImages_BIT247620.FindAsync(imageId);
            if (image == null)
            {
                TempData["ErrorMessage"] = "Hình ảnh không tồn tại.";
                return RedirectToAction(nameof(Index));
            }

            var roomId = image.RoomId;
            bool wasThumbnail = image.IsThumbnail;

            _context.RoomImages_BIT247620.Remove(image);
            await _context.SaveChangesAsync();

            // Nếu ảnh bị xóa là ảnh đại diện và phòng vẫn còn ảnh khác, tự động đặt ảnh tiếp theo làm đại diện
            if (wasThumbnail)
            {
                var remainingImage = await _context.RoomImages_BIT247620
                    .FirstOrDefaultAsync(i => i.RoomId == roomId);
                if (remainingImage != null)
                {
                    remainingImage.IsThumbnail = true;
                    _context.Update(remainingImage);
                    await _context.SaveChangesAsync();
                }
            }

            TempData["SuccessMessage"] = "Xóa ảnh thành công.";
            return RedirectToAction(nameof(Details), new { id = roomId });
        }

        private bool RoomExists(int id)
        {
            return _context.Rooms_BIT247620.Any(e => e.Id == id);
        }
    }
}
