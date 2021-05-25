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
            _userRepositoryMock.Setup(x => x.CountAdminRemain()).Returns(2);
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
            await userService.Disable(1,2);

            //Assert
            _userRepositoryMock.Verify(x => x.Update(User), Times.Once());
            Assert.AreEqual(UserStatus.Disabled, User.Status);
        }
        [Test]
        public void Disable_ValidInAssignment_ShouldThrowToException()
        {
            var User = new User { };
            _userRepositoryMock.Setup(x => x.CountAdminRemain()).Returns(2);
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
                    await userService.Disable(1,2);
                }
             );


            //Assert
            Assert.AreSame("User is still valid assignment", exception.Message);
        }

        [Test]
        public void Disable_NotFoundId_ShouldThrowToException()
        {
            var User = new User { };
            _userRepositoryMock.Setup(x => x.CountAdminRemain()).Returns(2);
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
                    await userService.Disable(1,2);
                }
             );


            //Assert
            Assert.AreSame("Can not find user", exception.Message);
        }
        [Test]
        public void Disable_DisableYourself_ShouldThrowToException()
        {
            var User = new User { };
            _userRepositoryMock.Setup(x => x.CountAdminRemain()).Returns(2);
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
            var exception = Assert.ThrowsAsync<Exception>(
                async () =>
                {
                    await userService.Disable(1,1);
                }
             );


            //Assert
            Assert.AreSame("Can not disable yourself", exception.Message);
        }

        [Test]
        public void Disable_HasOnlyOneAdminRemain_ShouldThrowToException()
        {
            var User = new User { };
            _userRepositoryMock.Setup(x => x.CountAdminRemain()).Returns(1);
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
            var exception = Assert.ThrowsAsync<Exception>(
                async () =>
                {
                    await userService.Disable(1,2);
                }
             );


            //Assert
            Assert.AreSame("System has only one admin remain", exception.Message);
        }


    }
}