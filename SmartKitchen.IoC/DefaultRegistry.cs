using System.Linq;
using SmartKitchen.IoC.Convention;
using StructureMap;

namespace SmartKitchen.IoC
{
    public class DefaultRegistry : Registry
    {
        public static readonly string[] NamespacesToScan = { "SmartKitchen." };

        public DefaultRegistry()
        {
            Scan(scan =>
            {
                scan.AssembliesFromApplicationBaseDirectory(a => NamespacesToScan.Any(x => a.FullName.StartsWith(x)));
                scan.WithDefaultConventions();
                scan.With(new ControllerConvention());
                scan.With(new DbContextConvention());
                scan.With(new AutoMapperConvention());
                scan.LookForRegistries();
            });
        }
    }
}
