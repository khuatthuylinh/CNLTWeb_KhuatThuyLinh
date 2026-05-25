using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using StudentManagement.Models;

namespace StudentManagement.Controllers
{
    public class HomeController : Controller
    {
        private const string StudentName = "Khuất Thùy Linh";
        private const string StudentEmail = "BIT247620@st.cmcu.edu.vn";
        private const string StudentNew = "Tin tức News";

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewBag.Message = StudentName;
            return View();
        }

        public IActionResult Contact()
        {
            ViewBag.Message = StudentEmail;
            return View();
        }

        public IActionResult New()
        {
            ViewBag.Message = StudentNew;
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
