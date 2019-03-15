using StructureMap;

namespace SmartKitchen.IoC
{
    public static class IoCWeb
    {
        public static IContainer Initialize()
        {
            return new Container(c => c.AddRegistry<DefaultRegistry>());
        }
    }
}
