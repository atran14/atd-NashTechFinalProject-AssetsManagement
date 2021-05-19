using NUnit.Framework;
using Moq;
using BackEndAPI.Interfaces;
using BackEndAPI.Entities;
using BackEndAPI.Enums;
using System;
using System.Threading.Tasks;
using BackEndAPI.Models;
using BackEndAPI.Services;
using AutoMapper;
using Microsoft.Extensions.Options;
using BackEndAPI.Helpers;

namespace BackEndAPI_Tests.Services_Tests
{
    [TestFixture]
    public class UpdateUserService_Test
    {

        private static IOptions<AppSettings> Settings
        {
            get
            {
                return Options.Create<AppSettings>(new AppSettings());
            }
        }
        Mock<IAsyncUserRepository> _userRepositoryMock;
        Mock<IAsyncAssignmentRepository> _assignmentRepositoryMock;
        private static IMapper _mapper;
        Mock<IOptions<AppSettings>> _optionsMock;



        [SetUp]
        public void Setup()
        {
            _userRepositoryMock = new Mock<IAsyncUserRepository>(MockBehavior.Strict);
            _assignmentRepositoryMock = new Mock<IAsyncAssignmentRepository>(MockBehavior.Strict);
            _optionsMock = new Mock<IOptions<AppSettings>>(MockBehavior.Strict);

        }

        [Test]
        public async Task Update_Valid_ShouldBeSuccessful()
        {
            var dontMatterUser = new User { };
            _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).ReturnsAsync(dontMatterUser);
            _userRepositoryMock.Setup(x => x.Update(dontMatterUser)).Returns(Task.CompletedTask).Verifiable();
            _assignmentRepositoryMock.Setup(x => x.GetCountUser(It.IsAny<int>())).Returns(It.IsAny<int>());
            _optionsMock.SetupGet(x => x.Value).Returns(Settings.Value);
            var userService = new UserService(
                _userRepositoryMock.Object,
                _assignmentRepositoryMock.Object,
                _mapper,
                _optionsMock.Object
            );

            var editModel = new EditUserModel
            {
                DateOfBirth = new DateTime(1990, 10, 1),
                Gender = Gender.Male,
                JoinedDate = new DateTime(2000, 1, 3),
                Type = UserType.User
            };

            //Act
            await userService.Update(It.IsAny<int>(), editModel);

            //Assert
            _userRepositoryMock.Verify(x => x.Update(dontMatterUser), Times.Once());
        }

        [Test]
        public async Task Update_DateOfBirthEqualsSundayOrSaturday_ShouldBeFail()
        {
            var dontMatterUser = new User { };
            _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).ReturnsAsync(dontMatterUser);
            _userRepositoryMock.Setup(x => x.Update(dontMatterUser)).Returns(Task.CompletedTask).Verifiable();
            _assignmentRepositoryMock.Setup(x => x.GetCountUser(It.IsAny<int>())).Returns(It.IsAny<int>());
            _optionsMock.SetupGet(x => x.Value).Returns(Settings.Value);
            var userService = new UserService(
                _userRepositoryMock.Object,
                _assignmentRepositoryMock.Object,
                _mapper,
                _optionsMock.Object
            );

            var editModel = new EditUserModel
            {
                DateOfBirth = new DateTime(1999, 10, 1),
                Gender = Gender.Male,
                JoinedDate = new DateTime(2021, 5, 9),
                Type = UserType.User
            };

            //Act
            var exception = Assert.ThrowsAsync<Exception>(
               async () =>
               {
                   await userService.Update(It.IsAny<int>(), editModel);
               }
           );

            //Assert
            Assert.AreSame(exception.Message, "Join Date is Saturday or Sunday. Please select different date");
        }

        [Test]
        public async Task Update_JoinDatedIsNotLaterThanDateOfBirth_ShouldBeFail()
        {
            var dontMatterUser = new User { };
            _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).ReturnsAsync(dontMatterUser);
            _userRepositoryMock.Setup(x => x.Update(dontMatterUser)).Returns(Task.CompletedTask).Verifiable();
            _assignmentRepositoryMock.Setup(x => x.GetCountUser(It.IsAny<int>())).Returns(It.IsAny<int>());
            _optionsMock.SetupGet(x => x.Value).Returns(Settings.Value);
            var userService = new UserService(
                _userRepositoryMock.Object,
                _assignmentRepositoryMock.Object,
                _mapper,
                _optionsMock.Object
            );

            var editModel = new EditUserModel
            {
                DateOfBirth = new DateTime(2001, 10, 1),
                Gender = Gender.Male,
                JoinedDate = new DateTime(2000, 5, 5),
                Type = UserType.User
            };

            //Act
            var exception = Assert.ThrowsAsync<Exception>(
               async () =>
               {
                   await userService.Update(It.IsAny<int>(), editModel);
               }
           );

            //Assert
            Assert.AreSame(exception.Message, "Join Date is not later than Date Of Birth. Please select different date");
        }


        [Test]
        public async Task Update_UserIsUnder18_ShouldBeFail()
        {
            var dontMatterUser = new User { };
            _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).ReturnsAsync(dontMatterUser);
            _userRepositoryMock.Setup(x => x.Update(dontMatterUser)).Returns(Task.CompletedTask).Verifiable();
            _assignmentRepositoryMock.Setup(x => x.GetCountUser(It.IsAny<int>())).Returns(It.IsAny<int>());
            _optionsMock.SetupGet(x => x.Value).Returns(Settings.Value);
            var userService = new UserService(
                _userRepositoryMock.Object,
                _assignmentRepositoryMock.Object,
                _mapper,
                _optionsMock.Object
            );

            var editModel = new EditUserModel
            {
                DateOfBirth = new DateTime(2006, 10, 1),
                Gender = Gender.Male,
                JoinedDate = new DateTime(2021, 5, 5),
                Type = UserType.User
            };

            //Act
            var exception = Assert.ThrowsAsync<Exception>(
               async () =>
               {
                   await userService.Update(It.IsAny<int>(), editModel);
               }
           );

            //Assert
            Assert.AreSame(exception.Message, "User is under 18. Please select different date");
        }

        [TearDown]
        public void TearDown()
        {

        }

    }
}