using System.Collections.Generic;
using System.Linq;
using BookstoreApp.API.Data;
using BookstoreApp.API.Models;
using MockQueryable.Moq;
using Moq;
using NUnit.Framework;

namespace BookstoreApp.API.UnitTests.Data
{
    [TestFixture]
    public class SeedTests
    {
        private Mock<IDataContext> _context;
        
        [SetUp]
        public void SetUp()
        {
            _context = new Mock<IDataContext>();
        }
        
        [Test]
        public void SeedBooks_WhenBooksDbSetIsEmpty_BooksAreAddedToDb()
        {
            var books = new List<Book>();
            var mock = books.AsQueryable().BuildMockDbSet();
            _context.Setup(c => c.Books).Returns(mock.Object);

            Seed.SeedBooks(_context.Object);

            _context.Verify(c => c.Books.Add(It.IsAny<Book>()));
        }

        [Test]
        public void SeedBooks_WhenBooksDbSetIsEmpty_SavesChangesToDb()
        {
            var books = new List<Book>();
            var mock = books.AsQueryable().BuildMockDbSet();
            _context.Setup(c => c.Books).Returns(mock.Object);

            Seed.SeedBooks(_context.Object);

            _context.Verify(c => c.SaveChanges());
        }

        [Test]
        public void SeedBooks_WhenBooksDbSetIsNotEmpty_DoesNotSeedBooksToDb()
        {
            var book = new Book { Id = 1, Title = "a" };
            var books = new List<Book> { book };
            var mock = books.AsQueryable().BuildMockDbSet();
            _context.Setup(c => c.Books).Returns(mock.Object);

            Seed.SeedBooks(_context.Object);

            _context.Verify(c => c.SaveChanges(), Times.Never);
        }
    }
}