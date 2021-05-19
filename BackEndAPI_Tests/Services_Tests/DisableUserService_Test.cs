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
using System.Linq;
using System.Collections.Generic;

namespace BackEndAPI_Tests.Services_Tests
{
    [TestFixture]
    public class DisableUserService_Test
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

        [SetUp]
        public void Setup()
        {
            _userRepositoryMock = new Mock<IAsyncUserRepository>(MockBehavior.Strict);
            _assignmentRepositoryMock = new Mock<IAsyncAssignmentRepository>(MockBehavior.Strict);
            _optionsMock = new Mock<IOptions<AppSettings>>(MockBehavior.Strict);

        }
        [Test]
        public async Task Disable_Valid_ShouldBeSuccessful()
        {
            var User = new User { };
            _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).ReturnsAsync(User);
            _userRepositoryMock.Setup(x => x.Update(User)).Returns(Task.CompletedTask).Verifiable();
            _assignmentRepositoryMock.Setup(x => x.GetCountUser(It.IsAny<int>())).Returns(It.IsAny<int>());
            _optionsMock.SetupGet(x => x.Value).Returns(Settings.Value);
            var userService = new UserService(
                _userRepositoryMock.Object,
                _assignmentRepositoryMock.Object,
                _mapper,
                _optionsMock.Object
            );

            //Act
            await userService.Disable(It.IsAny<int>());

            //Assert
            _userRepositoryMock.Verify(x => x.Update(User), Times.Once());
            Assert.AreEqual(UserStatus.Disabled, User.Status);
        }
        [Test]
        public async Task Disable_ValidInAssignment_ShouldBeFail()
        {
            var User = new User { };
            _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).ReturnsAsync(User);
            _userRepositoryMock.Setup(x => x.Update(User)).Returns(Task.CompletedTask).Verifiable();
            _assignmentRepositoryMock.Setup(x => x.GetCountUser(It.IsAny<int>())).Returns(1);
            _optionsMock.SetupGet(x => x.Value).Returns(Settings.Value);
            var userService = new UserService(
                _userRepositoryMock.Object,
                _assignmentRepositoryMock.Object,
                _mapper,
                _optionsMock.Object
            );


            //Act
            var exception = Assert.ThrowsAsync<ArgumentException>(
                async () =>
                {
                    await userService.Disable(It.IsAny<int>());
                }
             );


            //Assert
            Assert.AreSame("User is still valid assignment", exception.Message);
        }

        [Test]
        public async Task Disable_NotFoundId_ShouldBeFail()
        {
            var User = new User { };
            _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(Task.FromResult<User>(null));
            _userRepositoryMock.Setup(x => x.Update(User)).Returns(Task.CompletedTask).Verifiable();
            _assignmentRepositoryMock.Setup(x => x.GetCountUser(It.IsAny<int>())).Returns(It.IsAny<int>());
            _optionsMock.SetupGet(x => x.Value).Returns(Settings.Value);
            var userService = new UserService(
                _userRepositoryMock.Object,
                _assignmentRepositoryMock.Object,
                _mapper,
                _optionsMock.Object
            );


            //Act
            var exception = Assert.ThrowsAsync<InvalidOperationException>(
                async () =>
                {
                    await userService.Disable(It.IsAny<int>());
                }
             );


            //Assert
            Assert.AreSame("Can not find user", exception.Message);
        }

    }
}