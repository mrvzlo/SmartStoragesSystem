using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CommonServiceLocator;
using StructureMap;

namespace SmartKitchen.IoC
{
    public class StructureMapDependencyScope : ServiceLocatorImplBase
    {
        #region Constants and Fields

        private const string NestedContainerKey = "Nested.Container.Key";

        #endregion Constants and Fields

        #region Constructors and Destructors

        public StructureMapDependencyScope(object container)
        {
            Container = container as IContainer;
            if (Container == null) throw new ArgumentNullException("container");
        }

        #endregion Constructors and Destructors

        #region Public Properties

        public IContainer Container { get; set; }

        public IContainer CurrentNestedContainer
        {
            get
            {
                return (IContainer)HttpContext?.Items[NestedContainerKey];
            }
            set
            {
                HttpContext.Items[NestedContainerKey] = value;
            }
        }

        #endregion Public Properties

        #region Properties

        private HttpContextBase HttpContext
        {
            get
            {
                var ctx = Container.TryGetInstance<HttpContextBase>();
                return ctx ?? (System.Web.HttpContext.Current != null
                           ? new HttpContextWrapper(System.Web.HttpContext.Current)
                           : null);
            }
        }

        #endregion Properties

        #region Public Methods and Operators

        public void CreateNestedContainer()
        {
            if (CurrentNestedContainer != null) return;
            CurrentNestedContainer = Container.GetNestedContainer();
        }

        public void Dispose()
        {
            DisposeNestedContainer();
            Container.Dispose();
        }

        public void DisposeNestedContainer()
        {
            if (CurrentNestedContainer == null) return;
            CurrentNestedContainer.Dispose();
            CurrentNestedContainer = null;
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return DoGetAllInstances(serviceType);
        }

        #endregion Public Methods and Operators

        #region Methods

        protected override IEnumerable<object> DoGetAllInstances(Type serviceType)
        {
            return (CurrentNestedContainer ?? Container).GetAllInstances(serviceType).Cast<object>();
        }

        protected override object DoGetInstance(Type serviceType, string key)
        {
            var container = (CurrentNestedContainer ?? Container);

            var wh = container.WhatDoIHave();
            if (string.IsNullOrWhiteSpace(key))
                return serviceType.IsAbstract || serviceType.IsInterface
                    ? container.TryGetInstance(serviceType)
                    : container.GetInstance(serviceType);
            return container.GetInstance(serviceType, key);
        }

        #endregion Methods
    }
}
