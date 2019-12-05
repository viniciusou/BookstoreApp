using System.Collections.Generic;
using System.Threading.Tasks;
using BookstoreApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace BookstoreApp.API.Data
{
    public class BookstoreRepository : IBookstoreRepository
    {
        private readonly DataContext _context;
        public BookstoreRepository(DataContext context)
        {
            _context = context;
        }
        public void Add<T>(T entity) where T : class
        {
            _context.Add(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
            _context.Remove(entity);
        }

        public async Task<Book> GetBook(int id)
        {
            var book = await _context.Books.Include(p => p.Photos).FirstOrDefaultAsync(b => b.Id == id);

            return book;
        }

        public async Task<IEnumerable<Book>> GetBooks()
        {
            var books = await _context.Books.Include(p => p.Photos).ToListAsync();

            return books;
        }

        public async Task<bool> SaveAll()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}