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
using System.Collections.Generic;
using System.Linq;
// ReSharper disable NUnit.MethodWithParametersAndTestAttribute

namespace SmartKitchen.DomainService.Test.Tests
{
    [TestFixture, Category("Storage service")]
    public class StorageServiceTests
    {
        [Test, CustomAutoData]
        public void AddStorage_validateStorageNameNotUnique(IFixture fixture, StorageCreationModel model, Person person)
        {
            var personRepMock = fixture.Freeze<Mock<IPersonRepository>>();
            person.Storages.Add(new Storage { Name = model.Name });
            personRepMock.Setup(x => x.GetPersonByEmail(It.IsAny<string>())).Returns(person);

            var sut = fixture.Create<StorageService>();
            var actual = sut.AddStorage(model, person.Email);

            var error = actual.Errors.First();
            Assert.That(error, Has.Property(nameof(error.ErrorEnum)).EqualTo(GeneralError.NameIsAlreadyTaken));
        }

        [Test, CustomAutoData]
        public void AddStorage_validateStorageNameUnique(IFixture fixture, StorageCreationModel model, Person person)
        {
            var personRepMock = fixture.Freeze<Mock<IPersonRepository>>();
            person.Storages = new List<Storage>();
            personRepMock.Setup(x => x.GetPersonByEmail(It.IsAny<string>())).Returns(person);

            var sut = fixture.Create<StorageService>();
            var actual = sut.AddStorage(model, person.Email);

            var result = actual.Successful();
            Assert.IsTrue(result);
        }

        [Test, CustomAutoData]
        public void GetStorageDescriptionById_Map_StorageDisplayModel(IFixture fixture, [Frozen] Storage storage, Person person)
        {
            person.Id = storage.PersonId;
            var storageRepMock = fixture.Freeze<Mock<IStorageRepository>>();
            var personRepMock = fixture.Freeze<Mock<IPersonRepository>>();
            storageRepMock.Setup(x => x.GetStorageById(storage.Id)).Returns(storage);
            personRepMock.Setup(x => x.GetPersonByEmail(person.Email)).Returns(person);

            var sut = fixture.Create<StorageService>();
            var actual = sut.GetStorageById(storage.Id, person.Email);

            Assert.NotNull(actual);
            var type = actual.Type;
            Assert.That(actual, Has.Property(nameof(StorageDisplayModel.Id)).EqualTo(storage.Id));
            Assert.That(actual, Has.Property(nameof(StorageDisplayModel.Name)).EqualTo(storage.Name));
            Assert.That(actual, Has.Property(nameof(StorageDisplayModel.CellCount)).EqualTo(storage.Cells.Count));
            Assert.That(type, Has.Property(nameof(StorageTypeDisplayModel.Id)).EqualTo(storage.Type.Id));
            Assert.That(type, Has.Property(nameof(StorageTypeDisplayModel.Name)).EqualTo(storage.Type.Name));
            Assert.That(type, Has.Property(nameof(StorageTypeDisplayModel.Background)).EqualTo(storage.Type.Background));
        }
    }
}