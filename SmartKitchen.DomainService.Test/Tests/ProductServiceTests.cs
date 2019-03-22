using AutoFixture;
using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using SmartKitchen.Domain.CreationModels;
using SmartKitchen.Domain.DisplayModels;
using SmartKitchen.Domain.Enitities;
using SmartKitchen.Domain.Enums;
using SmartKitchen.Domain.IRepositories;
using SmartKitchen.DomainService.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using SmartKitchen.Domain.IServices;

// ReSharper disable NUnit.MethodWithParametersAndTestAttribute

namespace SmartKitchen.DomainService.Test.Tests
{
    [TestFixture, Category("Storage service")]
    public class ProductServiceTests
    {
        [Test, CustomAutoData]
        public void GetAllProductDisplays_Map_ProductDisplay(IFixture fixture, [Frozen] IQueryable<Product> products)
        {
            var product = products.First();
            var productRepositoryMock = fixture.Freeze<Mock<IProductRepository>>();
            productRepositoryMock.Setup(x => x.GetAllProducts()).Returns(products);

            var sut = fixture.Create<ProductService>();
            var actual = sut.GetAllProductDisplays().First();

            Assert.NotNull(actual);
            Assert.That(actual, Has.Property(nameof(ProductDisplayModel.Id)).EqualTo(product.Id));
            Assert.That(actual, Has.Property(nameof(ProductDisplayModel.Name)).EqualTo(product.Name));
            Assert.That(actual, Has.Property(nameof(ProductDisplayModel.CategoryId)).EqualTo(product.CategoryId));
            Assert.That(actual, Has.Property(nameof(ProductDisplayModel.CategoryName)).EqualTo(product.Category.Name));
            Assert.That(actual, Has.Property(nameof(ProductDisplayModel.Usages)).EqualTo(product.Cells.Count));
        }

        [Test, CustomAutoData]
        public void AddProduct_validateproductNameNotUnique(IFixture fixture, NameCreationModel model, Product product)
        {
            var productRepMock = fixture.Freeze<Mock<IProductRepository>>();
            product.Name = model.Name;
            productRepMock.Setup(x => x.GetProductByName(It.IsAny<string>())).Returns(product);

            var sut = fixture.Create<CategoryService>();
            var actual = sut.AddCategory(model);

            var error = actual.Errors.First();
            Assert.That(error, Has.Property(nameof(error.ErrorEnum)).EqualTo(GeneralError.NameIsAlreadyTaken));
        }

        [Test, CustomAutoData]
        public void AddProduct_validateproductNameUnique(IFixture fixture, NameCreationModel model)
        {
            var productRepMock = fixture.Freeze<Mock<IProductRepository>>();
            productRepMock.Setup(x => x.GetProductByName(It.IsAny<string>())).Returns((Product)null);

            var sut = fixture.Create<ProductService>();
            var actual = sut.AddProduct(model);

            var result = actual.Successful();
            Assert.IsTrue(result);
        }

        [Test, CustomAutoData]
        public void UpdateProductList_verifyUpdatesCount(IFixture fixture, List<ProductDisplayModel> products)
        {
            var categoryRepMock = fixture.Freeze<Mock<ICategoryRepository>>();
            var productRepMock = fixture.Freeze<Mock<IProductRepository>>();
            productRepMock.Setup(x => x.ExistsAnotherWithEqualName(It.IsAny<string>(), It.IsAny<int>())).Returns(false);
            categoryRepMock.Setup(x => x.ExistsWithId(It.IsAny<int>())).Returns(true);
            var newList = products;
            int count = 0;
            Random rand = new Random();
            foreach (var i in newList)
            {
                productRepMock.Setup(x => x.GetProductById(i.Id)).Returns(new Product { CategoryId = i.CategoryId, Id = i.Id, Name = i.Name });
                int r = rand.Next(3);
                if (r == 1) i.CategoryId++;
                if (r == 2) i.Name += "a";
                if (r > 0) count++;
            }

            var sut = fixture.Create<ProductService>();
            var actual = sut.UpdateProductList(newList);

            Assert.AreEqual(count, actual);
        }


        [Test, CustomAutoData]
        public void GetOrAddAndGetProduct_ShouldGet(IFixture fixture, Product product)
        {
            var productRepMock = fixture.Freeze<Mock<IProductService>>();
            productRepMock.Setup(x => x.GetProductByName(It.IsAny<string>())).Returns(product);

            var sut = fixture.Create<ProductService>();
            var actual = sut.GetOrAddAndGetProduct(product.Name);

            productRepMock.Verify(x => x.AddProduct(new NameCreationModel(It.IsAny<string>())), Times.Never());
            Assert.NotNull(actual);
        }

        [Test, CustomAutoData]
        public void GetOrAddAndGetProduct_ShouldAdd(IFixture fixture, Person person, Storage storage, Product product, Cell cell)
        {
            var productRepMock = fixture.Freeze<Mock<IProductService>>();
            productRepMock.Setup(x => x.GetProductByName(It.IsAny<string>())).Returns((Product)null);

            var sut = fixture.Create<ProductService>();
            var actual = sut.GetOrAddAndGetProduct(product.Name);

            productRepMock.Verify(x => x.AddProduct(new NameCreationModel(It.IsAny<string>())), Times.Never());
            Assert.NotNull(actual);
        }
    }
}
