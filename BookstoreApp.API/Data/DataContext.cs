using BookstoreApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace BookstoreApp.API.Data
{
    public interface IDataContext
    {
        DbSet<User> Users { get; set; }
        DbSet<Book> Books { get; set; }
        DbSet<Photo> Photos { get; set; }

        System.Threading.Tasks.Task<int> SaveChangesAsync();
    }

    public class DataContext : DbContext, IDataContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }

        public DbSet<Book> Books { get; set; }

        public DbSet<Photo> Photos { get; set; }

        public System.Threading.Tasks.Task<int> SaveChangesAsync()
        {
            return base.SaveChangesAsync();
        }
    }
}