using System;
using System.Linq;
using AutoFixture;
using AutoFixture.AutoMoq;
using AutoMapper;
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
            fixture.Customize(new AutoConfiguredMoqCustomization());
            fixture.Register(() => fixture.CreateMany<Basket>(3).AsQueryable());
            fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList().ForEach(b => fixture.Behaviors.Remove(b));
            fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        }
    }
}