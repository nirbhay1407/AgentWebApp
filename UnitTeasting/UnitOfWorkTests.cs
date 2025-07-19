using Ioc.Core.DbModel.Models;
using Ioc.Core.EnumClass;
using Ioc.Data.Caches;
using Ioc.Data.Data;
using Ioc.Data.UnitOfWorkRepo;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;

namespace UnitTeasting
{
    public class UnitOfWorkTests
    {
        private IocDbContext _context;
        private Mock<ICacheService> _cacheService;
        private UnitOfWork _unitOfWork;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<IocDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;

            _context = new IocDbContext(options);
            _cacheService = new Mock<ICacheService>();
            
            // Setup cache service
            Func<CacheTech, ICacheService> cacheService = (tech) => _cacheService.Object;
            _unitOfWork = new UnitOfWork(_context, cacheService);
        }

        [Test]
        public void GetRepository_ShouldReturnSameInstanceForSameType()
        {
            // Act
            var repo1 = _unitOfWork.GetRepository<Category>();
            var repo2 = _unitOfWork.GetRepository<Category>();

            // Assert
            Assert.That(repo1, Is.Not.Null);
            Assert.That(repo2, Is.Not.Null);
            Assert.That(repo1, Is.SameAs(repo2), "Should return same instance for same type");
        }

        [Test]
        public void GetRepository_ShouldReturnDifferentInstancesForDifferentTypes()
        {
            // Act
            var categoryRepo = _unitOfWork.GetRepository<Category>();
            var subCategoryRepo = _unitOfWork.GetRepository<SubCategory>();

            // Assert
            Assert.That(categoryRepo, Is.Not.Null);
            Assert.That(subCategoryRepo, Is.Not.Null);
            Assert.That(categoryRepo, Is.Not.SameAs(subCategoryRepo), "Should return different instances for different types");
        }

        [Test]
        public async Task Complete_ShouldSaveChangesToDatabase()
        {
            // Arrange
            var categoryRepo = _unitOfWork.GetRepository<Category>();
            var category = new Category { ID = Guid.NewGuid(), Name = "Test Category" };
            await categoryRepo.Create(category);

            // Act
            var result = _unitOfWork.Complete();

            // Assert
            Assert.That(result, Is.GreaterThan(0), "Should return number of affected records");
            var savedCategory = await _context.Category.FindAsync(category.ID);
            Assert.That(savedCategory, Is.Not.Null);
            Assert.That(savedCategory.Name, Is.EqualTo("Test Category"));
        }

        [TearDown]
        public void Cleanup()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
            _unitOfWork.Dispose();
        }
    }
}