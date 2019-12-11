using NUnit.Framework;
using Moq;
using BookstoreApp.API.Data;
using BookstoreApp.API.Models;
using System.Collections.Generic;
using System.Linq;
using MockQueryable.Moq;
using System.Threading.Tasks;
using BookstoreApp.API.Helpers;

namespace BookstoreApp.API.UnitTests.Data
{
    [TestFixture]
    public class BookstoreRepositoryTests
    {
        private Mock<IDataContext> _context;
        private BookstoreRepository _bookstoreRepository;
        private Book _book;
        private List<Book> _books;
        private Photo _photo;
        private List<Photo> _photos;

        [SetUp]
        public void SetUp()
        {
            _context = new Mock<IDataContext>();
            _bookstoreRepository = new BookstoreRepository(_context.Object);
            
            _book = new Book { Id = 1, Title = "a" };
            _books = new List<Book> { _book };
            var booksMock = _books.AsQueryable().BuildMockDbSet();

            _context.Setup(c => c.Books).Returns(booksMock.Object);

            _photo = new Photo {Id = 1, BookId = 1, IsMain = true};
            _photos = new List<Photo> { _photo };
            var photosMock = _photos.AsQueryable().BuildMockDbSet();

            _context.Setup(c => c.Photos).Returns(photosMock.Object);
        }

        [Test]
        public void Add_WhenCalled_AddsBookToDb()
        {
            var newBook = new Book { Id = 2, Title = "b" };
            _bookstoreRepository.Add(newBook);

            _context.Verify(c => c.Add(newBook));
        }

        [Test]
        public void Delete_WhenCalled_DeletesBookFromDb()
        {
            _bookstoreRepository.Delete(_book);

            _context.Verify(c => c.Remove(_book));
        }

        [Test]
        public async Task GetBook_WhenCalled_ReturnsBook()
        {
            var result = await _bookstoreRepository.GetBook(_book.Id);

            Assert.That(result, Is.EqualTo(_book));
        }

        [Test]
        public async Task GetBooks_WhenCalled_ReturnsBooks()
        {
            var result = await _bookstoreRepository.GetBooks(new BookParams());
            
            Assert.That(result, Is.EqualTo(_books));
        }

        [Test]
        public async Task SaveAll_WhenCalled_SaveChangesToDb()
        {
            await _bookstoreRepository.SaveAll();

            _context.Verify(c => c.SaveChangesAsync());
        }

        [Test]
        public async Task SaveAll_ThereAreNoChangesToDb_ReturnsFalse()
        {
            var result = await _bookstoreRepository.SaveAll();

            Assert.That(result, Is.False);
        }

        [Test]
        public async Task GetPhoto_WhenCalled_ReturnsPhoto()
        {
            var result = await _bookstoreRepository.GetPhoto(_photo.Id);

            Assert.That(result, Is.EqualTo(_photo));
        }

        [Test]
        public async Task GetMainPhotoForBook_WhenCalled_ReturnsMainPhoto()
        {
            var result = await _bookstoreRepository.GetMainPhotoForBook(_book.Id);

            Assert.That(result, Is.EqualTo(_photo));
        }
    }
}