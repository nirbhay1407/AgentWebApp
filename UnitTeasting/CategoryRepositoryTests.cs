using Ioc.Core.DbModel.Models;
using Ioc.Core.EnumClass;
using Ioc.Data;
using Ioc.Data.Caches;
using Ioc.Data.Data;
using Ioc.Data.UnitOfWorkRepo;
using Ioc.Service.Services;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;

namespace UnitTeasting
{
    public class CategoryRepositoryTests
    {
        private Mock<IUnitOfWork> _mockUnitOfWork;
        private Mock<IGenericRepository<Category>> _mockRepository;
        private CategoryRepository _categoryRepository;
        private Mock<ICacheService> _cacheService;

        [SetUp]
        public void Setup()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockRepository = new Mock<IGenericRepository<Category>>();
            _cacheService = new Mock<ICacheService>();
            
            _mockUnitOfWork
                .Setup(uow => uow.GetRepository<Category>())
                .Returns(_mockRepository.Object);

            var options = new DbContextOptionsBuilder<IocDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb_" + Guid.NewGuid().ToString())
                .Options;

            var context = new IocDbContext(options);

            // Setup cache service function
            Func<CacheTech, ICacheService> cacheService = (tech) => _cacheService.Object;
            _categoryRepository = new CategoryRepository(context, cacheService);
        }

        [Test]
        public async Task GetCategories_ShouldReturnAllCategories()
        {
            // Arrange
            var categories = new List<Category>
            {
                new Category { ID = Guid.NewGuid(), Name = "Category 1" },
                new Category { ID = Guid.NewGuid(), Name = "Category 2" }
            };

            _mockRepository
                .Setup(r => r.GetAll())
                .Returns(categories.AsQueryable());

            // Act
            var result = _categoryRepository.GetAll();

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count(), Is.EqualTo(2));
        }

        [Test]
        public async Task GetCategoryById_ShouldReturnCorrectCategory()
        {
            // Arrange
            var categoryId = Guid.NewGuid();
            var category = new Category { ID = categoryId, Name = "Test Category" };

            _mockRepository
                .Setup(r => r.GetById(categoryId))
                .ReturnsAsync(category);

            // Act
            var result = await _categoryRepository.GetById(categoryId);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.ID, Is.EqualTo(categoryId));
            Assert.That(result.Name, Is.EqualTo("Test Category"));
        }

        [Test]
        public async Task Create_ShouldCallRepositoryCreate()
        {
            // Arrange
            var category = new Category { ID = Guid.NewGuid(), Name = "New Category" };

            // Act
            await _categoryRepository.Create(category);

            // Assert
            _mockRepository.Verify(r => r.Create(category), Times.Once);
        }

        [Test]
        public async Task Update_ShouldCallRepositoryUpdate()
        {
            // Arrange
            var categoryId = Guid.NewGuid();
            var category = new Category { ID = categoryId, Name = "Updated Category" };

            // Act
            await _categoryRepository.Update(categoryId, category);

            // Assert
            _mockRepository.Verify(r => r.Update(categoryId, category), Times.Once);
        }

        [Test]
        public async Task Delete_ShouldCallRepositoryDelete()
        {
            // Arrange
            var categoryId = Guid.NewGuid();

            // Act
            await _categoryRepository.Delete(categoryId);

            // Assert
            _mockRepository.Verify(r => r.Delete(categoryId), Times.Once);
        }
    }
}