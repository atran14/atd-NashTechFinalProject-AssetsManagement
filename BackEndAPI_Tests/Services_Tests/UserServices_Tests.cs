using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BackEndAPI.Entities;
using BackEndAPI.Enums;
using BackEndAPI.Helpers;
using BackEndAPI.Interfaces;
using BackEndAPI.Models;
using BackEndAPI.Services;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;

namespace BackEndAPI_Tests.Services_Tests
{
    [TestFixture]
    public class UserServices_Tests
    {

        private static IQueryable<User> Users
        {
            get
            {
                return new List<User>
                {
                    new User
                    {
                        Id = 1,
                        StaffCode = "SD0001",
                        FirstName = "Binh",
                        LastName = "Nguyen Van",
                        DateOfBirth = new DateTime(1993, 01, 20),
                        JoinedDate = new DateTime(2021, 12, 05),
                        Gender = Gender.Male,
                        Type = UserType.Admin,
                        UserName = "binhnv",
                        Password = "binhnv@20011993",
                        Location = Location.HaNoi,
                        Status = UserStatus.Active
                    },
                    new User
                    {
                        Id = 2,
                        StaffCode = "SD0002",
                        FirstName = "Binh",
                        LastName = "Nguyen Thi",
                        DateOfBirth = new DateTime(1994, 01, 12).Date,
                        JoinedDate = new DateTime(2021, 12, 05).Date,
                        Gender = Gender.Female,
                        Type = UserType.Admin,
                        UserName = "binhnt",
                        Password = "binhnt@12011994",
                        Location = Location.HoChiMinh,
                        Status = UserStatus.Active
                    },
                    new User{ Id = 3, Location = Location.HaNoi, Type = UserType.User},
                    new User{ Id = 4, Location = Location.HaNoi, Type = UserType.User},
                    new User{ Id = 5, Location = Location.HaNoi, Type = UserType.User},
                    new User{ Id = 6, Location = Location.HaNoi, Type = UserType.User},
                    new User{ Id = 7, Location = Location.HaNoi, Type = UserType.User},
                    new User{ Id = 8, Location = Location.HaNoi, Type = UserType.User},
                    new User{ Id = 9, Location = Location.HaNoi, Type = UserType.User},
                    new User{ Id = 10, Location = Location.HaNoi, Type = UserType.User},
                    new User{ Id = 11, Location = Location.HaNoi, Type = UserType.User},
                    new User{ Id = 12, Location = Location.HaNoi, Type = UserType.User},
                    new User{ Id = 13, Location = Location.HaNoi, Type = UserType.User},
                    new User{ Id = 14, Location = Location.HaNoi, Type = UserType.User},
                    new User{ Id = 15, Location = Location.HaNoi, Type = UserType.User},
                    new User{ Id = 16, Location = Location.HaNoi, Type = UserType.User},
                    new User{ Id = 17, Location = Location.HaNoi, Type = UserType.User},
                    new User{ Id = 18, Location = Location.HaNoi, Type = UserType.User},
                    new User{ Id = 19, Location = Location.HaNoi, Type = UserType.User},
                    new User{ Id = 20, Location = Location.HaNoi, Type = UserType.User},
                    new User{ Id = 21, Location = Location.HoChiMinh, Type = UserType.User},
                    new User{ Id = 22, Location = Location.HoChiMinh, Type = UserType.User},
                    new User{ Id = 23, Location = Location.HoChiMinh, Type = UserType.User},
                    new User{ Id = 24, Location = Location.HoChiMinh, Type = UserType.User},
                    new User{ Id = 25, Location = Location.HoChiMinh, Type = UserType.User},
                    new User{ Id = 26, Location = Location.HoChiMinh, Type = UserType.User},
                    new User{ Id = 27, Location = Location.HoChiMinh, Type = UserType.User},
                    new User{ Id = 28, Location = Location.HoChiMinh, Type = UserType.User},
                    new User{ Id = 29, Location = Location.HoChiMinh, Type = UserType.User},
                    new User{ Id = 30, Location = Location.HoChiMinh, Type = UserType.User}
                }
                .AsQueryable();
            }
        }

        private static IQueryable<Assignment> Assignments
        {
            get
            {
                return new List<Assignment>
                {
                    new Assignment
                    {
                        Id = 1,
                        AssetId = 1,
                        AssignedByUserId = 1,
                        AssignedToUserId = 2,
                        Note = "Testing1",
                        State = AssignmentState.WaitingForAcceptance
                    }
                }
                .AsQueryable();
            }
        }

        private static IMapper _mapper;

        private static IOptions<AppSettings> Settings
        {
            get
            {
                return Options.Create<AppSettings>(new AppSettings());
            }
        }

        private Mock<IAsyncUserRepository> _userRepositoryMock;
        private Mock<IAsyncAssignmentRepository> _assignmentRepositoryMock;

        private Mock<IOptions<AppSettings>> _optionsMock;

        public UserServices_Tests()
        {
            if (_mapper == null)
            {
                var mappingConfig = new MapperConfiguration(mc =>
                {
                    mc.AddProfile(new AutoMapperProfile());
                });
                IMapper mapper = mappingConfig.CreateMapper();
                _mapper = mapper;
            }
        }

        [SetUp]
        public void SetUp()
        {
            _userRepositoryMock = new Mock<IAsyncUserRepository>(behavior: MockBehavior.Strict);
            _assignmentRepositoryMock = new Mock<IAsyncAssignmentRepository>(behavior: MockBehavior.Strict);
            _optionsMock = new Mock<IOptions<AppSettings>>(behavior: MockBehavior.Strict);
        }

        [Test]
        public async Task GetUsers_WithDefaultValidPaginationParameters_ShouldReturnProperlyPagedListResponse()
        {
            //Arrange
            _userRepositoryMock.Setup(x => x.GetAll()).Returns(Users);
            _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).ReturnsAsync(Users.Single(u => u.Id == 1));
            _assignmentRepositoryMock.Setup(x => x.GetCountUser(It.IsAny<int>())).Returns(1);
            _optionsMock.SetupGet(x => x.Value).Returns(Settings.Value);
            var userService = new UserService(
                _userRepositoryMock.Object,
                _assignmentRepositoryMock.Object,
                _mapper,
                _optionsMock.Object
            );

            PaginationParameters parameters = new PaginationParameters
            {
                PageNumber = 1,
                PageSize = 30
            };

            //Act
            var usersPagedListResponse = await userService.GetUsers(parameters, 1);

            //Assert
            var expectedCount = Users.Where(u => u.Location == Location.HaNoi).Count();
            Assert.AreEqual(expectedCount, usersPagedListResponse.TotalCount);
            Assert.AreEqual(1, usersPagedListResponse.CurrentPage);
            Assert.AreEqual(1, usersPagedListResponse.TotalPages);
            Assert.IsFalse(usersPagedListResponse.HasNext);
            Assert.IsFalse(usersPagedListResponse.HasPrevious);
        }

        [TestCase(10)]
        [TestCase(15)]
        [TestCase(20)]
        public async Task GetUsers_WithDifferentValidPageSize_ShouldReturnProperlyPagedListResponse(int pageSize)
        {
            //Arrange
            _userRepositoryMock.Setup(x => x.GetAll()).Returns(Users);
            _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).ReturnsAsync(Users.Single(u => u.Id == 1));
            _assignmentRepositoryMock.Setup(x => x.GetCountUser(It.IsAny<int>())).Returns(1);
            _optionsMock.SetupGet(x => x.Value).Returns(Settings.Value);
            var userService = new UserService(
                _userRepositoryMock.Object,
                _assignmentRepositoryMock.Object,
                _mapper,
                _optionsMock.Object
            );
            var expectedCount = Users.Where(u => u.Location == Location.HaNoi).Count();


            var expectedTotalPages = (int)Math.Ceiling(expectedCount / (double)pageSize);
            List<PaginationParameters> parametersList = new List<PaginationParameters>();
            for (int i = 1; i <= expectedTotalPages; i++)
            {
                parametersList.Add(
                    new PaginationParameters
                    {
                        PageNumber = i,
                        PageSize = pageSize
                    }
                );
            }

            int actualCount = 0;
            foreach (var parameters in parametersList)
            {
                //Act
                var usersPagedListResponse = await userService.GetUsers(parameters, 1);
                actualCount += usersPagedListResponse.Items.Count();

                //Assert
                Assert.AreEqual(parameters.PageNumber, usersPagedListResponse.CurrentPage);
                Assert.AreEqual(expectedTotalPages, usersPagedListResponse.TotalPages);
                Assert.AreEqual(parameters.PageNumber < expectedTotalPages, usersPagedListResponse.HasNext);
                Assert.AreEqual(parameters.PageNumber > 1, usersPagedListResponse.HasPrevious);
            }

            Assert.AreEqual(expectedCount, actualCount);

        }

        [Test]
        public async Task GetUsers_WithNegativePageNumber_ShouldReturnPagedListResponseWithDefaultPageNumberOf1()
        {
            //Arrange
            _userRepositoryMock.Setup(x => x.GetAll()).Returns(Users);
            _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).ReturnsAsync(Users.Single(u => u.Id == 1));
            _assignmentRepositoryMock.Setup(x => x.GetCountUser(It.IsAny<int>())).Returns(1);
            _optionsMock.SetupGet(x => x.Value).Returns(Settings.Value);
            var userService = new UserService(
                _userRepositoryMock.Object,
                _assignmentRepositoryMock.Object,
                _mapper,
                _optionsMock.Object
            );

            PaginationParameters parameters = new PaginationParameters
            {
                PageNumber = -1,
                PageSize = 30
            };

            //Act
            var usersPagedListResponse = await userService.GetUsers(parameters, 1);

            //Assert
            var expectedCount = Users.Where(u => u.Location == Location.HaNoi).Count();
            Assert.AreEqual(expectedCount, usersPagedListResponse.TotalCount);
            Assert.AreEqual(1, usersPagedListResponse.CurrentPage);
            Assert.AreEqual(1, usersPagedListResponse.TotalPages);
            Assert.IsFalse(usersPagedListResponse.HasNext);
            Assert.IsFalse(usersPagedListResponse.HasPrevious);
        }

        [Test]
        public async Task GetUsers_WithPageSizeSmallerThanMinOf10_ShouldReturnPagedListResponseWithDefaultMinPageSizeOf10()
        {
            //Arrange
            _userRepositoryMock.Setup(x => x.GetAll()).Returns(Users);
            _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).ReturnsAsync(Users.Single(u => u.Id == 1));
            _assignmentRepositoryMock.Setup(x => x.GetCountUser(It.IsAny<int>())).Returns(1);
            _optionsMock.SetupGet(x => x.Value).Returns(Settings.Value);
            var userService = new UserService(
                _userRepositoryMock.Object,
                _assignmentRepositoryMock.Object,
                _mapper,
                _optionsMock.Object
            );

            PaginationParameters parameters = new PaginationParameters
            {
                PageNumber = 1,
                PageSize = 0
            };

            //Act
            var usersPagedListResponse = await userService.GetUsers(parameters, 1);

            //Assert
            var expectedCount = Users.Where(u => u.Location == Location.HaNoi).Count();
            var expectedTotalPages = (int)Math.Ceiling(expectedCount / 10.0);
            Assert.AreEqual(expectedCount, usersPagedListResponse.TotalCount);
            Assert.AreEqual(1, usersPagedListResponse.CurrentPage);
            Assert.AreEqual(expectedTotalPages, usersPagedListResponse.TotalPages);
            Assert.IsTrue(usersPagedListResponse.HasNext);
            Assert.IsFalse(usersPagedListResponse.HasPrevious);
        }

        [Test]
        public async Task GetUsers_WithPageSizeBiggerThanMaxOf50_ShouldReturnPagedListResponseWithDefaultMinPageSizeOf50()
        {
            //Arrange
            _userRepositoryMock.Setup(x => x.GetAll()).Returns(Users);
            _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).ReturnsAsync(Users.Single(u => u.Id == 1));
            _assignmentRepositoryMock.Setup(x => x.GetCountUser(It.IsAny<int>())).Returns(1);
            _optionsMock.SetupGet(x => x.Value).Returns(Settings.Value);
            var userService = new UserService(
                _userRepositoryMock.Object,
                _assignmentRepositoryMock.Object,
                _mapper,
                _optionsMock.Object
            );

            PaginationParameters parameters = new PaginationParameters
            {
                PageNumber = 1,
                PageSize = 100
            };

            //Act
            var usersPagedListResponse = await userService.GetUsers(parameters, 1);

            //Assert
            var expectedCount = Users.Where(u => u.Location == Location.HaNoi).Count();
            Assert.AreEqual(expectedCount, usersPagedListResponse.TotalCount);
            Assert.AreEqual(1, usersPagedListResponse.CurrentPage);
            Assert.AreEqual(1, usersPagedListResponse.TotalPages);
            Assert.IsFalse(usersPagedListResponse.HasNext);
            Assert.IsFalse(usersPagedListResponse.HasPrevious);
        }

        [TestCase(1)] //for Hanoi
        [TestCase(2)] //for HoChiMinh
        public async Task GetUsers_TwoAdminsFromTwoDifferentLocations_ShouldReturnPagedListResponseOfUsersOfSameLocation(int adminId)
        {
            //Arrange
            _userRepositoryMock.Setup(x => x.GetAll()).Returns(Users);
            _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).ReturnsAsync(Users.Single(u => u.Id == adminId));
            _assignmentRepositoryMock.Setup(x => x.GetCountUser(It.IsAny<int>())).Returns(1);
            _optionsMock.SetupGet(x => x.Value).Returns(Settings.Value);
            var userService = new UserService(
                _userRepositoryMock.Object,
                _assignmentRepositoryMock.Object,
                _mapper,
                _optionsMock.Object
            );
            var adminUser = Users.Single(u => u.Id == adminId);

            PaginationParameters parameters = new PaginationParameters
            {
                PageNumber = 1,
                PageSize = 30
            };

            //Act
            var usersPagedListResponse = await userService.GetUsers(parameters, 1);

            //Assert
            var expectedCount = Users.Where(u => u.Location == adminUser.Location).Count();
            Assert.AreEqual(expectedCount, usersPagedListResponse.TotalCount);
            Assert.AreEqual(1, usersPagedListResponse.CurrentPage);
            Assert.AreEqual(1, usersPagedListResponse.TotalPages);
            Assert.IsFalse(usersPagedListResponse.HasNext);
            Assert.IsFalse(usersPagedListResponse.HasPrevious);
        }

        [Test]
        public void GetUsers_UserNotAdmin_ShouldThrowException()
        {
            _userRepositoryMock.Setup(x => x.GetAll()).Returns(Users);
            _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).ReturnsAsync(new User { Type = UserType.User });
            _assignmentRepositoryMock.Setup(x => x.GetCountUser(It.IsAny<int>())).Returns(1);
            _optionsMock.SetupGet(x => x.Value).Returns(Settings.Value);
            var userService = new UserService(
                _userRepositoryMock.Object,
                _assignmentRepositoryMock.Object,
                _mapper,
                _optionsMock.Object
            );

            PaginationParameters parameters = new PaginationParameters
            {
                PageNumber = 1,
                PageSize = 100
            };

            //Act
            var exception = Assert.ThrowsAsync<Exception>(
                async () =>
                {
                    await userService.GetUsers(parameters, 70);
                }
            );

            Assert.AreEqual("Unauthorized access", exception.Message);
        }

        [TearDown]
        public void TearDown()
        {

        }

    }
}