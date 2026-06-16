using Microsoft.EntityFrameworkCore;

namespace Lesson3_CNLTWeb.Data
{
    public static class DatabaseInitializer
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<BookDbContext>();
            context.Database.EnsureCreated();
        }
    }
}
