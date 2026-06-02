using BookManagement_Buoi5.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookManagement_Buoi5.Controllers
{
    public class BookController : Controller
    {
        private static List<Book> GetBooks()
        {
            return new List<Book>()
            {
                new Book { Id = 1, Name = "Clean Code", Price = 20 },
                new Book { Id = 2, Name = "ASP.NET MVC", Price = 15 },
                new Book { Id = 3, Name = "Design Pattern", Price = 25 }
            };
        }

        public IActionResult Index()
        {
            return View(GetBooks());
        }

        public IActionResult Detail(int id)
        {
            var book = GetBooks().FirstOrDefault(b => b.Id == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Book book)
        {
            if (ModelState.IsValid)
            {
                ViewBag.Message = "Thêm sách thành công!";
            }

            return View(book);
        }
    }
}
