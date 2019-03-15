using AutoMapper;
using StructureMap;
using StructureMap.Graph;
using StructureMap.Graph.Scanning;
using StructureMap.TypeRules;
using System;
using System.Linq;

namespace SmartKitchen.IoC.Convention
{
    public class AutoMapperConvention : IRegistrationConvention
    {
        private readonly Action<MapperConfiguration> _mapperConfig;

        /// <summary>
        /// AutoMapper registration convention
        /// </summary>
        public AutoMapperConvention()
        {
            _mapperConfig = null;
        }

        /// <summary>
        /// AutoMapper registration convention
        /// </summary>
        /// <param name="mapperConfig">Additional Auto Mappper configuration</param>
        public AutoMapperConvention(Action<MapperConfiguration> mapperConfig)
        {
            _mapperConfig = mapperConfig;
        }

        public void ScanTypes(TypeSet types, Registry registry)
        {
            var profiles = types.FindTypes(TypeClassification.Concretes | TypeClassification.Closed)
                .Where(type => type.CanBeCastTo<Profile>()).Select(type => Activator.CreateInstance(type) as Profile);

            var config = new MapperConfiguration(cfg =>
            {
                foreach (var profile in profiles)
                    cfg.AddProfile(profile);
            });

            registry.For<MapperConfiguration>().Use(config);
            registry.For<IMapper>().Use(ctx => ctx.GetInstance<MapperConfiguration>().CreateMapper(ctx.GetInstance));
            registry.Policies.FillAllPropertiesOfType<Lazy<IMapper>>();
        }
    }
}
