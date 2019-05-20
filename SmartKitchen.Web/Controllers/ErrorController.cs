using System.Web.Mvc;

namespace SmartKitchen.Web.Controllers
{
	[HandleError]
    public class ErrorController : Controller
    {
        /// <summary>
        /// Return error page according to error type
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Index(int id=0)
        {
            switch (id)
            {
                case 403: return Redirect(Url.Action("Forbidden"));
                case 404: return Redirect(Url.Action("PageNotFound"));
                default: return Redirect(Url.Action("Index", "Home"));
            }
        }

        /// <summary>
        /// 404 error page
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public ActionResult PageNotFound(string path)
        {
            ViewBag.path = path;
            Response.StatusCode = 404;
            return View();
        }

        /// <summary>
        /// 403 error page
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public ActionResult Forbidden(string path)
        {
            ViewBag.path = path;
            Response.StatusCode = 403;
            return View();
        }
    }
}