using System.Linq;
using AutoFixture;
using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using SmartKitchen.Domain.CreationModels;
using SmartKitchen.Domain.DisplayModels;
using SmartKitchen.Domain.Enitities;
using SmartKitchen.Domain.Enums;
using SmartKitchen.Domain.IRepositories;
using SmartKitchen.Domain.IServices;
using SmartKitchen.DomainService.Services;
// ReSharper disable NUnit.MethodWithParametersAndTestAttribute

namespace SmartKitchen.DomainService.Test.Tests
{
    [TestFixture, Category("Cell service")]
    public class CellServiceTests
    {
        [Test, CustomAutoData]
        public void GetOrAddAndGetCell_ShouldGet(IFixture fixture, Person person, Storage storage, Product product)
        {
            var model = new CellCreationModel { Product = product.Name, Storage = storage.Id };
            storage.PersonId = person.Id;

            var storageRepMock = fixture.Freeze<Mock<IStorageRepository>>();
            var personRepMock = fixture.Freeze<Mock<IPersonRepository>>();
            var productRepMock = fixture.Freeze<Mock<IProductService>>();
            var cellRepMock = fixture.Freeze<Mock<ICellRepository>>();
            storageRepMock.Setup(x => x.GetStorageById(It.IsAny<int>())).Returns(storage);
            personRepMock.Setup(x => x.GetPersonByEmail(person.Email)).Returns(person);
            productRepMock.Setup(x => x.GetOrAddAndGetProduct(It.IsAny<string>())).Returns(product);
            cellRepMock.Setup(x => x.GetCellById(It.IsAny<int>())).Returns((Cell)null);

            var sut = fixture.Create<CellService>();
            var actual = sut.GetOrAddAndGetCell(model, person.Email);

            cellRepMock.Verify(x => x.GetCellById(It.IsAny<int>()), Times.Never());
            Assert.NotNull(actual);
        }

        [Test, CustomAutoData]
        public void GetOrAddAndGetCell_ShouldAdd(IFixture fixture, Person person, Storage storage, Product product, Cell cell)
        {
            var model = new CellCreationModel { Product = product.Name, Storage = storage.Id };
            storage.PersonId = person.Id;

            var storageRepMock = fixture.Freeze<Mock<IStorageRepository>>();
            var personRepMock = fixture.Freeze<Mock<IPersonRepository>>();
            var productRepMock = fixture.Freeze<Mock<IProductService>>();
            var cellRepMock = fixture.Freeze<Mock<ICellRepository>>();
            storageRepMock.Setup(x => x.GetStorageById(It.IsAny<int>())).Returns(storage);
            personRepMock.Setup(x => x.GetPersonByEmail(person.Email)).Returns(person);
            productRepMock.Setup(x => x.GetOrAddAndGetProduct(It.IsAny<string>())).Returns(product);
            cellRepMock.Setup(x => x.GetCellById(It.IsAny<int>())).Returns(cell);
            cellRepMock.Setup(x => x.GetCellByProductAndStorage(It.IsAny<int>(), It.IsAny<int>())).Returns((Cell)null);

            var sut = fixture.Create<CellService>();
            var actual = sut.GetOrAddAndGetCell(model, person.Email);

            cellRepMock.Verify(x => x.GetCellById(It.IsAny<int>()), Times.Once());
            Assert.NotNull(actual);
        }

        [Test, CustomAutoData]
        public void AddCell_ValidateNameUnique(IFixture fixture, CellCreationModel model, Product product, Storage storage, Person person, Cell cell)
        {
            person.Id = storage.PersonId;
            var cellRepMock = fixture.Freeze<Mock<ICellRepository>>();
            var productRepMock = fixture.Freeze<Mock<IProductService>>();
            var storageRepMock = fixture.Freeze<Mock<IStorageRepository>>();
            product.Name = model.Product;
            storageRepMock.Setup(x => x.GetStorageById(It.IsAny<int>())).Returns(storage);
            productRepMock.Setup(x => x.GetOrAddAndGetProduct(It.IsAny<string>())).Returns(product);
            cellRepMock.Setup(x => x.GetCellByProductAndStorage(It.IsAny<int>(), It.IsAny<int>())).Returns((Cell)null);

            var sut = fixture.Create<CellService>();
            var actual = sut.AddCell(model, person);
            
            Assert.True(actual.Successful());
        }

        [Test, CustomAutoData]
        public void AddCell_ValidateNameNotUnique(IFixture fixture, CellCreationModel model, Product product, Storage storage, Person person, Cell cell)
        {
            person.Id = storage.PersonId;
            var cellRepMock = fixture.Freeze<Mock<ICellRepository>>();
            var productRepMock = fixture.Freeze<Mock<IProductService>>();
            var storageRepMock = fixture.Freeze<Mock<IStorageRepository>>();
            product.Name = model.Product;
            storageRepMock.Setup(x => x.GetStorageById(It.IsAny<int>())).Returns(storage);
            productRepMock.Setup(x => x.GetOrAddAndGetProduct(It.IsAny<string>())).Returns(product);
            cellRepMock.Setup(x => x.GetCellByProductAndStorage(It.IsAny<int>(), It.IsAny<int>())).Returns(cell);

            var sut = fixture.Create<CellService>();
            var actual = sut.AddCell(model, person);

            var result = actual.Errors;
            Assert.AreEqual(GeneralError.NameIsAlreadyTaken, result[0].ErrorEnum);
        }
        
        [Test, CustomAutoData]
        public void GetCellsOfStorage_Map_CellDisplay(IFixture fixture, [Frozen] IQueryable<Cell> cells)
        {
            var cell = cells.First();
            var person = new Person { Id = 1 };
            var storage = new Storage { PersonId = 1 };
            var storageRepositoryMock = fixture.Freeze<Mock<IStorageRepository>>();
            var cellRepositoryMock = fixture.Freeze<Mock<ICellRepository>>();
            var personRepositoryMock = fixture.Freeze<Mock<IPersonRepository>>();
            storageRepositoryMock.Setup(x => x.GetStorageById(It.IsAny<int>())).Returns(storage);
            personRepositoryMock.Setup(x => x.GetPersonByEmail(It.IsAny<string>())).Returns(person);
            cellRepositoryMock.Setup(x => x.GetCellsForStorage(It.IsAny<int>())).Returns(cells);

            var sut = fixture.Create<CellService>();
            var actual = sut.GetCellsOfStorage(storage.Id, person.Email).First();

            Assert.NotNull(actual);
            Assert.That(actual, Has.Property(nameof(CellDisplayModel.Id)).EqualTo(cell.Id));
            Assert.That(actual, Has.Property(nameof(CellDisplayModel.ProductName)).EqualTo(cell.Product.Name));
            Assert.That(actual, Has.Property(nameof(CellDisplayModel.CategoryName)).EqualTo(cell.Product.Category.Name));
            Assert.That(actual, Has.Property(nameof(CellDisplayModel.CellChanges)).EqualTo(cell.CellChanges));
            Assert.That(actual, Has.Property(nameof(CellDisplayModel.Amount)).EqualTo(cell.CellChanges.OrderByDescending(x => x.UpdateDate).First().Amount));
        }
    }
}