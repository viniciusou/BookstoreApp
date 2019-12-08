using NUnit.Framework;
using Moq;
using BookstoreApp.API.Data;
using BookstoreApp.API.Models;
using System.Collections.Generic;
using System.Linq;
using MockQueryable.Moq;
using System.Threading.Tasks;

namespace BookstoreApp.API.UnitTests.Data
{
    [TestFixture]
    public class BookstoreRepositoryTests
    {
        private Mock<IDataContext> _context;
        private BookstoreRepository _bookstoreRepository;
        private Book _book;
        private List<Book> _books;

        [SetUp]
        public void SetUp()
        {
            _context = new Mock<IDataContext>();
            _bookstoreRepository = new BookstoreRepository(_context.Object);
            
            _book = new Book { Id = 1, Title = "a" };
            _books = new List<Book> { _book };
            var mock = _books.AsQueryable().BuildMockDbSet();

            _context.Setup(c => c.Books).Returns(mock.Object);
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

        // [Test]
        // public async Task GetBook_WhenCalled_RetrievesBookFromDb()
        // {
        //     await _bookstoreRepository.GetBook(_book.Id);

        //     _context.Verify(c => c.Books.FirstOrDefault);
        // }

        [Test]
        public async Task GetBook_WhenCalled_ReturnsBook()
        {
            var result = await _bookstoreRepository.GetBook(_book.Id);

            Assert.That(result, Is.EqualTo(_book));
        }

        // [Test]
        // public async Task GetBooks_WhenCalled_RetrievesBooksFromDb()
        // {
        //     await _bookstoreRepository.GetBooks();
            
        //     _context.Verify(c => c.Books.ToListAsync(new System.Threading.CancellationToken()));
        // }

        [Test]
        public async Task GetBooks_WhenCalled_ReturnsBooks()
        {
            var result = await _bookstoreRepository.GetBooks();
            
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

        // [Test]
        // public async Task SaveAll_ThereAreChangesToDb_ReturnsTrue()
        // {
        //     var result = await _bookstoreRepository.SaveAll();
            
        //     Assert.That(result, Is.True);
        // }
    }
}