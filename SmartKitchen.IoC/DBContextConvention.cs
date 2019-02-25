using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StructureMap;
using StructureMap.Graph;
using StructureMap.Graph.Scanning;
using StructureMap.TypeRules;

namespace SmartKitchen.IoC
{
    public class DBContextConvention : IRegistrationConvention
    {
        private readonly string _nameOrConnectionString;

        public DBContextConvention(string nameOrConnectionString = "DefaultConnection")
        {
            _nameOrConnectionString = nameOrConnectionString;
        }

        public void ScanTypes(TypeSet types, Registry registry)
        {
            var matches = types.FindTypes(TypeClassification.Concretes | TypeClassification.Closed)
                .Where(type => TypeExtensions.CanBeCastTo<DbContext>(type));
            foreach (var type in matches)
            {
                registry.For(type)
                    .Use(type)
                    .Ctor<string>()
                    .Is(ConfigurationManager.ConnectionStrings[_nameOrConnectionString].Name);
                registry.For(typeof(DbContext)).Add(x => x.GetInstance(type)).Named(type.Name);
            }
        }

    }
}
