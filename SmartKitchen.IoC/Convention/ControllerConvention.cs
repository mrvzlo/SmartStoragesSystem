// ReSharper disable PossibleNullReferenceException
using System;
using System.Linq;
using System.Web.Mvc;
using StructureMap;
using StructureMap.Graph;
using StructureMap.Graph.Scanning;
using StructureMap.Pipeline;
using StructureMap.TypeRules;

namespace SmartKitchen.IoC.Convention
{
    class ControllerConvention : IRegistrationConvention
    {
        private readonly string[] _emptyConstructorTypes;

        /// <summary>
        /// The Mvc controller registration convention constructor
        /// </summary>
        /// <param name="emptyConstructorTypes">Sets to use empty constructor for the types specified</param>
        public ControllerConvention(params string[] emptyConstructorTypes)
        {
            _emptyConstructorTypes = emptyConstructorTypes;
        }

        public void ScanTypes(TypeSet types, Registry registry)
        {
            var matches = types.FindTypes(TypeClassification.Concretes | TypeClassification.Closed)
                .Where(type => type.CanBeCastTo<Controller>());
            foreach (var type in matches)
            {
                var expression = registry.For(type).LifecycleIs(new UniquePerRequestLifecycle()).Use(type);
                if (_emptyConstructorTypes.Any(name => type.FullName.EndsWith(name)))
                    expression.Constructor = type.GetConstructor(new Type[0]);
            }
        }
    }
}
