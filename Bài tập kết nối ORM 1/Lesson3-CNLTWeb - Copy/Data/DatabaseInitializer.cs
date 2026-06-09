using Microsoft.Data.SqlClient;

namespace Lesson3_CNLTWeb.Data
{
    public static class DatabaseInitializer
    {
        public static void Initialize(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("BookManagement")
                ?? throw new InvalidOperationException("Connection string 'BookManagement' not found.");

            var builder = new SqlConnectionStringBuilder(connectionString);
            var databaseName = builder.InitialCatalog;
            builder.InitialCatalog = "master";

            using (var connection = new SqlConnection(builder.ConnectionString))
            {
                connection.Open();
                using var cmd = new SqlCommand(
                    $"IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'{databaseName}') CREATE DATABASE [{databaseName}];",
                    connection);
                cmd.ExecuteNonQuery();
            }

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using var cmd = new SqlCommand(@"
                    IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Book')
                    BEGIN
                        CREATE TABLE Book (
                            Id INT IDENTITY(1,1) PRIMARY KEY,
                            Name NVARCHAR(200) NOT NULL,
                            Author NVARCHAR(200) NOT NULL,
                            Price DECIMAL(18,2) NOT NULL
                        );
                    END

                    IF NOT EXISTS (
                        SELECT * FROM sys.columns
                        WHERE object_id = OBJECT_ID('Book') AND name = 'Author')
                    BEGIN
                        ALTER TABLE Book ADD Author NVARCHAR(200) NOT NULL DEFAULT N'';
                    END", connection);
                cmd.ExecuteNonQuery();
            }
        }
    }
}
