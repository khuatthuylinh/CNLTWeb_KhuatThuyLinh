using Lesson3_CNLTWeb.Models;
using Microsoft.Data.SqlClient;

namespace Lesson3_CNLTWeb.Data
{
    public class BookRepository
    {
        private readonly string _connectionString;

        public BookRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("BookManagement")
                ?? throw new InvalidOperationException("Connection string 'BookManagement' not found.");
        }

        public List<Book> GetAll() => Search(null, "id");

        public List<Book> Search(string? name, string sortOrder)
        {
            var orderBy = sortOrder switch
            {
                "price_asc" => "Price ASC",
                "price_desc" => "Price DESC",
                _ => "Id ASC"
            };

            var sql = "SELECT Id, Name, Author, Price FROM Book";
            if (!string.IsNullOrWhiteSpace(name))
            {
                sql += " WHERE Name LIKE @Name";
            }
            sql += $" ORDER BY {orderBy}";

            var books = new List<Book>();

            using var connection = new SqlConnection(_connectionString);
            connection.Open();

            using var cmd = new SqlCommand(sql, connection);
            if (!string.IsNullOrWhiteSpace(name))
            {
                cmd.Parameters.AddWithValue("@Name", $"%{name.Trim()}%");
            }

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                books.Add(new Book
                {
                    Id = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    Author = reader.GetString(2),
                    Price = reader.GetDecimal(3)
                });
            }

            return books;
        }

        public Book? GetById(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            connection.Open();

            using var cmd = new SqlCommand("SELECT Id, Name, Author, Price FROM Book WHERE Id = @Id", connection);
            cmd.Parameters.AddWithValue("@Id", id);

            using var reader = cmd.ExecuteReader();
            if (!reader.Read())
            {
                return null;
            }

            return new Book
            {
                Id = reader.GetInt32(0),
                Name = reader.GetString(1),
                Author = reader.GetString(2),
                Price = reader.GetDecimal(3)
            };
        }

        public void Create(Book book)
        {
            using var connection = new SqlConnection(_connectionString);
            connection.Open();

            using var cmd = new SqlCommand(
                "INSERT INTO Book (Name, Author, Price) OUTPUT INSERTED.Id VALUES (@Name, @Author, @Price);",
                connection);
            cmd.Parameters.AddWithValue("@Name", book.Name);
            cmd.Parameters.AddWithValue("@Author", book.Author);
            cmd.Parameters.AddWithValue("@Price", book.Price);

            book.Id = (int)cmd.ExecuteScalar()!;
        }

        public bool Update(Book book)
        {
            using var connection = new SqlConnection(_connectionString);
            connection.Open();

            using var cmd = new SqlCommand(
                "UPDATE Book SET Name = @Name, Author = @Author, Price = @Price WHERE Id = @Id",
                connection);
            cmd.Parameters.AddWithValue("@Id", book.Id);
            cmd.Parameters.AddWithValue("@Name", book.Name);
            cmd.Parameters.AddWithValue("@Author", book.Author);
            cmd.Parameters.AddWithValue("@Price", book.Price);

            return cmd.ExecuteNonQuery() > 0;
        }

        public bool Delete(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            connection.Open();

            using var cmd = new SqlCommand("DELETE FROM Book WHERE Id = @Id", connection);
            cmd.Parameters.AddWithValue("@Id", id);

            return cmd.ExecuteNonQuery() > 0;
        }
    }
}
