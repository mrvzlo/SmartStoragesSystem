using System.Linq;
using AutoFixture;
using AutoFixture.AutoMoq;
using AutoMapper;
using SmartKitchen.Domain.DisplayModels;
using SmartKitchen.Domain.Enitities;
using SmartKitchen.DomainService.Profiles;

namespace SmartKitchen.DomainService.Test
{
    public class BaseCustomization : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            fixture.Inject(new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<BasketProfile>();
                cfg.AddProfile<CategoryProfile>();
                cfg.AddProfile<CellProfile>();
                cfg.AddProfile<ProductProfile>();
                cfg.AddProfile<StorageProfile>();
            }).CreateMapper());
            fixture.Customize(new AutoMoqCustomization{ConfigureMembers = true});
            fixture.Register(() => fixture.CreateMany<Basket>(3).AsQueryable());
            fixture.Register(() => fixture.CreateMany<Storage>(3).AsQueryable());
            fixture.Register(() => fixture.CreateMany<Product>(3).AsQueryable());
            fixture.Register(() => fixture.CreateMany<Cell>(3).AsQueryable());
            fixture.Register(() => fixture.CreateMany<CellChange>(3).AsQueryable());
            fixture.Register(() => fixture.CreateMany<Category>(3).AsQueryable());
            fixture.Register(() => fixture.CreateMany<ProductDisplayModel>(3).ToList());
            fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList().ForEach(b => fixture.Behaviors.Remove(b));
            fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        }
    }
}