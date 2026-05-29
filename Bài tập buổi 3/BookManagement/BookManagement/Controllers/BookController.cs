using Microsoft.AspNetCore.Mvc;
using BookManagement.Models;

namespace BookManagement.Controllers
{
    public class BookController : Controller
    {
        // Danh sách sách
        public IActionResult Index()
        {
            List<Book> books = new List<Book>()
            {
                new Book { Id = 1, Name = "Clean Code", Price = 20 },
                new Book { Id = 2, Name = "ASP.NET MVC", Price = 15 },
                new Book { Id = 3, Name = "Design Pattern", Price = 25 }
            };

            return View(books);
        }

        // Chi tiết sách
        public IActionResult Detail(int id)
        {
            Book book = new Book()
            {
                Id = id,
                Name = "Clean Code",
                Price = 20
            };

            return View(book);
        }

        // GET
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // POST
        [HttpPost]
        public IActionResult Create(Book book)
        {
            ViewBag.Message = "Thêm sách thành công!";

            return View();
        }
    }
}