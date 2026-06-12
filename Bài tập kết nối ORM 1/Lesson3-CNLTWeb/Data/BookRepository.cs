using Lesson3_CNLTWeb.Models;
using Microsoft.EntityFrameworkCore;

namespace Lesson3_CNLTWeb.Data
{
    public class BookRepository
    {
        private readonly BookDbContext _context;

        public BookRepository(BookDbContext context)
        {
            _context = context;
        }

        public List<Book> GetAll() => Search(null, "id");

        public List<Book> Search(string? name, string sortOrder)
        {
            var query = _context.Books.AsQueryable();

            if (!string.IsNullOrWhiteSpace(name))
            {
                var keyword = name.Trim();
                query = query.Where(b => b.Name.Contains(keyword));
            }

            query = sortOrder switch
            {
                "price_asc" => query.OrderBy(b => b.Price),
                "price_desc" => query.OrderByDescending(b => b.Price),
                _ => query.OrderBy(b => b.Id)
            };

            return query.ToList();
        }

        public Book? GetById(int id)
        {
            return _context.Books.Find(id);
        }

        public void Create(Book book)
        {
            _context.Books.Add(book);
            _context.SaveChanges();
        }

        public bool Update(Book book)
        {
            var existing = _context.Books.Find(book.Id);
            if (existing == null)
            {
                return false;
            }

            existing.Name = book.Name;
            existing.Author = book.Author;
            existing.Price = book.Price;

            return _context.SaveChanges() > 0;
        }

        public bool Delete(int id)
        {
            var book = _context.Books.Find(id);
            if (book == null)
            {
                return false;
            }

            _context.Books.Remove(book);
            return _context.SaveChanges() > 0;
        }
    }
}
