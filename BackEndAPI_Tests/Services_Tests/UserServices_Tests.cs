using System;
using System.Collections.Generic;
using System.Linq;
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
                        Type = UserType.User,
                        UserName = "binhnt",
                        Password = "binhnt@12011994",
                        Location = Location.HaNoi,
                        Status = UserStatus.Active
                    },
                    new User{ Id = 3},
                    new User{ Id = 4},
                    new User{ Id = 5},
                    new User{ Id = 6},
                    new User{ Id = 7},
                    new User{ Id = 8},
                    new User{ Id = 9},
                    new User{ Id = 10},
                    new User{ Id = 11},
                    new User{ Id = 12},
                    new User{ Id = 13},
                    new User{ Id = 14},
                    new User{ Id = 15},
                    new User{ Id = 16},
                    new User{ Id = 17},
                    new User{ Id = 18},
                    new User{ Id = 19},
                    new User{ Id = 20},
                    new User{ Id = 21},
                    new User{ Id = 22},
                    new User{ Id = 23},
                    new User{ Id = 24},
                    new User{ Id = 25},
                    new User{ Id = 26},
                    new User{ Id = 27},
                    new User{ Id = 28},
                    new User{ Id = 29},
                    new User{ Id = 30}
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
        public void GetUsers_WithDefaultValidPaginationParameters_ShouldReturnProperlyPagedListResponse()
        {
            //Arrange
            _userRepositoryMock.Setup(x => x.GetAll()).Returns(Users);
            _assignmentRepositoryMock.Setup(x => x.CountUser(It.IsAny<int>())).Returns(1);
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
            var usersPagedListResponse = userService.GetUsers(parameters);

            //Assert
            Assert.AreEqual(30, usersPagedListResponse.TotalCount);
            Assert.AreEqual(1, usersPagedListResponse.CurrentPage);
            Assert.AreEqual(1, usersPagedListResponse.TotalPages);
            Assert.IsFalse(usersPagedListResponse.HasNext);
            Assert.IsFalse(usersPagedListResponse.HasPrevious);
        }

        [TestCase(10)]
        [TestCase(15)]
        [TestCase(20)]
        public void GetUsers_WithDifferentValidPageSize_ShouldReturnProperlyPagedListResponse(int pageSize)
        {
            //Arrange
            _userRepositoryMock.Setup(x => x.GetAll()).Returns(Users);
            _assignmentRepositoryMock.Setup(x => x.CountUser(It.IsAny<int>())).Returns(1);
            _optionsMock.SetupGet(x => x.Value).Returns(Settings.Value);
            var userService = new UserService(
                _userRepositoryMock.Object,
                _assignmentRepositoryMock.Object,
                _mapper,
                _optionsMock.Object
            );

            var expectedTotalPages = (int)Math.Ceiling(Users.Count() / (double)pageSize);
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
                var usersPagedListResponse = userService.GetUsers(parameters);
                actualCount += usersPagedListResponse.Items.Count();

                //Assert
                Assert.AreEqual(parameters.PageNumber, usersPagedListResponse.CurrentPage);
                Assert.AreEqual(expectedTotalPages, usersPagedListResponse.TotalPages);
                Assert.AreEqual(parameters.PageNumber < expectedTotalPages, usersPagedListResponse.HasNext);
                Assert.AreEqual(parameters.PageNumber > 1, usersPagedListResponse.HasPrevious);
            }

            Assert.AreEqual(Users.Count(), actualCount);

        }

        [Test]
        public void GetUsers_WithNegativePageNumber_ShouldReturnPagedListResponseWithDefaultPageNumberOf1()
        {
            //Arrange
            _userRepositoryMock.Setup(x => x.GetAll()).Returns(Users);
            _assignmentRepositoryMock.Setup(x => x.CountUser(It.IsAny<int>())).Returns(1);
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
            var usersPagedListResponse = userService.GetUsers(parameters);

            //Assert
            Assert.AreEqual(30, usersPagedListResponse.TotalCount);
            Assert.AreEqual(1, usersPagedListResponse.CurrentPage);
            Assert.AreEqual(1, usersPagedListResponse.TotalPages);
            Assert.IsFalse(usersPagedListResponse.HasNext);
            Assert.IsFalse(usersPagedListResponse.HasPrevious);
        }

        [Test]
        public void GetUsers_WithPageSizeSmallerThanMinOf10_ShouldReturnPagedListResponseWithDefaultMinPageSizeOf10()
        {
            //Arrange
            _userRepositoryMock.Setup(x => x.GetAll()).Returns(Users);
            _assignmentRepositoryMock.Setup(x => x.CountUser(It.IsAny<int>())).Returns(1);
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
            var usersPagedListResponse = userService.GetUsers(parameters);

            //Assert
            Assert.AreEqual(30, usersPagedListResponse.TotalCount);
            Assert.AreEqual(1, usersPagedListResponse.CurrentPage);
            Assert.AreEqual(3, usersPagedListResponse.TotalPages);
            Assert.IsTrue(usersPagedListResponse.HasNext);
            Assert.IsFalse(usersPagedListResponse.HasPrevious);
        }

        [Test]
        public void GetUsers_WithPageSizeBiggerThanMaxOf50_ShouldReturnPagedListResponseWithDefaultMinPageSizeOf50()
        {
            //Arrange
            _userRepositoryMock.Setup(x => x.GetAll()).Returns(Users);
            _assignmentRepositoryMock.Setup(x => x.CountUser(It.IsAny<int>())).Returns(1);
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
            var usersPagedListResponse = userService.GetUsers(parameters);

            //Assert
            Assert.AreEqual(30, usersPagedListResponse.TotalCount);
            Assert.AreEqual(1, usersPagedListResponse.CurrentPage);
            Assert.AreEqual(1, usersPagedListResponse.TotalPages);
            Assert.IsFalse(usersPagedListResponse.HasNext);
            Assert.IsFalse(usersPagedListResponse.HasPrevious);
        }

        [TearDown]
        public void TearDown()
        {

        }

    }
}