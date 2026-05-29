using Microsoft.AspNetCore.Mvc;

namespace StudentManagement.Controllers
{
    public class ProductController : Controller
    {
        // /Product/Detail/5  →  id lấy từ segment URL (route parameter)
        public IActionResult Detail(int? id)
        {
            if (id == null || id <= 0)
            {
                ViewBag.Message = "Lỗi: Chưa truyền Product ID hoặc ID không hợp lệ. Ví dụ: /Product/Detail/5";
                ViewBag.IsError = true;
                return View();
            }

            ViewBag.Message = $"Product ID = {id}";
            return View();
        }

        // /Product/Category?name=Laptop  →  name lấy từ query string
        public IActionResult Category(string? name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                ViewBag.Message = "Lỗi: Chưa truyền tham số name. Ví dụ: /Product/Category?name=Laptop";
                ViewBag.IsError = true;
                return View();
            }

            ViewBag.Message = $"Category = {name}";
            return View();
        }
    }
}
