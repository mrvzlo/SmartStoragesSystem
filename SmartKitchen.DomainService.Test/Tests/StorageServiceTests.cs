using System.Collections.Generic;
using AutoFixture;
using AutoFixture.NUnit3;
using NUnit.Framework;
using SmartKitchen.Domain.DisplayModels;
using SmartKitchen.Domain.Enitities;
using SmartKitchen.DomainService.Services;
using System.Linq;
using Moq;
using SmartKitchen.Domain.CreationModels;
using SmartKitchen.Domain.Enums;
using SmartKitchen.Domain.IRepositories;

namespace SmartKitchen.DomainService.Test.Tests
{
    [TestFixture, Category("Storage service")]
    public class StorageServiceTests
    {
        [Test, CustomAutoData]
        public void GetAllCategoryDisplays_Map_CategoryDisplay(IFixture fixture, [Frozen] IQueryable<Category> categories)
        {
            var category = categories.First();
            var categoryRepositoryMock = fixture.Freeze<Mock<ICategoryRepository>>();
            categoryRepositoryMock.Setup(x => x.GetAllCategories()).Returns(categories);

            var sut = fixture.Create<CategoryService>();
            var actual = sut.GetAllCategoryDisplays().First();

            Assert.NotNull(actual);
            Assert.That(actual, Has.Property(nameof(CategoryDisplayModel.Id)).EqualTo(category.Id));
            Assert.That(actual, Has.Property(nameof(CategoryDisplayModel.Name)).EqualTo(category.Name));
            Assert.That(actual, Has.Property(nameof(CategoryDisplayModel.ProductsCount)).EqualTo(category.Products.Count));
            Assert.That(actual, Has.Property(nameof(CategoryDisplayModel.Primal)).EqualTo(category.Name == ""));
        }

        [Test, CustomAutoData]
        public void AddCategory_validateCategoryNameNotUnique(IFixture fixture, NameCreationModel model, Category category)
        {
            var categoryRepMock = fixture.Freeze<Mock<ICategoryRepository>>();
            category.Name = model.Name;
            categoryRepMock.Setup(x => x.GetCategoryByName(It.IsAny<string>())).Returns(category);
            
            var sut = fixture.Create<CategoryService>();
            var actual = sut.AddCategory(model);

            var error = actual.Errors.First();
            Assert.That(error, Has.Property(nameof(error.ErrorEnum)).EqualTo(GeneralError.NameIsAlreadyTaken));
        }

        [Test, CustomAutoData]
        public void AddCategory_validateCategoryNameUnique(IFixture fixture, NameCreationModel model)
        {
            var categoryRepMock = fixture.Freeze<Mock<ICategoryRepository>>();
            categoryRepMock.Setup(x => x.GetCategoryByName(It.IsAny<string>())).Returns((Category)null);

            var sut = fixture.Create<CategoryService>();
            var actual = sut.AddCategory(model);

            var result = actual.Successful();
            Assert.IsTrue(result);
        }
    }
}
