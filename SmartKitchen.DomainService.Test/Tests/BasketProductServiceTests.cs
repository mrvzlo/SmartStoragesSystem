using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoFixture;
using Moq;
using NUnit.Framework;
using SmartKitchen.Domain.CreationModels;
using SmartKitchen.Domain.Enitities;
using SmartKitchen.Domain.Enums;
using SmartKitchen.Domain.IRepositories;
using SmartKitchen.DomainService.Services;

namespace SmartKitchen.DomainService.Test.Tests
{
    [TestFixture, Category("Basket service")]
    public class BasketProductServiceTests
    {
        [Test, CustomAutoData]
        public void AddBasketProduct_validateNullStorage(IFixture fixture, BasketProductCreationModel model, Person person, Basket basket)
        {
            var personRepMock = fixture.Freeze<Mock<IPersonRepository>>();
            var storageRepMock = fixture.Freeze<Mock<IStorageRepository>>();
            var basketRepMock = fixture.Freeze<Mock<IBasketRepository>>();

            personRepMock.Setup(x => x.GetPersonByEmail(It.IsAny<string>())).Returns(person);
            basketRepMock.Setup(x => x.GetBasketById(It.IsAny<int>())).Returns(basket);
            storageRepMock.Setup(x => x.GetStorageById(It.IsAny<int>())).Returns((Storage)null);

            var sut = fixture.Create<BasketProductService>();
            var actual = sut.AddBasketProduct(model, person.Email);

            var error = actual.Errors.First();
            Assert.That(error, Has.Property(nameof(error.ErrorEnum)).EqualTo(GeneralError.ItemNotFound));
        }


        [Test, CustomAutoData]
        public void AddBasketProduct_validateNullBasket(IFixture fixture, BasketProductCreationModel model, Person person, Storage storage)
        {
            var personRepMock = fixture.Freeze<Mock<IPersonRepository>>();
            var storageRepMock = fixture.Freeze<Mock<IStorageRepository>>();
            var basketRepMock = fixture.Freeze<Mock<IBasketRepository>>();

            personRepMock.Setup(x => x.GetPersonByEmail(It.IsAny<string>())).Returns(person);
            basketRepMock.Setup(x => x.GetBasketById(It.IsAny<int>())).Returns((Basket)null);
            storageRepMock.Setup(x => x.GetStorageById(It.IsAny<int>())).Returns(storage);

            var sut = fixture.Create<BasketProductService>();
            var actual = sut.AddBasketProduct(model, person.Email);

            var error = actual.Errors.First();
            Assert.That(error, Has.Property(nameof(error.ErrorEnum)).EqualTo(GeneralError.ItemNotFound));
        }

        [Test, CustomAutoData]
        public void AddBasketProduct_validateNotBasketOwner(IFixture fixture, BasketProductCreationModel model, Person person, Storage storage, Basket basket)
        {
            var personRepMock = fixture.Freeze<Mock<IPersonRepository>>();
            var storageRepMock = fixture.Freeze<Mock<IStorageRepository>>();
            var basketRepMock = fixture.Freeze<Mock<IBasketRepository>>();
            person.Id = 1;
            storage.PersonId = 1;
            basket.PersonId = 2;

            personRepMock.Setup(x => x.GetPersonByEmail(It.IsAny<string>())).Returns(person);
            basketRepMock.Setup(x => x.GetBasketById(It.IsAny<int>())).Returns(basket);
            storageRepMock.Setup(x => x.GetStorageById(It.IsAny<int>())).Returns(storage);

            var sut = fixture.Create<BasketProductService>();
            var actual = sut.AddBasketProduct(model, person.Email);

            var error = actual.Errors.First();
            Assert.That(error, Has.Property(nameof(error.ErrorEnum)).EqualTo(GeneralError.AccessDenied));
        }
        
        [Test, CustomAutoData]
        public void AddBasketProduct_validateNotStorageOwner(IFixture fixture, BasketProductCreationModel model, Person person, Storage storage, Basket basket)
        {
            var personRepMock = fixture.Freeze<Mock<IPersonRepository>>();
            var storageRepMock = fixture.Freeze<Mock<IStorageRepository>>();
            var basketRepMock = fixture.Freeze<Mock<IBasketRepository>>();
            person.Id = 1;
            storage.PersonId = 2;
            basket.PersonId = 1;

            personRepMock.Setup(x => x.GetPersonByEmail(It.IsAny<string>())).Returns(person);
            basketRepMock.Setup(x => x.GetBasketById(It.IsAny<int>())).Returns(basket);
            storageRepMock.Setup(x => x.GetStorageById(It.IsAny<int>())).Returns(storage);

            var sut = fixture.Create<BasketProductService>();
            var actual = sut.AddBasketProduct(model, person.Email);

            var error = actual.Errors.First();
            Assert.That(error, Has.Property(nameof(error.ErrorEnum)).EqualTo(GeneralError.AccessDenied));
        }

        [Test, CustomAutoData]
        public void AddBasketProduct_validateSuccess(IFixture fixture, BasketProductCreationModel model, Person person, Storage storage, Basket basket)
        {
            var personRepMock = fixture.Freeze<Mock<IPersonRepository>>();
            var storageRepMock = fixture.Freeze<Mock<IStorageRepository>>();
            var basketRepMock = fixture.Freeze<Mock<IBasketRepository>>();
            person.Id = 1;
            storage.PersonId = 1;
            basket.PersonId = 1;

            personRepMock.Setup(x => x.GetPersonByEmail(It.IsAny<string>())).Returns(person);
            basketRepMock.Setup(x => x.GetBasketById(It.IsAny<int>())).Returns(basket);
            storageRepMock.Setup(x => x.GetStorageById(It.IsAny<int>())).Returns(storage);

            var sut = fixture.Create<BasketProductService>();
            var actual = sut.AddBasketProduct(model, person.Email);

            var result = actual.Successful();
            Assert.IsTrue(result);
        }
    }
}
