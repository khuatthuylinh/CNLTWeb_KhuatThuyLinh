using Microsoft.AspNetCore.Mvc;
using StudentManagement.Models;

namespace StudentManagement.Controllers
{
    public class StudentController : Controller
    {
        public IActionResult Info()
        {
            ViewBag.Name = "Khuất Thùy Linh";
            ViewData["Age"] = 19;

            var model = new StudentInfoViewModel
            {
                Major = "CNTT"
            };

            return View(model);
        }
    }
}
