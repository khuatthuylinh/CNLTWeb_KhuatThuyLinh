using Lesson3_CNLTWeb.Data;
using Lesson3_CNLTWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Collections.Generic;
using System;
using System.Linq;

namespace Lesson3_CNLTWeb.Controllers
{
    public class BookController : Controller
    {
        private readonly BookRepository _bookRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public BookController(BookRepository bookRepository, IWebHostEnvironment webHostEnvironment)
        {
            _bookRepository = bookRepository;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index(string? searchName, string sortOrder = "id")
        {
            var viewModel = new BookIndexViewModel
            {
                SearchName = searchName,
                SortOrder = sortOrder,
                Books = _bookRepository.Search(searchName, sortOrder)
            };

            return View(viewModel);
        }

        public IActionResult Detail(int id)
        {
            var book = _bookRepository.GetById(id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Book book, List<IFormFile> ImageFiles)
        {
            if (ImageFiles != null && ImageFiles.Count > 0)
            {
                var allowedExtensions = new[] { ".jpg", ".png" };
                foreach (var file in ImageFiles)
                {
                    var ext = Path.GetExtension(file.FileName).ToLower();
                    if (!allowedExtensions.Contains(ext))
                    {
                        ModelState.AddModelError("ImageFiles", $"Chỉ chấp nhận file định dạng .jpg hoặc .png. File '{file.FileName}' không hợp lệ.");
                    }
                }
            }

            if (!ModelState.IsValid)
            {
                return View(book);
            }

            if (ImageFiles != null && ImageFiles.Count > 0)
            {
                var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                var savedFileNames = new List<string>();
                foreach (var file in ImageFiles)
                {
                    if (file.Length > 0)
                    {
                        var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                        var filePath = Path.Combine(uploadsFolder, uniqueFileName);
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            file.CopyTo(fileStream);
                        }
                        savedFileNames.Add(uniqueFileName);
                    }
                }

                if (savedFileNames.Count > 0)
                {
                    book.Images = string.Join(";", savedFileNames);
                }
            }

            _bookRepository.Create(book);

            TempData["SuccessMessage"] = "Thêm sách thành công!";
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(int id)
        {
            var book = _bookRepository.GetById(id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Book book, List<IFormFile> ImageFiles)
        {
            if (id != book.Id)
            {
                return NotFound();
            }

            if (ImageFiles != null && ImageFiles.Count > 0)
            {
                var allowedExtensions = new[] { ".jpg", ".png" };
                foreach (var file in ImageFiles)
                {
                    var ext = Path.GetExtension(file.FileName).ToLower();
                    if (!allowedExtensions.Contains(ext))
                    {
                        ModelState.AddModelError("ImageFiles", $"Chỉ chấp nhận file định dạng .jpg hoặc .png. File '{file.FileName}' không hợp lệ.");
                    }
                }
            }

            if (!ModelState.IsValid)
            {
                return View(book);
            }

            if (ImageFiles != null && ImageFiles.Count > 0)
            {
                var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                // Delete old image files if they exist to avoid orphaned files
                if (!string.IsNullOrEmpty(book.Images))
                {
                    var oldFiles = book.Images.Split(';', StringSplitOptions.RemoveEmptyEntries);
                    foreach (var oldFile in oldFiles)
                    {
                        var oldFilePath = Path.Combine(uploadsFolder, oldFile);
                        if (System.IO.File.Exists(oldFilePath))
                        {
                            try
                            {
                                System.IO.File.Delete(oldFilePath);
                            }
                            catch (Exception)
                            {
                                // Fail silently if cannot delete file
                            }
                        }
                    }
                }

                var savedFileNames = new List<string>();
                foreach (var file in ImageFiles)
                {
                    if (file.Length > 0)
                    {
                        var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                        var filePath = Path.Combine(uploadsFolder, uniqueFileName);
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            file.CopyTo(fileStream);
                        }
                        savedFileNames.Add(uniqueFileName);
                    }
                }

                if (savedFileNames.Count > 0)
                {
                    book.Images = string.Join(";", savedFileNames);
                }
            }

            if (!_bookRepository.Update(book))
            {
                return NotFound();
            }

            TempData["SuccessMessage"] = "Cập nhật sách thành công!";
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int id)
        {
            var book = _bookRepository.GetById(id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var book = _bookRepository.GetById(id);
            if (book == null)
            {
                return NotFound();
            }

            if (!_bookRepository.Delete(id))
            {
                return NotFound();
            }

            if (!string.IsNullOrEmpty(book.Images))
            {
                var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");
                var files = book.Images.Split(';', StringSplitOptions.RemoveEmptyEntries);
                foreach (var file in files)
                {
                    var filePath = Path.Combine(uploadsFolder, file);
                    if (System.IO.File.Exists(filePath))
                    {
                        try
                        {
                            System.IO.File.Delete(filePath);
                        }
                        catch (Exception)
                        {
                            // Fail silently
                        }
                    }
                }
            }

            TempData["SuccessMessage"] = "Xóa sách thành công!";
            return RedirectToAction(nameof(Index));
        }
    }
}
