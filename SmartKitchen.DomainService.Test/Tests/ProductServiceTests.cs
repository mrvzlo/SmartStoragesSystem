using System;
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
                productRepMock.Setup(x => x.GetProductById(i.Id)).Returns(new Product{CategoryId = i.CategoryId, Id = i.Id, Name = i.Name});
                if (rand.Next(2) == 0)
                {
                    i.CategoryId++;
                    count++;
                }

                if (rand.Next(2) == 0)
                {
                    i.Name += "a";
                    count++;
                }
            }

            var sut = fixture.Create<ProductService>();
            var actual = sut.UpdateProductList(newList);

            Assert.AreEqual(count,actual);
        }
    }
}
