using System.Web.Mvc;
using CommonServiceLocator;
using Microsoft.Web.Infrastructure.DynamicModuleHelper;
using SmartKitchen.IoC;
using SmartKitchen.Web;
using WebActivatorEx;

[assembly: System.Web.PreApplicationStartMethod(typeof(StructuremapMvc), "Start")]
[assembly: ApplicationShutdownMethod(typeof(StructuremapMvc), "End")]

namespace SmartKitchen.Web
{
    public static class StructuremapMvc
    {
        #region Public Methods and Operators

        public static void End()
        {
            StructureMapScopeModule.CurrentDependencyScope.Dispose();
        }

        public static void Start()
        {

            var container = IoCWeb.Initialize();
            // Create scope before assigning to ServiceLocator, ServiceLocator might recreate it
            var scope = new StructureMapDependencyScope(container);
            // Common Service Locator setup
            ServiceLocator.SetLocatorProvider(() => scope);
            // Mvc dependency resolver setup
            DependencyResolver.SetResolver(ServiceLocator.Current);
            // Registers Http module to create & dispose http scoped objects
            DynamicModuleUtility.RegisterModule(typeof(StructureMapScopeModule));

            // uncomment to see what dependencies are registered within container.
            //var wdih = container.WhatDoIHave();
        }

        #endregion Public Methods and Operators
    }
}