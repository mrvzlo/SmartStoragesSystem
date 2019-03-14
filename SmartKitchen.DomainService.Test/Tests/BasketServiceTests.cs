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
        public void AddBasket_validateBasketNameNotUnique(IFixture fixture, NameCreationModel name, Person person)
        {
            var basketRepMock = fixture.Freeze<Mock<IBasketRepository>>();
            var personRepMock = fixture.Freeze<Mock<IPersonRepository>>();
            person.Baskets = new List<Basket>{new Basket{Name = name.Name}};
            basketRepMock.Setup(x => x.AddOrUpdateBasket(It.IsAny<Basket>()));
            personRepMock.Setup(x => x.GetPersonByEmail(person.Email)).Returns(person);
            
            var sut = fixture.Create<BasketService>();
            var actual = sut.AddBasket(name, person.Email);

            var error = actual.Errors.First();
            Assert.That(error, Has.Property(nameof(error.ErrorEnum)).EqualTo(GeneralError.NameIsAlreadyTaken));
        }

        [Test, CustomAutoData]
        public void AddBasket_validateBasketNameUnique(IFixture fixture, NameCreationModel name, Person person)
        {
            var basketRepMock = fixture.Freeze<Mock<IBasketRepository>>();
            var personRepMock = fixture.Freeze<Mock<IPersonRepository>>();
            person.Baskets = new List<Basket> { new Basket { Name = name.Name } };
            basketRepMock.Setup(x => x.AddOrUpdateBasket(It.IsAny<Basket>()));
            personRepMock.Setup(x => x.GetPersonByEmail(person.Email)).Returns(person);

            name.Name = name.Name + "a";

            var sut = fixture.Create<BasketService>();
            var actual = sut.AddBasket(name, person.Email);

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