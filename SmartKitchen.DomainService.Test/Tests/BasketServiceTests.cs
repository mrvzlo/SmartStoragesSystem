using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using SmartKitchen.Domain.CreationModels;
using SmartKitchen.Domain.Enitities;
using SmartKitchen.Domain.Enums;
using SmartKitchen.Domain.IRepositories;
using SmartKitchen.DomainService.Services;
using System.Collections.Generic;
using System.Linq;
using SmartKitchen.Domain.DisplayModels;

namespace SmartKitchen.DomainService.Test.Tests
{
    [TestFixture, Category("Basket service")]
    public class BasketServiceTests
    {
        [Test, CustomAutoData]
        public void AddBasket_validateBasketNameNotUnique(IFixture fixture, NameCreationModel model, string email)
        {
            var personRepMock = fixture.Freeze<Mock<IPersonRepository>>();
            var basketRepMock = fixture.Freeze<Mock<IBasketRepository>>();

            personRepMock.Setup(x => x.GetPersonByEmail(It.IsAny<string>()))
                .Returns(new Person { Baskets = new List<Basket> { new Basket { Name = model.Name } } });
            basketRepMock.Setup(x => x.AddOrUpdateBasket(It.IsAny<Basket>()));
            
            var sut = fixture.Create<BasketService>();
            var actual = sut.AddBasket(model, email);

            var error = actual.Errors.First();
            Assert.That(error, Has.Property(nameof(error.ErrorEnum)).EqualTo(GeneralError.NameIsAlreadyTaken));
        }

        [Test, CustomAutoData]
        public void AddBasket_validateBasketNameUnique(IFixture fixture, NameCreationModel model, string email)
        {
            var personRepMock = fixture.Freeze<Mock<IPersonRepository>>();
            var basketRepMock = fixture.Freeze<Mock<IBasketRepository>>();

            personRepMock.Setup(x => x.GetPersonByEmail(It.IsAny<string>()))
                .Returns(new Person { Baskets = new List<Basket> { new Basket { Name = model.Name } } });
            basketRepMock.Setup(x => x.AddOrUpdateBasket(It.IsAny<Basket>()));

            model.Name = model.Name + "a";

            var sut = fixture.Create<BasketService>();
            var actual = sut.AddBasket(model, email);

            var result = actual.Successful();
            Assert.IsTrue(result);
        }

        [Test, CustomAutoData]
        public void GetBasketById_Map_BasketDisplayModel(IFixture fixture, [Frozen] IQueryable<Basket> baskets)
        {
            var basket = baskets.First();
            var basketRepMock = fixture.Freeze<Mock<IBasketRepository>>();
            basketRepMock.Setup(x => x.GetBaskets()).Returns(baskets);

            var sut = fixture.Create<BasketService>();
            var actual = sut.GetBasketById(basket.Id, basket.Person.Email);

            Assert.NotNull(actual);
            Assert.That(actual, Has.Property(nameof(BasketDisplayModel.Id)).EqualTo(basket.Id));
            Assert.That(actual, Has.Property(nameof(BasketDisplayModel.Name)).EqualTo(basket.Name));
            Assert.That(actual, Has.Property(nameof(BasketDisplayModel.Closed)).EqualTo(basket.Closed));
            Assert.That(actual, Has.Property(nameof(BasketDisplayModel.Products)).EqualTo(basket.BasketProducts.Count));
            Assert.That(actual, Has.Property(nameof(BasketDisplayModel.CreationDate)).EqualTo(basket.CreationDate));
            Assert.That(actual, Has.Property(nameof(BasketDisplayModel.BoughtProducts)).EqualTo(basket.BasketProducts.Count(x => x.Bought)));
        }

        [Test, CustomAutoData]
        public void GetBasketById_Map_BasketWithProductsDisplayModel(IFixture fixture, [Frozen] IQueryable<Basket> baskets)
        {
            var basket = baskets.First();
            var basketRepMock = fixture.Freeze<Mock<IBasketRepository>>();
            basketRepMock.Setup(x => x.GetBaskets()).Returns(baskets);

            var sut = fixture.Create<BasketService>();
            var actual = sut.GetBasketWithProductsById(basket.Id, basket.Person.Email);

            Assert.NotNull(actual);
            Assert.That(actual, Has.Property(nameof(BasketWithProductsDisplayModel.Id)).EqualTo(basket.Id));
            Assert.That(actual, Has.Property(nameof(BasketWithProductsDisplayModel.Name)).EqualTo(basket.Name));
            Assert.That(actual, Has.Property(nameof(BasketWithProductsDisplayModel.Closed)).EqualTo(basket.Closed));
            Assert.That(actual, Has.Property(nameof(BasketWithProductsDisplayModel.BasketProducts)).EqualTo(basket.BasketProducts.ToList()));
            Assert.That(actual, Has.Property(nameof(BasketWithProductsDisplayModel.CreationDate)).EqualTo(basket.CreationDate));
        }
    }
}