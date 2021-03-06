using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using BookstoreApp.API.Controllers;
using BookstoreApp.API.Data;
using BookstoreApp.API.Dtos;
using BookstoreApp.API.Helpers;
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
        private BooksController _controller;
        private BookParams _bookParams;
        private Book _book;

        [SetUp]
        public void SetUp()
        {
            _repo = new Mock<IBookstoreRepository>();
            var mapper = new Mock<IMapper>();
            var cloudinaryConfig = new Mock<ICloudinaryConfig>();
            _controller = new BooksController(_repo.Object, mapper.Object, cloudinaryConfig.Object);

            _bookParams = new BookParams();

            _book = new Book { Id = 1 };
            _book.Photos = new List<Photo> { new Photo { Id = 1 } };
                       
            _repo.Setup(r => r.GetBook(_book.Id)).Returns(Task.FromResult(_book));
        }

        [Test]
        public async Task AddBook_WhenCalled_SavesBookIntoDb()
        {
            await _controller.AddBook(It.IsAny<BookForCreationDto>());

            _repo.Verify(r => r.Add(It.IsAny<Book>()));
        }

        [Test]
        public async Task AddBook_WhenCalled_ReturnsCreatedAtRouteResponse()
        {
            _repo.Setup(r => r.SaveAll()).Returns(Task.FromResult(true));

            var result = await _controller.AddBook(It.IsAny<BookForCreationDto>());

            Assert.That(result, Is.TypeOf<CreatedAtRouteResult>());
        }

        [Test]
        public async Task AddBook_SavingBookFails_ReturnsBadRequestResponse()
        {
            _repo.Setup(r => r.SaveAll()).Returns(Task.FromResult(false));

            var result = await _controller.AddBook(It.IsAny<BookForCreationDto>());

            Assert.That(result, Is.TypeOf<BadRequestObjectResult>());
        }

        [Test]
        public async Task DeleteBook_BookDoesNotExistInDb_ReturnsUnauthorizedResponse()
        {
            var result = await _controller.DeleteBook(2);

            Assert.That(result, Is.TypeOf<UnauthorizedResult>());
        }

        [Test]
        public async Task DeleteBook_WhenCalled_DeletesBookFromDb()
        {
            await _controller.DeleteBook(_book.Id);

            _repo.Verify(r => r.Delete(_book));
        }

        [Test]
        public async Task DeleteBook_WhenCalled_ReturnsOkResponse()
        {
            _repo.Setup(r => r.SaveAll()).Returns(Task.FromResult(true));
            var result = await _controller.DeleteBook(_book.Id);

            Assert.That(result, Is.TypeOf<OkResult>());
        }

        [Test]
        public async Task DeleteBook_DeletingBookFails_ReturnsBadRequestResponse()
        {
            _repo.Setup(r => r.SaveAll()).Returns(Task.FromResult(false));
            var result = await _controller.DeleteBook(_book.Id);

            Assert.That(result, Is.TypeOf<BadRequestObjectResult>());
        }

        [Test]
        public async Task GetBooks_WhenCalled_RetrievesBooksFromDb()
        {
            await _controller.GetBooks(_bookParams);

            _repo.Verify(r => r.GetBooks(_bookParams));
        }

        [Test]
        public async Task GetBooks_WhenCalled_ReturnsOkResponse()
        {   
            _repo.Setup(r => r.GetBooks(_bookParams))
                .Returns(Task.FromResult(It.IsAny<PagedList<Book>>()));

            var result = await _controller.GetBooks(_bookParams);

            Assert.That(result, Is.TypeOf<OkObjectResult>());
        }

        [Test]
        public async Task GetBook_WhenCalled_RetrievesBookFromDb()
        {
            var id = 1;

            await _controller.GetBook(id);

            _repo.Verify(r => r.GetBook(id));
        }

        [Test]
        public async Task GetBook_WhenCalled_ReturnsOkResponse()
        {
            var book = new Book {Id = 1};
            _repo.Setup(r => r.GetBook(book.Id)).Returns(Task.FromResult(book));

            var result = await _controller.GetBook(book.Id);

            Assert.That(result, Is.TypeOf<OkObjectResult>());
        }

        [Test]
        public async Task UpdateBook_WhenCalled_RetrievesBookFromDb()
        {
            var id = 1;

            _repo.Setup(r => r.SaveAll()).Returns(Task.FromResult(true));
            
            await _controller.UpdateBook(id, It.IsAny<BookForUpdateDto>());

            _repo.Verify(r => r.GetBook(id));
        }

        [Test]
        public async Task UpdateBook_WhenCalled_ReturnsNoContentResult()
        {
            var id = 1;

            _repo.Setup(r => r.GetBook(id)).Returns(Task.FromResult(It.IsAny<Book>()));

            _repo.Setup(r => r.SaveAll()).Returns(Task.FromResult(true));

            var result = await _controller.UpdateBook(id, It.IsAny<BookForUpdateDto>());

            Assert.That(result, Is.TypeOf<NoContentResult>());
        }

        [Test]
        public void UpdateBook_UpdatingBookFails_ReturnsException()
        {
            _repo.Setup(r => r.GetBook(It.IsAny<int>())).Returns(Task.FromResult(It.IsAny<Book>()));

            _repo.Setup(r => r.SaveAll()).Returns(Task.FromResult(false));

            Assert.That(async() => await _controller.UpdateBook(It.IsAny<int>(), It.IsAny<BookForUpdateDto>()),
                Throws.Exception);
        }
    }
}