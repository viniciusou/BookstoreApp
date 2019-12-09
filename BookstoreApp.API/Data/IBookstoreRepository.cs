using System.Collections.Generic;
using System.Threading.Tasks;
using BookstoreApp.API.Models;

namespace BookstoreApp.API.Data
{
    public interface IBookstoreRepository
    {
         void Add<T>(T entity) where T: class;

         void Delete<T>(T entity) where T: class;

         Task<bool> SaveAll();

         Task<IEnumerable<Book>> GetBooks();

         Task<Book> GetBook(int id);

         Task<Photo> GetPhoto(int id);

         Task<Photo> GetMainPhotoForBook(int bookId);
    }
}