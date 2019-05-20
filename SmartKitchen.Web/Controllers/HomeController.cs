using System.Web.Mvc;
using SmartKitchen.Domain.IServices;

namespace SmartKitchen.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHomeService _homeService;
        private readonly IPersonService _personService;

        public HomeController(IHomeService homeService, IPersonService personService)
        {
            _homeService = homeService;
            _personService = personService;
        }

        /// <summary>
        /// If no request show home page else preform the request
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ActionResult Index(string request = null)
        {
            if (request == null) return View();
            var result = _personService.Interpretator(request);
            return Json(new { success = result.Successful(), errors = result.Errors });
        }

        /// <summary>
        /// Show guide about the system
        /// </summary>
        /// <returns></returns>
        public ActionResult About()
        {
            var model = _homeService.GetHelpModel();
            return View(model);
        }
    }
}