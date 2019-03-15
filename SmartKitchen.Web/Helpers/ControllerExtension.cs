using JetBrains.Annotations;
using System.IO;
using System.Web.Mvc;

namespace SmartKitchen.Web.Helpers
{
    public static class ControllerExtension
    {
        public static string RenderPartialViewToString(this Controller controller, [AspMvcView] string viewName = null, object model = null)
        {
            if (string.IsNullOrEmpty(viewName))
            {
                viewName = controller.ControllerContext.RouteData.GetRequiredString("action");
            }

            controller.ViewData.Model = model;

            using (var sw = new StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindPartialView(controller.ControllerContext, viewName);
                var viewContext = new ViewContext(controller.ControllerContext, viewResult.View, controller.ViewData,
                    controller.TempData, sw);

                viewResult.View.Render(viewContext, sw);

                return sw.GetStringBuilder().ToString();
            }
        }
    }
}