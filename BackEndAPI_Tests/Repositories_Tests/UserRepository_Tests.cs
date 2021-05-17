using System;
using System.Linq;
using System.Threading.Tasks;
using BackEndAPI.DBContext;
using BackEndAPI.Entities;
using BackEndAPI.Enums;
using BackEndAPI.Interfaces;
using BackEndAPI.Repositories;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;

namespace BackEndAPI_Tests.Services
{
    [TestFixture]
    public class UserRepository_Tests
    {
        private AssetsManagementDBContext _context;
        private IAsyncUserRepository _repository;

        [OneTimeSetUp]
        public void Setup()
        {
        }

        [Test]
        public async Task Create_AddNewUser_WritesToDatabase()
        {

            //Arrange
            User user = new User()
            {
                Id = 3,
                StaffCode = "SD0003",
                FirstName = "Thang",
                LastName = "Doan Viet",
                DateOfBirth = new DateTime(1995, 06, 03),
                JoinedDate = new DateTime(2021, 12, 05),
                Gender = Gender.Male,
                Type = UserType.Admin,
                UserName = "thangdv",
                Password = "thangdv@30601995",
                Location = Location.HaNoi,
                Status = UserStatus.Active
            };
            var options = new DbContextOptionsBuilder<AssetsManagementDBContext>()
                    .UseInMemoryDatabase(databaseName: "Create_AddNewUser_WritesToDatabase")
                .Options;

            _context = new AssetsManagementDBContext(options);
            _repository = new UserRepository(_context);

            //Act
            var _user = await _repository.Create(user);
            await _context.SaveChangesAsync();

            //Assert
            Assert.AreEqual(1, _context.Users.Count());

            Assert.AreEqual("SD0003", _context.Users.Single().StaffCode);

            Assert.AreEqual("Thang", _context.Users.Single().FirstName);

            Assert.AreEqual("Doan Viet", _context.Users.Single().LastName);

            Assert.AreEqual(new DateTime(1995, 06, 03), _context.Users.Single().DateOfBirth);

            Assert.AreEqual(new DateTime(2021, 12, 05), _context.Users.Single().JoinedDate);

            Assert.AreEqual(Gender.Male, _context.Users.Single().Gender);

            Assert.AreEqual(UserType.Admin, _context.Users.Single().Type);

            Assert.AreEqual("thangdv", _context.Users.Single().UserName);

            Assert.AreEqual("thangdv@30601995", _context.Users.Single().Password);

            Assert.AreEqual(UserStatus.Active, _context.Users.Single().Status);

            Assert.AreEqual(Location.HaNoi, _context.Users.Single().Location);

        }

        [TestCase(null)]
        public void CountUsername_NullUsernameInserted_ThrowExceptionMessage(string username)
        {

            var options = new DbContextOptionsBuilder<AssetsManagementDBContext>()
                        .UseInMemoryDatabase(databaseName: "CountUsername_NullUsernameInserted_ThrowExceptionMessage")
                        .Options;
            _repository = new UserRepository(_context);

            var result = Assert.Throws<ArgumentNullException>(() => _repository.CountUsername(username));

            Assert.AreEqual("Value cannot be null. (Parameter 'Username can not be null!')", result.Message);
            //I don't know how to fix this one

        }

        [TestCase("thangdv")]
        public void CountUsername_ValidUsernameInserted_ReturnNumberOfGivenUsername(string username)
        {

            var options = new DbContextOptionsBuilder<AssetsManagementDBContext>()
                        .UseInMemoryDatabase(databaseName: "CountUsername_ValidUsernameInserted_ReturnNumberOfGivenUsername")
                        .Options;

            _context = new AssetsManagementDBContext(options);
            _repository = new UserRepository(_context);

            Assert.AreEqual(0, _repository.CountUsername(username));

        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _context.DisposeAsync();
        }
    }
}