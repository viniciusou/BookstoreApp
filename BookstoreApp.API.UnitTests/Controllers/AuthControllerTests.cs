using Moq;
using NUnit.Framework;
using BookstoreApp.API.Data;
using BookstoreApp.API.Dtos;
using Microsoft.Extensions.Configuration;
using BookstoreApp.API.Controllers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BookstoreApp.API.Models;

namespace BookstoreApp.API.UnitTests.Controllers
{
    [TestFixture]
    public class AuthControllerTests
    {
        private Mock<IAuthRepository> _repo;
        private Mock<IConfiguration> _config;
        private AuthController _controller;

        [SetUp]
        public void SetUp()
        {
            _repo = new Mock<IAuthRepository>();
            _config = new Mock<IConfiguration>();
            _controller = new AuthController(_repo.Object, _config.Object);
        }

        [Test]
        public async Task Register_UsernameAlreadyExists_ReturnsBadRequest()
        {
            var userForRegisterDto = new UserForRegisterDto { Username = "A" };
            
            _repo.Setup(r => r.UserExists("a")).Returns(Task.FromResult(true));

            var result = await _controller.Register(userForRegisterDto);
            
            Assert.That(result, Is.TypeOf<BadRequestObjectResult>());
        }

        [Test]
        public async Task Register_WhenCalled_CreatesUserIntoDb()
        {
            var userForRegisterDto = new UserForRegisterDto 
            {
                Username = "B",
                Password = "password"
            };

            await _controller.Register(userForRegisterDto);

            var user = new User { Username = userForRegisterDto.Username};
            
            _repo.Verify( r => r.Register(It.IsAny<User>(), "password"));
        }

        [Test]
        public async Task Register_WhenCalled_ReturnsCreatedStatus()
        {
            var userForRegisterDto = new UserForRegisterDto 
            {
                Username = "B",
                Password = "password"
            };

            var result = await _controller.Register(userForRegisterDto);
            
            Assert.That(result, Is.TypeOf<StatusCodeResult>());
        }

        [Test]
        public async Task Login_UsernameOrPasswordInvalid_ReturnsUnauthorized()
        {
            var userForLoginDto = new UserForLoginDto 
            {
                Username = "A",
                Password = "password"
            };

            _repo.Setup(r => r.Login(userForLoginDto.Username, userForLoginDto.Password))
                .Returns(Task.FromResult(new User()));

            var result = await _controller.Login(userForLoginDto);

            Assert.That(result, Is.TypeOf<UnauthorizedResult>());            
        } 

        [Test]
        public async Task Login_WhenCalled_LogsInToDb()
        {
            var userForLoginDto = new UserForLoginDto 
            {
                Username = "A",
                Password = "password"
            };

            await _controller.Login(userForLoginDto);

            _repo.Verify(r => r.Login("a", "password"));
        }

        [Test]
        public async Task login_WhenCalled_ReturnsOkStatus()
        {
            var userForLoginDto = new UserForLoginDto 
            {
                Username = "A",
                Password = "password"
            };

            var user = new User {Id = 1, Username = "a"};

            _repo.Setup(r => r.Login(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.FromResult(user));

          var sampleToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiaWF0IjoxNTE2MjM5MDIyfQ.SflKxwRJSMeKKF2QT4fwpMeJf36POk6yJV_adQssw5c";  
          _config.Setup(c => c.GetSection("AppSettings:Token").Value).Returns(sampleToken);

            var result = await _controller.Login(userForLoginDto);

            Assert.That(result, Is.TypeOf<OkObjectResult>());
        }
    }
}