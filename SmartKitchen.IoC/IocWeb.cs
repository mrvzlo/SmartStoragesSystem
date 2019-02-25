using System;
using System.Linq.Expressions;
using CommonServiceLocator;
using CoreMlm.IoC.Registries;
using CoreMlm.IoC.Scope;
using SmartKitchen.IoC;
using StructureMap;
using StructureMap.AutoMocking;

namespace CoreMlm.IoC
{
    public static class IoCWeb
    {
        public static IContainer Initialize()
        {
            return new Container(c => c.AddRegistry<DefaultRegistry>());
        }

        /// <summary>
        /// Add additional configuration after initialize
        /// </summary>
        /// <typeparam name="T">The type of</typeparam>
        /// <param name="action">The objct action to use</param>
        public static void InjectAdditional<T>(Expression<Func<T>> action)
        {
            ((StructureMapDependencyScope)ServiceLocator.Current).Container
                .Configure(x => x.For<T>().Use(action));
        }
    }
}
