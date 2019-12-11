using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookstoreApp.API.Helpers;
using BookstoreApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace BookstoreApp.API.Data
{
    public class BookstoreRepository : IBookstoreRepository
    {
        private readonly IDataContext _context;
        public BookstoreRepository(IDataContext context)
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

        public async Task<PagedList<Book>> GetBooks(BookParams bookParams)
        {
            var books = _context.Books.Include(p => p.Photos).OrderBy(b => b.Title);

            if (!string.IsNullOrEmpty(bookParams.OrderBy))
            {
                switch(bookParams.OrderBy)
                {
                    case "dateadded":
                        books = books.OrderByDescending(b => b.DateAdded);
                        break;
                    case "datemodified":
                        books = books.OrderByDescending(b => b.DateModified);
                        break;
                    default:
                        books = books.OrderBy(b => b.Title);
                        break;
                }
            }

            return await PagedList<Book>.CreateAsync(books, bookParams.PageNumber, bookParams.PageSize);
        }

        public async Task<Photo> GetPhoto(int id)
        {
            var photo = await _context.Photos.FirstOrDefaultAsync(p => p.Id == id);

            return photo;
        }

        public async Task<Photo> GetMainPhotoForBook(int bookId)
        {
            return await _context.Photos.Where(b => b.BookId == bookId).FirstOrDefaultAsync(p => p.IsMain);
        }

        public async Task<bool> SaveAll()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}