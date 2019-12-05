using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using BookstoreApp.API.Controllers;
using BookstoreApp.API.Data;
using BookstoreApp.API.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;

namespace BookstoreApp.API.UnitTests.Controllers
{
    [TestFixture]
    public class BooksControllerTests
    {
        private Mock<IBookstoreRepository> _repo;
        private Mock<IMapper> _mapper;
        private BooksController _controller;

        [SetUp]
        public void SetUp()
        {
            _repo = new Mock<IBookstoreRepository>();
            _mapper = new Mock<IMapper>();
            _controller = new BooksController(_repo.Object, _mapper.Object);
        }

        [Test]
        public async Task GetBooks_WhenCalled_RetrievesBooksFromDb()
        {
            await _controller.GetBooks();

            _repo.Verify(r => r.GetBooks());
        }

        [Test]
        public async Task GetBooks_WhenCalled_ReturnsOkResponse()
        {   
            _repo.Setup(r => r.GetBooks())
                .Returns(Task.FromResult(It.IsAny<IEnumerable<Book>>()));

            var result = await _controller.GetBooks();

            Assert.That(result, Is.TypeOf<OkObjectResult>());
        }

        [Test]
        public async Task GetBook_WhenCalled_RetrievesBookFromDb()
        {
            await _controller.GetBook(It.IsAny<int>());

            _repo.Verify(r => r.GetBook(It.IsAny<int>()));
        }

        [Test]
        public async Task GetBook_WhenCalled_ReturnsOkResponse()
        {
            var book = new Book {Id = 1};
            _repo.Setup(r => r.GetBook(book.Id)).Returns(Task.FromResult(book));

            var result = await _controller.GetBook(book.Id);

            Assert.That(result, Is.TypeOf<OkObjectResult>());
        }
    }
}