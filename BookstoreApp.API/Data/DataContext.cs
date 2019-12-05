using BookstoreApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace BookstoreApp.API.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options){}

        public DbSet<User> Users { get; set; }

        public DbSet<Book> Books { get; set; }

        public DbSet<Photo> Photos { get; set; }

    }
}