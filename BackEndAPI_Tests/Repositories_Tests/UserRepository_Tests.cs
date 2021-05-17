using System;
using System.Linq;
using System.Threading.Tasks;
using BackEndAPI.DBContext;
using BackEndAPI.Entities;
using BackEndAPI.Enums;
using BackEndAPI.Interfaces;
using BackEndAPI.Repositories;
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
            var options = new DbContextOptionsBuilder<AssetsManagementDBContext>()
                   .UseInMemoryDatabase(databaseName: "UserDatabase")
                .Options;

            _context = new AssetsManagementDBContext(options);
            _repository = new UserRepository(_context);
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

            _context.Database.EnsureDeleted();

        }

        [TestCase(null)]
        public void CountUsername_NullUsernameInserted_ThrowExceptionMessage(string username)
        {

            //Act
            var result = Assert.Throws<ArgumentNullException>(() => _repository.CountUsername(username));

            //Assert
            Assert.AreEqual("Value cannot be null. (Parameter 'Username can not be null!')", result.Message);

        }

        [TestCase("thangdv")]
        public void CountUsername_ValidUsernameInserted_ReturnNumberOfGivenUsername(string username)
        {

            //Act
            var result = _repository.CountUsername(username);

            //Assert
            Assert.AreEqual(0, result);

        }

        [Test]
        public async Task Update_write_to_database()
        {

            //Arrange
            User user = new User
            {
                Id = 1,
                StaffCode = "SD0001",
                FirstName = "Nguyen Van",
                LastName = "Binh",
                DateOfBirth = new DateTime(01 / 20 / 1993),
                JoinedDate = new DateTime(12 / 05 / 2021),
                Gender = Gender.Male,
                Type = UserType.Admin,
                UserName = "binhnv",
                PasswordHash = "binhnv@20011993",
                Location = Location.HaNoi,
                Status = UserStatus.Active,
                NormalizedUserName = "Admin",
                Email = null,
                NormalizedEmail = null,
                EmailConfirmed = true,
                SecurityStamp = string.Empty
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            user.DateOfBirth = new DateTime(01 / 20 / 1999);
            user.JoinedDate = new DateTime(12 / 05 / 2020);
            user.Gender = Gender.Female;


            //Act
            await _repository.Update(user);
            await _context.SaveChangesAsync();
            var result = _context.Users.Count();
            
            //Assert

            Assert.AreEqual(1, _context.Users.Count());
            Assert.AreEqual(new DateTime(01 / 20 / 1999), _context.Users.SingleOrDefault(x => x.Id == 1).DateOfBirth);
            Assert.AreEqual(new DateTime(12 / 05 / 2020), _context.Users.SingleOrDefault(x => x.Id == 1).JoinedDate);
            Assert.AreEqual(Gender.Female, _context.Users.SingleOrDefault(x => x.Id == 1).Gender);
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _context.DisposeAsync();
        }
    }
}