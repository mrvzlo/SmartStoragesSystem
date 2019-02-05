using System.Web.Mvc;

namespace SmartKitchen.Controllers
{
	[HandleError]
    public class ErrorController : Controller
    {
        public ActionResult PageNotFound(string path)
        {
	        ViewBag.path = path;
            return View();
        }
    }
}