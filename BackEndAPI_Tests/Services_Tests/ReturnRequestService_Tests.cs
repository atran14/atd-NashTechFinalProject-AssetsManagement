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
    public class ReturnRequestService_Tests
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
                    new User{ Id = 3, UserName = "abc001", Location = Location.HaNoi, Type = UserType.User}
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
                        State = AssignmentState.Accepted
                    }
                }
                .AsQueryable();
            }
        }

        private static IQueryable<ReturnRequest> ReturnRequests
        {
            get
            {
                return new List<ReturnRequest>
                {
                    new ReturnRequest
                    {
                        Id = 1,
                        AssignmentId = 1,
                        Assignment = new Assignment
                        {
                            Id = 1,
                            AssetId = 1,
                            AssignedByUserId = 1,
                            AssignedToUserId = 2,
                            Note = "Testing1",
                            State = AssignmentState.Accepted
                        },
                        AcceptedByUserId = 2,
                        RequestedByUserId = 2,
                        ReturnedDate = DateTime.UtcNow,
                        State = RequestState.Completed
                    },
                    new ReturnRequest
                    {
                        Id = 2,
                        AssignmentId = 2,
                        Assignment = new Assignment
                        {
                            Id = 2,
                            AssetId = 1,
                            AssignedByUserId = 1,
                            AssignedToUserId = 2,
                            Note = "Testing2",
                            State = AssignmentState.WaitingForAcceptance
                        },
                        RequestedByUserId = 2,
                        State = RequestState.WaitingForReturning
                    }
                }
                .AsQueryable();
            }
        }


        private Mock<IAsyncReturnRequestRepository> _returnRequestRepositoryMock;
        private Mock<IAsyncUserRepository> _userRepositoryMock;
        private Mock<IAsyncAssignmentRepository> _assignmentRepositoryMock;
        private readonly IMapper _mapper;

        public ReturnRequestService_Tests()
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
            _returnRequestRepositoryMock = new Mock<IAsyncReturnRequestRepository>(behavior: MockBehavior.Strict);
        }

        public async Task GetAll_WithDefaultValidPaginationParameters_ShouldReturnProperlyPagedListResponse()
        {
            //Arrange
            _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).ReturnsAsync(Users.Single(u => u.Id == 1));
            _returnRequestRepositoryMock.Setup(x => x.GetAll()).Returns(ReturnRequests);
            _assignmentRepositoryMock.Setup(x => x.GetCountUser(It.IsAny<int>())).Returns(1);
            var service = new ReturnRequestService(
                _userRepositoryMock.Object,
                _returnRequestRepositoryMock.Object,
                _assignmentRepositoryMock.Object,
                _mapper
            );

            PaginationParameters parameters = new PaginationParameters
            {
                PageNumber = 1,
                PageSize = 30
            };

            //Act
            var returnRequestsPagedListResponse = await service.GetAll(parameters, 1);

            //Assert
            var expectedCount = ReturnRequests.Count();
            Assert.AreEqual(expectedCount, returnRequestsPagedListResponse.TotalCount);
            Assert.AreEqual(1, returnRequestsPagedListResponse.CurrentPage);
            Assert.AreEqual(1, returnRequestsPagedListResponse.TotalPages);
            Assert.IsFalse(returnRequestsPagedListResponse.HasNext);
            Assert.IsFalse(returnRequestsPagedListResponse.HasPrevious);
        }

        [Test]
        public void GetAll_UserNotAdmin_ShouldThrowException()
        {
            //Arrange
            _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).ReturnsAsync(new User { Type = UserType.User });
            _returnRequestRepositoryMock.Setup(x => x.GetAll()).Returns(It.IsAny<IQueryable<ReturnRequest>>()).Verifiable();
            var service = new ReturnRequestService(
                _userRepositoryMock.Object,
                _returnRequestRepositoryMock.Object,
                _assignmentRepositoryMock.Object,
                _mapper
            );

            PaginationParameters parameters = new PaginationParameters
            {
                PageNumber = 1,
                PageSize = 30
            };

            //Act
            var exception = Assert.ThrowsAsync<Exception>(
                async () =>
                {
                    await service.GetAll(parameters, 70);
                }
            );

            //Assert
            Assert.AreEqual(Message.UnauthorizedUser, exception.Message);
            _returnRequestRepositoryMock.Verify(x => x.GetAll(), Times.Never());
        }

        [Test]
        public async Task Filter_NoFilterParamsProvided_ShouldReturnCorrectFilteredResults()
        {
            //Arrange
            var returnRequests = new List<ReturnRequest>
            {
                new ReturnRequest
                {
                    Assignment = new Assignment
                    {
                        AssignedByUserId = 1
                    },
                    State = RequestState.Completed,
                    ReturnedDate = DateTime.Now
                },
                new ReturnRequest
                {
                    Assignment = new Assignment
                    {
                        AssignedByUserId = 1
                    },
                    State = RequestState.Completed,
                    ReturnedDate = DateTime.Now
                },
                new ReturnRequest
                {
                    Assignment = new Assignment
                    {
                        AssignedByUserId = 1
                    },
                    State = RequestState.WaitingForReturning
                },
                new ReturnRequest
                {
                    Assignment = new Assignment
                    {
                        AssignedByUserId = 1
                    },
                    State = RequestState.WaitingForReturning
                }
            }
            .AsQueryable();

            var filterParams_NoneProvided = new ReturnRequestFilterParameters
            { };
            var filterParams_OnlyRequestStateProvided = new ReturnRequestFilterParameters
            {
                RequestState = RequestState.Completed
            };
            var filterParams_OnlyReturnDateProvided = new ReturnRequestFilterParameters
            {
                ReturnedDate = DateTime.Now
            };
            var filterParams_BothProvided = new ReturnRequestFilterParameters
            {
                RequestState = RequestState.Completed,
                ReturnedDate = DateTime.Now
            };

            _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).ReturnsAsync(new User { Id = 1, Type = UserType.Admin });
            _returnRequestRepositoryMock.Setup(x => x.GetAll()).Returns(returnRequests);
            _assignmentRepositoryMock.Setup(x => x.GetCountUser(It.IsAny<int>())).Returns(1);
            var service = new ReturnRequestService(
                _userRepositoryMock.Object,
                _returnRequestRepositoryMock.Object,
                _assignmentRepositoryMock.Object,
                _mapper
            );

            PaginationParameters parameters = new PaginationParameters
            {
                PageNumber = 1,
                PageSize = 30
            };

            //Act
            var actualFilteredResponse = await service.Filter(
                parameters,
                It.IsAny<int>(),
                filterParams_NoneProvided
            );

            //Assert
            Assert.AreEqual(4, actualFilteredResponse.TotalCount);
        }

        [Test]
        public async Task Filter_OnlyRequestStateFilterParamsProvided_ShouldReturnCorrectFilteredResults()
        {
            //Arrange
            var returnRequests = new List<ReturnRequest>
            {
                new ReturnRequest
                {
                    Assignment = new Assignment
                    {
                        AssignedByUserId = 1
                    },
                    State = RequestState.Completed,
                    ReturnedDate = DateTime.Now
                },
                new ReturnRequest
                {
                    Assignment = new Assignment
                    {
                        AssignedByUserId = 1
                    },
                    State = RequestState.Completed,
                    ReturnedDate = DateTime.Now
                },
                new ReturnRequest
                {
                    Assignment = new Assignment
                    {
                        AssignedByUserId = 1
                    },
                    State = RequestState.WaitingForReturning
                },
                new ReturnRequest
                {
                    Assignment = new Assignment
                    {
                        AssignedByUserId = 1
                    },
                    State = RequestState.WaitingForReturning
                }
            }
            .AsQueryable();

            var filterParams_OnlyRequestStateProvided = new ReturnRequestFilterParameters
            {
                RequestState = RequestState.Completed
            };

            _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).ReturnsAsync(new User { Id = 1, Type = UserType.Admin });
            _returnRequestRepositoryMock.Setup(x => x.GetAll()).Returns(returnRequests);
            _assignmentRepositoryMock.Setup(x => x.GetCountUser(It.IsAny<int>())).Returns(1);
            var service = new ReturnRequestService(
                _userRepositoryMock.Object,
                _returnRequestRepositoryMock.Object,
                _assignmentRepositoryMock.Object,
                _mapper
            );

            PaginationParameters parameters = new PaginationParameters
            {
                PageNumber = 1,
                PageSize = 30
            };

            //Act
            var actualFilteredResponse = await service.Filter(
                parameters,
                It.IsAny<int>(),
                filterParams_OnlyRequestStateProvided
            );

            //Assert
            Assert.AreEqual(2, actualFilteredResponse.TotalCount);
        }

        [Test]
        public async Task Filter_OnlyReturnedDateFilterParamsProvided_ShouldReturnCorrectFilteredResults()
        {
            //Arrange
            var returnRequests = new List<ReturnRequest>
            {
                new ReturnRequest
                {
                    Assignment = new Assignment
                    {
                        AssignedByUserId = 1
                    },
                    State = RequestState.Completed,
                    ReturnedDate = DateTime.Now.Date
                },
                new ReturnRequest
                {
                    Assignment = new Assignment
                    {
                        AssignedByUserId = 1
                    },
                    State = RequestState.Completed,
                    ReturnedDate = DateTime.Now
                },
                new ReturnRequest
                {
                    Assignment = new Assignment
                    {
                        AssignedByUserId = 1
                    },
                    State = RequestState.WaitingForReturning,
                },
                new ReturnRequest
                {
                    Assignment = new Assignment
                    {
                        AssignedByUserId = 1
                    },
                    State = RequestState.WaitingForReturning,
                }
            }
            .AsQueryable();

            var filterParams_OnlyReturnDateProvided = new ReturnRequestFilterParameters
            {
                ReturnedDate = DateTime.Now
            };

            _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).ReturnsAsync(new User { Id = 1, Type = UserType.Admin });
            _returnRequestRepositoryMock.Setup(x => x.GetAll()).Returns(returnRequests);
            _assignmentRepositoryMock.Setup(x => x.GetCountUser(It.IsAny<int>())).Returns(1);
            var service = new ReturnRequestService(
                _userRepositoryMock.Object,
                _returnRequestRepositoryMock.Object,
                _assignmentRepositoryMock.Object,
                _mapper
            );

            PaginationParameters parameters = new PaginationParameters
            {
                PageNumber = 1,
                PageSize = 30
            };

            //Act
            var actualFilteredResponse = await service.Filter(
                parameters,
                It.IsAny<int>(),
                filterParams_OnlyReturnDateProvided
            );

            //Assert
            Assert.AreEqual(2, actualFilteredResponse.TotalCount);
        }

        [Test]
        public async Task Filter_BothFilterParamsProvided_ShouldReturnCorrectFilteredResults()
        {
            //Arrange
            var returnRequests = new List<ReturnRequest>
            {
                new ReturnRequest
                {
                    Assignment = new Assignment
                    {
                        AssignedByUserId = 1
                    },
                    State = RequestState.Completed,
                    ReturnedDate = DateTime.Now
                },
                new ReturnRequest
                {
                    Assignment = new Assignment
                    {
                        AssignedByUserId = 1
                    },
                    State = RequestState.Completed,
                    ReturnedDate = DateTime.Now
                },
                new ReturnRequest
                {
                    Assignment = new Assignment
                    {
                        AssignedByUserId = 1
                    },
                    State = RequestState.WaitingForReturning
                },
                new ReturnRequest
                {
                    Assignment = new Assignment
                    {
                        AssignedByUserId = 1
                    },
                    State = RequestState.WaitingForReturning
                }
            }
            .AsQueryable();

            var filterParams_BothProvided = new ReturnRequestFilterParameters
            {
                RequestState = RequestState.Completed,
                ReturnedDate = DateTime.Now
            };

            _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).ReturnsAsync(new User { Id = 1, Type = UserType.Admin });
            _returnRequestRepositoryMock.Setup(x => x.GetAll()).Returns(returnRequests);
            _assignmentRepositoryMock.Setup(x => x.GetCountUser(It.IsAny<int>())).Returns(1);
            var service = new ReturnRequestService(
                _userRepositoryMock.Object,
                _returnRequestRepositoryMock.Object,
                _assignmentRepositoryMock.Object,
                _mapper
            );

            PaginationParameters parameters = new PaginationParameters
            {
                PageNumber = 1,
                PageSize = 30
            };

            //Act
            var actualFilteredResponse = await service.Filter(
                parameters,
                It.IsAny<int>(),
                filterParams_BothProvided
            );

            //Assert
            Assert.AreEqual(2, actualFilteredResponse.TotalCount);
        }

        [Test]
        public void Search_UserNotAdmin_ShouldThrowException()
        {
            //Arrange
            _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).ReturnsAsync(new User { Type = UserType.User });
            _returnRequestRepositoryMock.Setup(x => x.GetAll()).Returns(It.IsAny<IQueryable<ReturnRequest>>()).Verifiable();
            var service = new ReturnRequestService(
                _userRepositoryMock.Object,
                _returnRequestRepositoryMock.Object,
                _assignmentRepositoryMock.Object,
                _mapper
            );

            //Act
            var exception = Assert.ThrowsAsync<Exception>(
                async () =>
                {
                    await service.Search(
                        It.IsAny<PaginationParameters>(),
                        It.IsAny<int>(),
                        It.IsAny<string>()
                    );
                }
            );

            //Assert
            Assert.AreEqual(Message.UnauthorizedUser, exception.Message);
            _returnRequestRepositoryMock.Verify(x => x.GetAll(), Times.Never());
        }

        [Test]
        public async Task Search_ByUserName_ShouldReturnCorrectReturnRequests()
        {
            //Arrange
            var returnRequests = new List<ReturnRequest>
            {
                new ReturnRequest
                    {
                        Id = 1,
                        AssignmentId = 1,
                        Assignment = new Assignment
                        {
                            Id = 1,
                            AssetId = 1,
                            Asset = new Asset
                            {
                                AssetName = "laptop 02",
                                AssetCode = "LP0002"
                            },
                            AssignedByUserId = 1,
                            AssignedToUserId = 2,
                            Note = "Testing1",
                            State = AssignmentState.Accepted
                        },
                        AcceptedByUserId = 2,
                        RequestedByUserId = 2,
                        RequestedByUser = new User
                        {
                            UserName = "bnt01"
                        },
                        ReturnedDate = DateTime.UtcNow,
                        State = RequestState.Completed
                    },
                    new ReturnRequest
                    {
                        Id = 2,
                        AssignmentId = 2,
                        Assignment = new Assignment
                        {
                            Id = 2,
                            AssetId = 1,
                            Asset = new Asset
                            {
                                AssetName = "laptop 01",
                                AssetCode = "LP0001"
                            },
                            AssignedByUserId = 1,
                            AssignedToUserId = 2,
                            Note = "Testing2",
                            State = AssignmentState.WaitingForAcceptance
                        },
                        RequestedByUserId = 3,
                        RequestedByUser = new User
                        {
                            UserName = "bnt02"
                        },
                        State = RequestState.WaitingForReturning
                    },
                    new ReturnRequest
                    {
                        Id = 3,
                        Assignment = new Assignment
                        {
                            Asset = new Asset
                            {
                                AssetName = "printer 01",
                                AssetCode = "PR0001"
                            },
                        },
                        RequestedByUserId = 3,
                        RequestedByUser = new User
                        {
                            UserName = "sql02"
                        },
                        State = RequestState.WaitingForReturning
                    }
            };
            _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).ReturnsAsync(new User { Id = 1, Type = UserType.Admin });
            _returnRequestRepositoryMock.Setup(x => x.GetAll()).Returns(returnRequests.AsQueryable());
            var service = new ReturnRequestService(
                _userRepositoryMock.Object,
                _returnRequestRepositoryMock.Object,
                _assignmentRepositoryMock.Object,
                _mapper
            );

            PaginationParameters parameters = new PaginationParameters
            {
                PageNumber = 1,
                PageSize = 30
            };

            //Act
            var returnRequestsPagedListResponse = await service.Search(
                parameters,
                1,
                "bnt");

            //Assert
            Assert.AreEqual(2, returnRequestsPagedListResponse.TotalCount);
        }

        [Test]
        public async Task Search_ByAssetName_ShouldReturnCorrectReturnRequests()
        {
            //Arrange
            var returnRequests = new List<ReturnRequest>
            {
                new ReturnRequest
                    {
                        Id = 1,
                        AssignmentId = 1,
                        Assignment = new Assignment
                        {
                            Id = 1,
                            AssetId = 1,
                            Asset = new Asset
                            {
                                AssetName = "laptop 02",
                                AssetCode = "LP0002"
                            },
                            AssignedByUserId = 1,
                            AssignedToUserId = 2,
                            Note = "Testing1",
                            State = AssignmentState.Accepted
                        },
                        AcceptedByUserId = 2,
                        RequestedByUserId = 2,
                        RequestedByUser = new User
                        {
                            UserName = "bnt01"
                        },
                        ReturnedDate = DateTime.UtcNow,
                        State = RequestState.Completed
                    },
                    new ReturnRequest
                    {
                        Id = 2,
                        AssignmentId = 2,
                        Assignment = new Assignment
                        {
                            Id = 2,
                            AssetId = 1,
                            Asset = new Asset
                            {
                                AssetName = "laptop 01",
                                AssetCode = "LP0001"
                            },
                            AssignedByUserId = 1,
                            AssignedToUserId = 2,
                            Note = "Testing2",
                            State = AssignmentState.WaitingForAcceptance
                        },
                        RequestedByUserId = 3,
                        RequestedByUser = new User
                        {
                            UserName = "bnt02"
                        },
                        State = RequestState.WaitingForReturning
                    },
                    new ReturnRequest
                    {
                        Id = 3,
                        Assignment = new Assignment
                        {
                            Asset = new Asset
                            {
                                AssetName = "printer 01",
                                AssetCode = "PR0001"
                            },
                            AssignedByUserId = 1
                        },
                        RequestedByUserId = 3,
                        RequestedByUser = new User
                        {
                            UserName = "sql02"
                        },
                        State = RequestState.WaitingForReturning
                    }
            };
            _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).ReturnsAsync(new User { Id = 1, Type = UserType.Admin });
            _returnRequestRepositoryMock.Setup(x => x.GetAll()).Returns(returnRequests.AsQueryable());
            var service = new ReturnRequestService(
                _userRepositoryMock.Object,
                _returnRequestRepositoryMock.Object,
                _assignmentRepositoryMock.Object,
                _mapper
            );

            PaginationParameters parameters = new PaginationParameters
            {
                PageNumber = 1,
                PageSize = 30
            };

            //Act
            var returnRequestsPagedListResponse = await service.Search(
                parameters,
                1,
                "printer");

            //Assert
            Assert.AreEqual(1, returnRequestsPagedListResponse.TotalCount);
        }

        [Test]
        public async Task Search_ByAssetCode_ShouldReturnCorrectReturnRequests()
        {
            //Arrange
            var returnRequests = new List<ReturnRequest>
            {
                new ReturnRequest
                    {
                        Id = 1,
                        AssignmentId = 1,
                        Assignment = new Assignment
                        {
                            Id = 1,
                            AssetId = 1,
                            Asset = new Asset
                            {
                                AssetName = "laptop 02",
                                AssetCode = "LP0002"
                            },
                            AssignedByUserId = 1,
                            AssignedToUserId = 2,
                            Note = "Testing1",
                            State = AssignmentState.Accepted
                        },
                        AcceptedByUserId = 2,
                        RequestedByUserId = 2,
                        RequestedByUser = new User
                        {
                            UserName = "bnt01"
                        },
                        ReturnedDate = DateTime.UtcNow,
                        State = RequestState.Completed
                    },
                    new ReturnRequest
                    {
                        Id = 2,
                        AssignmentId = 2,
                        Assignment = new Assignment
                        {
                            Id = 2,
                            AssetId = 1,
                            Asset = new Asset
                            {
                                AssetName = "laptop 01",
                                AssetCode = "LP0001"
                            },
                            AssignedByUserId = 1,
                            AssignedToUserId = 2,
                            Note = "Testing2",
                            State = AssignmentState.WaitingForAcceptance
                        },
                        RequestedByUserId = 3,
                        RequestedByUser = new User
                        {
                            UserName = "bnt02"
                        },
                        State = RequestState.WaitingForReturning
                    },
                    new ReturnRequest
                    {
                        Id = 3,
                        Assignment = new Assignment
                        {
                            Asset = new Asset
                            {
                                AssetName = "printer 01",
                                AssetCode = "PR0001"
                            },
                            AssignedByUserId = 1
                        },
                        RequestedByUserId = 3,
                        RequestedByUser = new User
                        {
                            UserName = "sql02"
                        },
                        State = RequestState.WaitingForReturning
                    }
            };
            _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).ReturnsAsync(new User { Id = 1, Type = UserType.Admin });
            _returnRequestRepositoryMock.Setup(x => x.GetAll()).Returns(returnRequests.AsQueryable());
            var service = new ReturnRequestService(
                _userRepositoryMock.Object,
                _returnRequestRepositoryMock.Object,
                _assignmentRepositoryMock.Object,
                _mapper
            );

            PaginationParameters parameters = new PaginationParameters
            {
                PageNumber = 1,
                PageSize = 30
            };

            //Act
            var returnRequestsPagedListResponse = await service.Search(
                parameters,
                1,
                "LP");

            //Assert
            Assert.AreEqual(2, returnRequestsPagedListResponse.TotalCount);
        }

        [Test]
        public void Filter_UserNotAdmin_ShouldThrowException()
        {
            //Arrange
            _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).ReturnsAsync(new User { Type = UserType.User });
            _returnRequestRepositoryMock.Setup(x => x.GetAll()).Returns(It.IsAny<IQueryable<ReturnRequest>>()).Verifiable();
            var service = new ReturnRequestService(
                _userRepositoryMock.Object,
                _returnRequestRepositoryMock.Object,
                _assignmentRepositoryMock.Object,
                _mapper
            );

            //Act
            var exception = Assert.ThrowsAsync<Exception>(
                async () =>
                {
                    await service.Filter(
                        It.IsAny<PaginationParameters>(),
                        It.IsAny<int>(),
                        It.IsAny<ReturnRequestFilterParameters>()
                    );
                }
            );

            //Assert
            Assert.AreEqual(Message.UnauthorizedUser, exception.Message);
            _returnRequestRepositoryMock.Verify(x => x.GetAll(), Times.Never());
        }

        [Test]
        public async Task Create_Valid_ShouldBeSuccessful()
        {
            //Arrange
            var inputModel = new CreateReturnRequestModel
            {
                AssignmentId = 1
            };
            var userId = 1;
            var user = Users.Where(u => u.Id == userId).Single();
            var assignment = Assignments.Where(a => a.Id == inputModel.AssignmentId).Single();
            var simulatedReturnedRequest = new ReturnRequest
            {
                RequestedByUser = user,
                Assignment = assignment,
                ReturnedDate = null,
                State = RequestState.WaitingForReturning
            };
            _userRepositoryMock.Setup(x => x.GetAll()).Returns(Users);
            _assignmentRepositoryMock.Setup(x => x.GetById(1)).ReturnsAsync(assignment);
            _returnRequestRepositoryMock.Setup(x => x.Create(It.IsAny<ReturnRequest>())).ReturnsAsync(simulatedReturnedRequest).Verifiable();

            var service = new ReturnRequestService(
                _userRepositoryMock.Object,
                _returnRequestRepositoryMock.Object,
                _assignmentRepositoryMock.Object,
                _mapper
            );

            //Act
            var result = await service.Create(inputModel, 1);

            //Assert
            Assert.AreEqual(simulatedReturnedRequest.RequestedByUser.UserName, result.RequestedByUser);
            Assert.AreEqual(simulatedReturnedRequest.ReturnedDate, result.ReturnedDate);
            _returnRequestRepositoryMock.Verify(x => x.Create(It.IsAny<ReturnRequest>()), Times.Once());
        }

        [Test]
        public void Create_NullInputModel_ShouldThrowException()
        {
            //Arrange
            _userRepositoryMock.Setup(x => x.GetAll()).Returns(It.IsAny<IQueryable<User>>());
            _assignmentRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).ReturnsAsync(It.IsAny<Assignment>());
            _returnRequestRepositoryMock.Setup(x => x.Create(It.IsAny<ReturnRequest>())).ReturnsAsync(It.IsAny<ReturnRequest>()).Verifiable();

            var service = new ReturnRequestService(
                _userRepositoryMock.Object,
                _returnRequestRepositoryMock.Object,
                _assignmentRepositoryMock.Object,
                _mapper
            );

            //Act
            var exception = Assert.ThrowsAsync<Exception>(
                async () =>
                {
                    await service.Create(null, It.IsAny<int>());
                }
            );

            //Assert
            Assert.AreEqual(Message.NullInputModel, exception.Message);
            _returnRequestRepositoryMock.Verify(x => x.Create(It.IsAny<ReturnRequest>()), Times.Never());
        }

        [Test]
        public void Create_UserNotExists_ShouldThrowException()
        {
            //Arrange
            var inputModel = new CreateReturnRequestModel
            {
                AssignmentId = 1
            };
            var users = new List<User> { };
            _userRepositoryMock.Setup(x => x.GetAll()).Returns(users.AsQueryable());
            _assignmentRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).ReturnsAsync(It.IsAny<Assignment>());
            _returnRequestRepositoryMock.Setup(x => x.Create(It.IsAny<ReturnRequest>())).ReturnsAsync(It.IsAny<ReturnRequest>()).Verifiable();

            var service = new ReturnRequestService(
                _userRepositoryMock.Object,
                _returnRequestRepositoryMock.Object,
                _assignmentRepositoryMock.Object,
                _mapper
            );

            //Act
            var exception = Assert.ThrowsAsync<Exception>(
                async () =>
                {
                    await service.Create(inputModel, It.IsAny<int>());
                }
            );

            //Assert
            Assert.AreEqual(Message.UserNotFound, exception.Message);
            _returnRequestRepositoryMock.Verify(x => x.Create(It.IsAny<ReturnRequest>()), Times.Never());
        }

        [Test]
        public void Create_DisabledUser_ShouldThrowException()
        {
            //Arrange
            var inputModel = new CreateReturnRequestModel
            {
                AssignmentId = 1
            };
            var users = new List<User>
            {
                new User
                {
                    Id = 1,
                    Status = UserStatus.Disabled
                }
            };
            _userRepositoryMock.Setup(x => x.GetAll()).Returns(users.AsQueryable());
            _assignmentRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).ReturnsAsync(It.IsAny<Assignment>());
            _returnRequestRepositoryMock.Setup(x => x.Create(It.IsAny<ReturnRequest>())).ReturnsAsync(It.IsAny<ReturnRequest>()).Verifiable();

            var service = new ReturnRequestService(
                _userRepositoryMock.Object,
                _returnRequestRepositoryMock.Object,
                _assignmentRepositoryMock.Object,
                _mapper
            );

            //Act
            var exception = Assert.ThrowsAsync<Exception>(
                async () =>
                {
                    await service.Create(inputModel, 1);
                }
            );

            //Assert
            Assert.AreEqual(Message.UnauthorizedUser, exception.Message);
            _returnRequestRepositoryMock.Verify(x => x.Create(It.IsAny<ReturnRequest>()), Times.Never());
        }

        [Test]
        public void Create_AssignmentNotFound_ShouldThrowException()
        {
            //Arrange
            var inputModel = new CreateReturnRequestModel
            {
                AssignmentId = 1
            };
            var users = new List<User>
            {
                new User
                {
                    Id = 1,
                    Status = UserStatus.Active
                }
            };
            _userRepositoryMock.Setup(x => x.GetAll()).Returns(users.AsQueryable());
            _assignmentRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).ReturnsAsync(null as Assignment);
            _returnRequestRepositoryMock.Setup(x => x.Create(It.IsAny<ReturnRequest>())).ReturnsAsync(It.IsAny<ReturnRequest>()).Verifiable();

            var service = new ReturnRequestService(
                _userRepositoryMock.Object,
                _returnRequestRepositoryMock.Object,
                _assignmentRepositoryMock.Object,
                _mapper
            );

            //Act
            var exception = Assert.ThrowsAsync<Exception>(
                async () =>
                {
                    await service.Create(inputModel, 1);
                }
            );

            //Assert
            Assert.AreEqual(Message.AssignmentNotFound, exception.Message);
            _returnRequestRepositoryMock.Verify(x => x.Create(It.IsAny<ReturnRequest>()), Times.Never());
        }

        [Test]
        public void Create_NormalUserTryToCreateReturnRequestForAssetsOfSomeoneElse_ShouldThrowException()
        {
            //Arrange
            var inputModel = new CreateReturnRequestModel
            {
                AssignmentId = 1
            };
            var users = new List<User>
            {
                new User
                {
                    Id = 1,
                    Type = UserType.User,
                    Status = UserStatus.Active
                }
            };
            _userRepositoryMock.Setup(x => x.GetAll()).Returns(users.AsQueryable());
            _assignmentRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).ReturnsAsync(new Assignment
            {
                AssignedToUserId = 2,
                State = AssignmentState.Accepted
            });
            _returnRequestRepositoryMock.Setup(x => x.Create(It.IsAny<ReturnRequest>())).ReturnsAsync(It.IsAny<ReturnRequest>()).Verifiable();

            var service = new ReturnRequestService(
                _userRepositoryMock.Object,
                _returnRequestRepositoryMock.Object,
                _assignmentRepositoryMock.Object,
                _mapper
            );

            //Act
            var exception = Assert.ThrowsAsync<Exception>(
                async () =>
                {
                    await service.Create(inputModel, 1);
                }
            );

            //Assert
            Assert.AreEqual(Message.TriedToCreateReturnRequestForSomeoneElseAssignment, exception.Message);
            _returnRequestRepositoryMock.Verify(x => x.Create(It.IsAny<ReturnRequest>()), Times.Never());
        }

[Test]
        public void Create_AssetNotYetAccepted_ShouldThrowException()
        {
            //Arrange
            var inputModel = new CreateReturnRequestModel
            {
                AssignmentId = 1
            };
            var users = new List<User>
            {
                new User
                {
                    Id = 1,
                    Type = UserType.User,
                    Status = UserStatus.Active
                }
            };
            _userRepositoryMock.Setup(x => x.GetAll()).Returns(users.AsQueryable());
            _assignmentRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).ReturnsAsync(new Assignment
            {
                AssignedToUserId = 1,
                State = AssignmentState.WaitingForAcceptance
            });
            _returnRequestRepositoryMock.Setup(x => x.Create(It.IsAny<ReturnRequest>())).ReturnsAsync(It.IsAny<ReturnRequest>()).Verifiable();

            var service = new ReturnRequestService(
                _userRepositoryMock.Object,
                _returnRequestRepositoryMock.Object,
                _assignmentRepositoryMock.Object,
                _mapper
            );

            //Act
            var exception = Assert.ThrowsAsync<Exception>(
                async () =>
                {
                    await service.Create(inputModel, 1);
                }
            );

            //Assert
            Assert.AreEqual(Message.AssignedAssetNotAccepted, exception.Message);
            _returnRequestRepositoryMock.Verify(x => x.Create(It.IsAny<ReturnRequest>()), Times.Never());
        }
        

        // [Test]
        // public void Create_UserAgeUnder18_ThrowsExceptionMessage()
        // {

        //     //Arrange
        //     CreateUserModel user = new CreateUserModel
        //     {
        //         DateOfBirth = new DateTime(2010, 12, 12)
        //     };

        //     _optionsMock.SetupGet(x => x.Value).Returns(Settings.Value);
        //     var userService = new UserService(
        //         _userRepositoryMock.Object,
        //         _assignmentRepositoryMock.Object,
        //         _mapper,
        //         _optionsMock.Object
        //     );

        //     //Act
        //     var result = Assert.ThrowsAsync<Exception>(async () => await userService.Create(user));

        //     //Assert
        //     Assert.AreEqual(Message.RestrictedAge, result.Message);

        // }

        // [Test]
        // public void Create_JoinedDateEarlierThanDob_ThrowsExceptionMessage()
        // {

        //     //Arrange
        //     CreateUserModel user = new CreateUserModel
        //     {
        //         DateOfBirth = new DateTime(2000, 12, 12),
        //         JoinedDate = new DateTime(2000, 01, 01)
        //     };

        //     _optionsMock.SetupGet(x => x.Value).Returns(Settings.Value);
        //     var userService = new UserService(
        //         _userRepositoryMock.Object,
        //         _assignmentRepositoryMock.Object,
        //         _mapper,
        //         _optionsMock.Object
        //     );

        //     //Act
        //     var result = Assert.ThrowsAsync<Exception>(async () => await userService.Create(user));

        //     //Assert
        //     Assert.AreEqual(Message.JoinedBeforeBirth, result.Message);

        // }

        // [Test]
        // public void Create_JoinedDateAtWeekend_ThrowsExceptionMessage()
        // {

        //     //Arrange
        //     CreateUserModel user = new CreateUserModel
        //     {
        //         JoinedDate = new DateTime(2010, 05, 16)
        //     };

        //     _optionsMock.SetupGet(x => x.Value).Returns(Settings.Value);
        //     var userService = new UserService(
        //         _userRepositoryMock.Object,
        //         _assignmentRepositoryMock.Object,
        //         _mapper,
        //         _optionsMock.Object
        //     );

        //     //Act
        //     var result = Assert.ThrowsAsync<Exception>(async () => await userService.Create(user));

        //     //Assert
        //     Assert.AreEqual(Message.WeekendJoinedDate, result.Message);

        // }

        // [Test]
        // public void Create_ValidUserInserted_ReturnsCreatedUser()
        // {

        //     //Arrange
        //     CreateUserModel user = new CreateUserModel
        //     {
        //         FirstName = "Thang",
        //         LastName = "Doan Viet",
        //         DateOfBirth = new DateTime(1995, 06, 03),
        //         Gender = Gender.Male,
        //         JoinedDate = new DateTime(2021, 05, 19),
        //         Type = UserType.Admin,
        //         Location = Location.HaNoi
        //     };

        //     User _user = _mapper.Map<User>(user);

        //     User createdUser = new User
        //     {
        //         FirstName = "Thang",
        //         LastName = "Doan Viet",
        //         DateOfBirth = new DateTime(1995, 06, 03),
        //         Gender = Gender.Male,
        //         JoinedDate = new DateTime(2021, 05, 19),
        //         Type = UserType.Admin,
        //         Location = Location.HaNoi,
        //         StaffCode = "SD0001",
        //         UserName = "thangdv",
        //         Password = "thangdv@03061995"
        //     };

        //     _userRepositoryMock.Setup(x => x.CountUsername("thangdv")).Returns(0);
        //     _userRepositoryMock.Setup(x => x.Create(_user)).ReturnsAsync(createdUser);
        //     _optionsMock.SetupGet(x => x.Value).Returns(Settings.Value);
        //     var userService = new UserService(
        //         _userRepositoryMock.Object,
        //         _assignmentRepositoryMock.Object,
        //         _mapper,
        //         _optionsMock.Object
        //     );

        //     //Act

        //     var result = userService.Create(user);

        //     //Assert
        //     Assert.AreEqual("Thang", createdUser.FirstName);
        //     Assert.AreEqual("Doan Viet", createdUser.LastName);
        //     Assert.AreEqual(new DateTime(1995, 06, 03), createdUser.DateOfBirth);

        // }

        [TearDown]
        public void TearDown()
        {

        }

    }
}