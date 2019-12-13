using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BookstoreApp.API.Controllers;
using BookstoreApp.API.Data;
using BookstoreApp.API.Dtos;
using BookstoreApp.API.Helpers;
using BookstoreApp.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;

namespace BookstoreApp.API.UnitTests.Controllers
{
    [TestFixture]
    public class PhotosControllerTests
    {
        private Mock<IBookstoreRepository> _repo;
        private PhotosController _controller;
        private Book _book;
        private Photo _photo1, _photo2;

        [SetUp]
        public void SetUp()
        {
            _repo = new Mock<IBookstoreRepository>();
            var mapper = new Mock<IMapper>();
            var cloudinaryConfig = new Mock<ICloudinaryConfig>();

            _controller = new PhotosController(_repo.Object,
                mapper.Object, cloudinaryConfig.Object);

            _photo1 = new Photo { Id = 1, IsMain = true } ;
            _photo2 = new Photo { Id = 2, IsMain = false } ;
            _book = new Book { Id = 1, Title = "a", Photos = new List<Photo> { _photo1, _photo2 } };
            _repo.Setup(r => r.GetBook(1)).Returns(Task.FromResult(_book));
            _repo.Setup(r => r.GetPhoto(1)).Returns(Task.FromResult(_photo1));
            _repo.Setup(r => r.GetPhoto(2)).Returns(Task.FromResult(_photo2));
            _repo.Setup(r => r.GetMainPhotoForBook(_book.Id)).Returns(Task.FromResult(_photo1));
        }

        [Test]
        public async Task GetPhoto_WhenCalled_RetrievesPhotoFromDb()
        {
            var id = 1;
            await _controller.GetPhoto(id);

            _repo.Verify(r => r.GetPhoto(id));
        }

        [Test]
        public async Task GetPhoto_WhenCalled_ReturnsOkResponse() 
        {
            var id = 1;
            var result = await _controller.GetPhoto(id);

            Assert.That(result, Is.TypeOf<OkObjectResult>());
        }

        [Test]
        public async Task AddPhotoForBook_WhenCalled_PhotoIsAddedIntoRepository()
        {
            var mockFile = new FormFile(new MemoryStream(Encoding.UTF8.GetBytes("Mock file")), 0, 0, "Data", "mock.txt");
            var photoForCreationDto = new PhotoForCreationDto { Description = "description", File = mockFile };
            
            await _controller.AddPhotoForBook(_book.Id, photoForCreationDto);

            _repo.Verify(r => r.SaveAll());
        }

        [Test]
        public async Task AddPhotoForBook_PhotoIsAddedIntoRepository_ReturnsCreatedAtRouteResponse()
        {
            _repo.Setup(r => r.SaveAll()).Returns(Task.FromResult(true));
            var mockFile = new FormFile(new MemoryStream(Encoding.UTF8.GetBytes("Mock file")), 0, 0, "Data", "mock.txt");
            var photoForCreationDto = new PhotoForCreationDto { Description = "description", File = mockFile };
            
            var result = await _controller.AddPhotoForBook(_book.Id, photoForCreationDto);

            Assert.That(result, Is.TypeOf<CreatedAtRouteResult>());
        }
        
        [Test]
        public async Task AddPhotoForBook_PhotoIsNotAddedIntoRepository_ReturnsBadRequestResponse()
        {
            _repo.Setup(r => r.SaveAll()).Returns(Task.FromResult(false));
            var mockFile = new FormFile(new MemoryStream(Encoding.UTF8.GetBytes("Mock file")), 0, 0, "Data", "mock.txt");
            var photoForCreationDto = new PhotoForCreationDto { Description = "description", File = mockFile };
            
            var result = await _controller.AddPhotoForBook(_book.Id, photoForCreationDto);

            Assert.That(result, Is.TypeOf<BadRequestObjectResult>());
        }

        [Test]
        public async Task SetMainPhoto_PhotoIsNotFromBook_ReturnsUnathorizedResponse()
        {
            var newPhotoId = 3;

            var result = await _controller.SetMainPhoto(_book.Id, newPhotoId);

            Assert.That(result, Is.TypeOf<UnauthorizedResult>());
        }

        [Test]
        public async Task SetMainPhoto_PhotoIsAlreadyMain_ReturnsBadRequestResponse()
        {
            var result = await _controller.SetMainPhoto(_book.Id, _photo1.Id);

            Assert.That(result, Is.TypeOf<BadRequestObjectResult>());
        }

        [Test]
        public async Task SetMainPhoto_PhotoIsSavedIntoRepo_ReturnsOkResponse()
        {
            _repo.Setup(r => r.SaveAll()).Returns(Task.FromResult(true));

            var result = await _controller.SetMainPhoto(_book.Id, _photo2.Id);

            Assert.That(result, Is.TypeOf<NoContentResult>());
        }

        [Test]
        public async Task SetMainPhoto_PhotoIsNotSavedIntoRepo_ReturnsBadRequestResponse()
        {
            _repo.Setup(r => r.SaveAll()).Returns(Task.FromResult(false));

            var result = await _controller.SetMainPhoto(_book.Id, _photo2.Id);

            Assert.That(result, Is.TypeOf<BadRequestObjectResult>());
        }

        [Test]
        public async Task DeletePhoto_PhotoIsNotFromBook_ReturnsUnauthorizedResponse()
        {
            var newPhotoId = 3;

            var result = await _controller.DetelePhoto(_book.Id, newPhotoId);

            Assert.That(result, Is.TypeOf<UnauthorizedResult>());
        }

        [Test]
        public async Task DeletePhoto_PhotoIsMain_ReturnsBadRequestResponse()
        {
            var result = await _controller.DetelePhoto(_book.Id, _photo1.Id);

            Assert.That(result, Is.TypeOf<BadRequestObjectResult>());
        }

        [Test]
        public async Task DeletePhoto_WhenCalled_DeletesPhotoFromRepo()
        {
            await _controller.DetelePhoto(_book.Id, _photo2.Id);

            _repo.Verify(r => r.Delete(_photo2));
        }

        [Test]
        public async Task DeletePhoto_PhotoIsDeletedFromRepo_ReturnsOkResponse()
        {
            _repo.Setup(r => r.SaveAll()).Returns(Task.FromResult(true));

            var result = await _controller.DetelePhoto(_book.Id, _photo2.Id);

            Assert.That(result, Is.TypeOf<OkResult>());
        }

        [Test]
        public async Task DeletePhoto_PhotoIsNotDeletedFromRepo_ReturnsBadRequestResponse()
        {
            _repo.Setup(r => r.SaveAll()).Returns(Task.FromResult(false));

            var result = await _controller.DetelePhoto(_book.Id, _photo2.Id);

            Assert.That(result, Is.TypeOf<BadRequestObjectResult>());
        }
        
    }
}