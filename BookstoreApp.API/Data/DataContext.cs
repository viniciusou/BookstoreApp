using BookstoreApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace BookstoreApp.API.Data
{
    public interface IDataContext
    {
        DbSet<User> Users { get; set; }
        DbSet<Book> Books { get; set; }
        DbSet<Photo> Photos { get; set; }

        void Add<T>(T entity);
        void Remove<T>(T entity);

        void SaveChanges();

        System.Threading.Tasks.Task<int> SaveChangesAsync();
    }

    public class DataContext : DbContext, IDataContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }

        public DbSet<Book> Books { get; set; }

        public DbSet<Photo> Photos { get; set; }

        public new void Add<T>(T entity)
        {
            base.Add(entity);
        }

        public new void Remove<T>(T entity)
        {
            base.Remove(entity);
        }

        public new void SaveChanges()
        {
            base.SaveChanges();
        }

        public System.Threading.Tasks.Task<int> SaveChangesAsync()
        {
            return base.SaveChangesAsync();
        }
    }
}