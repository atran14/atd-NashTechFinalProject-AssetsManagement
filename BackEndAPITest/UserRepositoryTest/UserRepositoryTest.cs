using System;
using System.Linq;
using System.Threading.Tasks;
using System.Security.AccessControl;
using BackEndAPI.DBContext;
using BackEndAPI.Entities;
using BackEndAPI.Interfaces;
using BackEndAPI.Repositories;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;


namespace BackEndAPITest.UserRepositoryTest
{
    [TestFixture]
    public class UserRepositoryTest
    {

        private AssetsManagementDBContext _context;
        private IAsyncUserRepository _repository;

        [OneTimeSetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<AssetsManagementDBContext>()
                   .UseInMemoryDatabase(databaseName: "Update_write_to_database()")
                .Options;

            _context = new AssetsManagementDBContext(options);
            _repository = new UserRepository(_context);
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
            _context.SaveChangesAsync();

            user.DateOfBirth = new DateTime(01 / 20 / 1999);
            user.JoinedDate = new DateTime(12 / 05 / 2020);
            user.Gender = Gender.Female;


            //Act
            await _repository.Update(user);
            await _context.SaveChangesAsync();

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