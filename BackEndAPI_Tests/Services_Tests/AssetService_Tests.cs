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
    public class AssetService_Tests
    {

        private static IQueryable<Asset> assets
        {
            get
            {
                return new List<Asset>
                {
                    new Asset
                    {
                        Id=1,
                        AssetName = "Asus 2021",
                        CategoryId = 1,
                        InstalledDate = DateTime.Now,
                        Location = 0,
                        Specification = "None",
                        State = 0
                    },
                    new Asset
                    {
                        Id=2,
                        AssetName = "Asus 2022",
                        AssetCode = "LA0000001",
                        CategoryId = 1,
                        InstalledDate = DateTime.Now,
                        Location = 0,
                        Specification = "None",
                        State = 0
                    },
                    new Asset
                    {
                        Id=3,
                        AssetName = "Asus 2023",
                        AssetCode = "LA0000002",
                        CategoryId = 1,
                        InstalledDate = DateTime.Now,
                        Location = 0,
                        Specification = "None",
                        State = 0
                    }
                }
                .AsQueryable();
            }
        }

        private static IMapper _mapper;

        private Mock<IAsyncAssetRepository> _assetRepositoryMock;
        private Mock<IAsyncUserRepository> _userRepositoryMock;
        private Mock<IAsyncAssetCategoryRepository> _categoryRepositoryMock;

        public AssetService_Tests()
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
            _assetRepositoryMock = new Mock<IAsyncAssetRepository>(behavior: MockBehavior.Strict);
            _userRepositoryMock = new Mock<IAsyncUserRepository>(behavior: MockBehavior.Strict);
            _categoryRepositoryMock = new Mock<IAsyncAssetCategoryRepository>(behavior: MockBehavior.Strict);
        }

        [Test]
        public void Create_NullAssetInserted_ThrowsExceptionMessage()
        {

            //Arrange
            CreateAssetModel assetModel = null;

            var assetService = new AssetService(
                _assetRepositoryMock.Object,
                _userRepositoryMock.Object,
                _categoryRepositoryMock.Object,
                _mapper
            );

            //Act
            var result = Assert.ThrowsAsync<ArgumentNullException>(async () => await assetService.Create(assetModel));

            //Assert
            Assert.AreEqual(Message.NullAsset, result.ParamName);

        }

        [Test]
        public void Create_NegativeAssetNumberInserted_ThrowsExceptionMessage()
        {

            //Arrange
            CreateAssetModel assetModel = new CreateAssetModel
            {
                AssetName = "Asus 2029",
                CategoryId = 1,
                InstalledDate = DateTime.Now,
                Location = 0,
                Specification = "None",
                State = 0
            };

            var assetService = new AssetService(
                _assetRepositoryMock.Object,
                _userRepositoryMock.Object,
                _categoryRepositoryMock.Object,
                _mapper
            );

            _assetRepositoryMock.Setup(x => x.CountingAssetNumber(It.IsAny<int>())).Returns(-1);

            //Act
            var result = Assert.ThrowsAsync<Exception>(async () => await assetService.Create(assetModel));

            //Assert
            Assert.AreEqual(Message.AssetNumberError, result.Message);

        }

        [Test]
        public void Create_InvalidAssetCategoryIdInserted_ThrowsExceptionMessage()
        {

            //Arrange
            CreateAssetModel assetModel = new CreateAssetModel
            {
                AssetName = "Asus 2029",
                CategoryId = 1,
                InstalledDate = DateTime.Now,
                Location = 0,
                Specification = "None",
                State = 0
            };

            var assetService = new AssetService(
                _assetRepositoryMock.Object,
                _userRepositoryMock.Object,
                _categoryRepositoryMock.Object,
                _mapper
            );

            AssetCategory invalidCategory = null;
            _assetRepositoryMock.Setup(x => x.CountingAssetNumber(It.IsAny<int>())).Returns(0);
            _categoryRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).ReturnsAsync(invalidCategory);

            //Act
            var result = Assert.ThrowsAsync<Exception>(async () => await assetService.Create(assetModel));

            //Assert
            Assert.AreEqual(Message.InvalidId, result.Message);

        }

        [Test]
        public async Task Create_ValidAssetInserted_ReturnsCreatedAsset()
        {

            //Arrange
            CreateAssetModel assetModel = new CreateAssetModel
            {
                AssetName = "Asus 2029",
                CategoryId = 1,
                InstalledDate = DateTime.Now,
                Location = 0,
                Specification = "None",
                State = 0
            };

            var category = new AssetCategory
            {
                Id = 1,
                CategoryCode = "LA",
                CategoryName = "Laptop"
            };

            var assetService = new AssetService(
                _assetRepositoryMock.Object,
                _userRepositoryMock.Object,
                _categoryRepositoryMock.Object,
                _mapper
            );

            Asset asset = _mapper.Map<Asset>(assetModel);
            
            Asset createdAsset = new Asset
            {
                AssetName = "Asus 2029",
                AssetCode = "LA000001",
                CategoryId = 1,
                InstalledDate = DateTime.Now,
                Location = 0,
                Specification = "None",
                State = 0
            };

            _assetRepositoryMock.Setup(x => x.CountingAssetNumber(It.IsAny<int>())).Returns(0);
            _categoryRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).ReturnsAsync(category);
            _assetRepositoryMock.Setup(x => x.Create(It.IsAny<Asset>())).ReturnsAsync(createdAsset);

            //Act
            var result = await assetService.Create(assetModel);

            //Assert
            Assert.AreEqual(result.AssetName, createdAsset.AssetName);
            Assert.AreEqual(result.AssetCode, createdAsset.AssetCode);
            Assert.AreEqual(result.CategoryId, createdAsset.CategoryId);
            Assert.AreEqual(result.InstalledDate, createdAsset.InstalledDate);
            Assert.AreEqual(result.Location, createdAsset.Location);
            Assert.AreEqual(result.Specification, createdAsset.Specification);
            Assert.AreEqual(result.State, createdAsset.State);

        }

        [TearDown]
        public void TearDown()
        {

        }

    }
}