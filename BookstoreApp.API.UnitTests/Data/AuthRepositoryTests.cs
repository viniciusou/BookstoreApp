using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using BookstoreApp.API.Data;
using BookstoreApp.API.Models;
using MockQueryable.Moq;
using Moq;
using NUnit.Framework;
using System.Threading;

namespace BookstoreApp.API.UnitTests.Data
{
    [TestFixture]
    public class AuthRepositoryTests
    {
        private Mock<IDataContext> _context;
        private AuthRepository _authRepository;
        private User _user;

        [SetUp]
        public void SetUp()
        {
            _context =  new Mock<IDataContext>();  
            _authRepository = new AuthRepository(_context.Object);
            byte[] passwordHash, passwordSalt;
            var password = "password";
            CreatePasswordHash(password, out passwordHash, out passwordSalt);
            _user = new User {Id = 1, Username = "a", PasswordHash = passwordHash, PasswordSalt = passwordSalt};
            var users = new List<User> { _user };
            var mock = users.AsQueryable().BuildMockDbSet();
            _context.Setup(c => c.Users).Returns(mock.Object);
        }

        [Test]
        public async Task Login_UsernameDoesNotExist_ReturnsNull()
        {
            var result = await _authRepository.Login("b", "password");

            Assert.That(result, Is.EqualTo(null));
        }

        [Test]
        public async Task Login_PasswordIsIncorrect_ReturnsNull()
        {
            var incorrectPassword = "passw0rd";
            
            var result = await _authRepository.Login(_user.Username, incorrectPassword);

            Assert.That(result, Is.EqualTo(null));
        }

        [Test]
        public async Task Login_WhenCalled_ReturnsUser()
        {
            var password = "password";
            var result = await _authRepository.Login(_user.Username, password);

            Assert.That(result, Is.EqualTo(_user));
        }

        [Test]
        public async Task Register_WhenCalled_AddsUserToDB()
        {
            var newUser = new User {Id = 2, Username = "b"};
            await _authRepository.Register(newUser, "password");
            
            _context.Verify(c => c.Users.AddAsync(newUser, CancellationToken.None));
        }

        [Test]
        public async Task Register_WhenCalled_SavesChangesToDB()
        {
            var newUser = new User {Id = 2, Username = "b"};
            await _authRepository.Register(newUser, "password");

            _context.Verify(c => c.SaveChangesAsync());
        }

        [Test]
        public async Task Register_WhenCalled_ReturnsUser()
        {
            var newUser = new User {Id = 2, Username = "b"};
            var result = await _authRepository.Register(newUser, "password");
            
            Assert.That(result, Is.EqualTo(newUser));
        }

        [Test]
        public async Task UserExists_UsernameExistsInDb_ReturnsTrue()
        {
            var result = await _authRepository.UserExists(_user.Username);

            Assert.That(result, Is.True);
        }

        [Test]
        public async Task UserExists_UsernameDoesNotExistInDb_ReturnsFalse()
        {
            var newUsername = "new";

            var result = await _authRepository.UserExists(newUsername);

            Assert.That(result, Is.False);
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using(var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            };
        }
    }
}