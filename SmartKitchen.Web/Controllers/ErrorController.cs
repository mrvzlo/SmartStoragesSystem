using System.Web.Mvc;

namespace SmartKitchen.Web.Controllers
{
	[HandleError]
    public class ErrorController : Controller
    {
        public ActionResult Index(int id=0)
        {
            switch (id)
            {
                case 403: return Redirect(Url.Action("Forbidden"));
                case 404: return Redirect(Url.Action("PageNotFound"));
                default: return Redirect(Url.Action("Index", "Home"));
            }
        }
        public ActionResult PageNotFound(string path)
        {
            ViewBag.path = path;
            Response.StatusCode = 404;
            return View();
        }
        public ActionResult Forbidden(string path)
        {
            ViewBag.path = path;
            Response.StatusCode = 403;
            return View();
        }
    }
}