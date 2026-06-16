namespace Lesson3_CNLTWeb.Models
{
    public class BookIndexViewModel
    {
        public List<Book> Books { get; set; } = [];
        public string? SearchName { get; set; }
        public string SortOrder { get; set; } = "id";
    }
}
