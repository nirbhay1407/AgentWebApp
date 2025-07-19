using Ioc.Core;
using Ioc.Core.DbModel.Models;
using Ioc.Core.EnumClass;
using Ioc.Data;
using Ioc.Data.Caches;
using Ioc.Data.Data;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;

namespace UnitTeasting
{
    public class GenericRepositoryTests
    {
        private IocDbContext _context;
        private Mock<ICacheService> _cacheService;
        private GenericRepository<Category> _repository;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<IocDbContext>()
                .UseInMemoryDatabase(databaseName: $"TestDb_{Guid.NewGuid()}")
                .Options;

            _context = new IocDbContext(options);
            _cacheService = new Mock<ICacheService>();
            
            // Setup cache service expectations
            _cacheService.Setup(x => x.Set(It.IsAny<string>(), It.IsAny<object>()))
                .Verifiable();
            _cacheService.Setup(x => x.Remove(It.IsAny<string>()))
                .Verifiable();
            _cacheService.Setup(x => x.TryGet(It.IsAny<string>(), out It.Ref<IReadOnlyList<Category>>.IsAny))
                .Returns(false);
            
            // Setup cache service factory
            Func<CacheTech, ICacheService> cacheService = (tech) => _cacheService.Object;
            _repository = new GenericRepository<Category>(_context, cacheService);
        }

        [Test]
        public async Task Create_ShouldAddEntityToDatabase()
        {
            // Arrange
            var category = new Category 
            { 
                ID = Guid.NewGuid(), 
                Name = "Test Category", 
                Description = "Test Description",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            // Act
            await _repository.Create(category);

            // Assert
            var savedCategory = await _context.Category.FindAsync(category.ID);
            Assert.That(savedCategory, Is.Not.Null);
            Assert.That(savedCategory.Name, Is.EqualTo("Test Category"));
            Assert.That(savedCategory.IsActive, Is.True);
            _cacheService.Verify(x => x.Remove(It.IsAny<string>()), Times.Once);
        }

        [Test]
        public async Task GetById_ShouldReturnCorrectEntity()
        {
            // Arrange
            var categoryId = Guid.NewGuid();
            var category = new Category 
            { 
                ID = categoryId, 
                Name = "Test Category", 
                Description = "Test Description",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };
            await _context.Category.AddAsync(category);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetById(categoryId);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.ID, Is.EqualTo(categoryId));
            Assert.That(result.Name, Is.EqualTo("Test Category"));
        }

        [Test]
        public async Task GetAll_ShouldReturnAllEntities()
        {
            // Arrange
            var now = DateTime.UtcNow;
            await _context.Category.AddRangeAsync(
                new Category { ID = Guid.NewGuid(), Name = "Category 1", IsActive = true, CreatedAt = now },
                new Category { ID = Guid.NewGuid(), Name = "Category 2", IsActive = true, CreatedAt = now },
                new Category { ID = Guid.NewGuid(), Name = "Category 3", IsActive = true, CreatedAt = now }
            );
            await _context.SaveChangesAsync();

            // Act
            var results = _repository.GetAll().ToList();

            // Assert
            Assert.That(results.Count, Is.EqualTo(3));
            Assert.That(results.All(c => c.IsActive), Is.True);
        }

        [Test]
        public async Task Update_ShouldModifyExistingEntity()
        {
            // Arrange
            var categoryId = Guid.NewGuid();
            var category = new Category 
            { 
                ID = categoryId, 
                Name = "Original Name",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };
            await _context.Category.AddAsync(category);
            await _context.SaveChangesAsync();

            // Act
            category.Name = "Updated Name";
            category.ModifiedDate = DateTime.UtcNow;
            await _repository.Update(categoryId, category);

            // Assert
            var updatedCategory = await _context.Category.FindAsync(categoryId);
            Assert.That(updatedCategory.Name, Is.EqualTo("Updated Name"));
            Assert.That(updatedCategory.ModifiedDate, Is.Not.Null);
            _cacheService.Verify(x => x.Remove(It.IsAny<string>()), Times.Once);
        }

        [Test]
        public async Task Delete_ShouldRemoveEntity()
        {
            // Arrange
            var categoryId = Guid.NewGuid();
            var category = new Category 
            { 
                ID = categoryId, 
                Name = "To Be Deleted",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };
            await _context.Category.AddAsync(category);
            await _context.SaveChangesAsync();

            // Act
            await _repository.Delete(categoryId);

            // Assert
            var deletedCategory = await _context.Category.FindAsync(categoryId);
            Assert.That(deletedCategory, Is.Null);
            _cacheService.Verify(x => x.Remove(It.IsAny<string>()), Times.Once);
        }

        [Test]
        public async Task GetAllWithInclude_ShouldReturnEntitiesWithRelatedData()
        {
            // Arrange
            var now = DateTime.UtcNow;
            await _context.Category.AddRangeAsync(
                new Category { ID = Guid.NewGuid(), Name = "Category 1", IsActive = true, CreatedAt = now },
                new Category { ID = Guid.NewGuid(), Name = "Category 2", IsActive = true, CreatedAt = now }
            );
            await _context.SaveChangesAsync();

            // Act
            var results = _repository.GetAllWithInclude("SubCategories").ToList();

            // Assert
            Assert.That(results.Count, Is.EqualTo(2));
        }

        [Test]
        public async Task GetAllAsync_ShouldUseCacheWhenAvailable()
        {
            // Arrange
            var cachedList = new List<Category>
            {
                new Category { ID = Guid.NewGuid(), Name = "Cached Category" }
            };

            _cacheService.Setup(x => x.TryGet(It.IsAny<string>(), out It.Ref<IReadOnlyList<Category>>.IsAny))
                .Callback(new TryGetCallback((string key, out IReadOnlyList<Category> value) =>
                {
                    value = cachedList;
                }))
                .Returns(true);

            // Act
            var results = await _repository.GetAllAsync();

            // Assert
            Assert.That(results.Count, Is.EqualTo(1));
            Assert.That(results.First().Name, Is.EqualTo("Cached Category"));
            _cacheService.Verify(x => x.TryGet(It.IsAny<string>(), out It.Ref<IReadOnlyList<Category>>.IsAny), Times.Once);
        }

        [TearDown]
        public void Cleanup()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        private delegate void TryGetCallback(string key, out IReadOnlyList<Category> value);
    }
}