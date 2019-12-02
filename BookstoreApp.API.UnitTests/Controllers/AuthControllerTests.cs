using Moq;
using NUnit.Framework;
using BookstoreApp.API.Data;
using BookstoreApp.API.Dtos;
using Microsoft.Extensions.Configuration;
using BookstoreApp.API.Controllers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace BookstoreApp.API.UnitTests.Controllers
{
    [TestFixture]
    public class AuthControllerTests
    {
        private Mock<IAuthRepository> _repo;
        private Mock<IConfiguration> _config;

        [SetUp]
        public void SetUp()
        {
            _repo = new Mock<IAuthRepository>();
            _config = new Mock<IConfiguration>();
        }

        [Test]
        public async Task Register_UserAlreadyExists_ReturnBadRequest()
        {
            _repo.Setup(r => r.UserExists("a")).Returns(Task.FromResult(true));

            var userForRegisterDto = new UserForRegisterDto { Username = "a" };

            var controller = new AuthController(_repo.Object, _config.Object);

            var result = await controller.Register(userForRegisterDto);
            
            Assert.That(result, Is.TypeOf<BadRequestObjectResult>());
        }
    }
}