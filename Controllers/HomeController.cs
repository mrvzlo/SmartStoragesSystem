using System.Linq;
using System.Web.Mvc;
using SmartKitchen.Models;

namespace SmartKitchen.Controllers
{
	public class HomeController : Controller
	{
		public ActionResult Index()
		{
			return View();
		}
		public ActionResult About()
		{
            using (var db = new Context())
            {
                ViewBag.StorageCount = db.StorageTypes.Count();
                ViewBag.ProductCount = db.Products.Count();
            }
			return View();
		}
	}
}