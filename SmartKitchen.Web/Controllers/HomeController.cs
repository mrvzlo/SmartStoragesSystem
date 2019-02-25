using System.Linq;
using System.Web.Mvc;
using SmartKitchen.Domain.IService;
using SmartKitchen.Models;

namespace SmartKitchen.Controllers
{
	public class HomeController : Controller
    {
        private readonly IHomeService _homeService;

        public HomeController(IHomeService homeService)
        {
            _homeService = homeService;
        }

        public ActionResult Index()
		{
			return View();
		}
		public ActionResult About()
        {
            var model = _homeService.GetHelpModel();
            return View(model);
		}
	}
}