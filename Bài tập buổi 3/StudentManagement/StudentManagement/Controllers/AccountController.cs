using Microsoft.AspNetCore.Mvc;
using StudentManagement.Models;

namespace StudentManagement.Controllers
{
    public class AccountController : Controller
    {
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            if (model.Username == "admin" && model.Password == "123")
            {
                ViewBag.Message = "Login success";
            }
            else
            {
                ViewBag.Message = "Login failed";
            }

            return View(model);
        }
    }
}
