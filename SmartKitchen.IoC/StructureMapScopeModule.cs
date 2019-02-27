using System.Web;
using CommonServiceLocator;
using StructureMap.Web.Pipeline;

namespace SmartKitchen.IoC
{

    public class StructureMapScopeModule : IHttpModule
    {
        #region Public Methods and Operators

        public static StructureMapDependencyScope CurrentDependencyScope => ServiceLocator.Current as StructureMapDependencyScope;

        public void Dispose()
        {
        }

        public void Init(HttpApplication context)
        {
            context.BeginRequest += (sender, e) => CurrentDependencyScope.CreateNestedContainer();
            context.EndRequest += (sender, e) =>
            {
                HttpContextLifecycle.DisposeAndClearAll();
                CurrentDependencyScope.DisposeNestedContainer();
            };
        }

        #endregion Public Methods and Operators
    }
}
