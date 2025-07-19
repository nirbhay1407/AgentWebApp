using Ioc.Core;
using Ioc.Core.DbModel.Models;
using Ioc.Core.EnumClass;
using Ioc.Data.Caches;
using Ioc.Data.Data;
using Ioc.Service.Interfaces.Validation;
using Ioc.Service.Services.Validation;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using Ioc.Core.DbModel.Validation;

namespace UnitTeasting
{
    public class ValidationRuleRepositoryTests
    {
        private IocDbContext _context;
        private Mock<ICacheService> _cacheService;
        private IValidationRuleRepository _repository;

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
            _cacheService.Setup(x => x.TryGet(It.IsAny<string>(), out It.Ref<IReadOnlyList<ValidationRule>>.IsAny))
                .Returns(false);
            
            // Setup cache service function
            Func<CacheTech, ICacheService> cacheService = (tech) => _cacheService.Object;
            _repository = new ValidationRuleRepository(_context, cacheService);

            // Setup test data
            var validationRules = new List<ValidationRule>
            {
                new ValidationRule 
                { 
                    ID = Guid.NewGuid(), 
                    ModelName = "TestModel",
                    PropertyName = "TestProperty",
                    RuleType = "Error",
                    Rule = "^test$",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                },
                new ValidationRule 
                { 
                    ID = Guid.NewGuid(), 
                    ModelName = "TestModel",
                    PropertyName = "AnotherProperty",
                    RuleType = "Warning",
                    Rule = "[A-Za-z]+",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                }
            };

            _context.ValidationRule.AddRange(validationRules);
            _context.SaveChanges();
        }

        [Test]
        public async Task GetValidationRulesAsync_ShouldReturnAllRulesForModel()
        {
            // Act
            var rules = await _repository.GetValidationRulesAsync("TestModel");

            // Assert
            Assert.That(rules, Is.Not.Null);
            Assert.That(rules.Count, Is.EqualTo(2));
            Assert.That(rules.Any(r => r.PropertyName == "TestProperty"), Is.True);
            Assert.That(rules.Any(r => r.PropertyName == "AnotherProperty"), Is.True);
        }

        [Test]
        public async Task GetValidationRulesAsync_ShouldReturnEmptyForNonExistentModel()
        {
            // Act
            var rules = await _repository.GetValidationRulesAsync("NonExistentModel");

            // Assert
            Assert.That(rules, Is.Empty);
        }

        [Test]
        public async Task GetValidationRulesAsync_ShouldReturnCorrectRuleTypes()
        {
            // Act
            var rules = await _repository.GetValidationRulesAsync("TestModel");

            // Assert
            Assert.That(rules.Any(r => r.RuleType == "Error"), Is.True);
            Assert.That(rules.Any(r => r.RuleType == "Warning"), Is.True);
        }

        [Test]
        public async Task GetValidationRulesAsync_ShouldReturnValidRegexPatterns()
        {
            // Act
            var rules = await _repository.GetValidationRulesAsync("TestModel");

            // Assert
            Assert.That(rules.All(r => !string.IsNullOrEmpty(r.Rule)), Is.True);
            var errorRule = rules.First(r => r.RuleType == "Error");
            Assert.That(errorRule.Rule, Is.EqualTo("^test$"));
        }

        [Test]
        public async Task GetValidationRulesAsync_ShouldOnlyReturnActiveRules()
        {
            // Arrange
            var inactiveRule = new ValidationRule 
            { 
                ID = Guid.NewGuid(), 
                ModelName = "TestModel",
                PropertyName = "InactiveProperty",
                RuleType = "Error",
                Rule = "test",
                IsActive = false,
                CreatedAt = DateTime.UtcNow
            };
            _context.ValidationRule.Add(inactiveRule);
            await _context.SaveChangesAsync();

            // Act
            var rules = await _repository.GetValidationRulesAsync("TestModel");

            // Assert
            Assert.That(rules.Any(r => r.PropertyName == "InactiveProperty"), Is.False);
        }

        [TearDown]
        public void Cleanup()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}