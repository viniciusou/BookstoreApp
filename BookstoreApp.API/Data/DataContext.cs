using Microsoft.EntityFrameworkCore;

namespace BookstoreApp.API.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options){}

    }
}