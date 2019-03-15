using AutoFixture;
using AutoFixture.NUnit3;

namespace SmartKitchen.DomainService.Test
{
    public class CustomAutoDataAttribute : AutoDataAttribute
    {
        public CustomAutoDataAttribute() : base(GetFixture) { }

        public static IFixture GetFixture()
        {
            return new Fixture().Customize(new BaseCustomization());
        }
    }
}