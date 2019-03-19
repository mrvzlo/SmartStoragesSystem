using SmartKitchen.Domain.CreationModels;
using SmartKitchen.Domain.IServices;
using System.Linq;
using System.Web.Mvc;

namespace SmartKitchen.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CategoryController : BaseController
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public ActionResult Index()
        {
            if (TempData.ContainsKey("error")) ModelState.AddModelError("Name", TempData["error"].ToString());
            var query = _categoryService.GetAllCategoryDisplays().Select(x => x.Id);
            return View(query.ToList());
        }

        [HttpPost]
        public ActionResult Create(NameCreationModel model)
        {
            var response = _categoryService.AddCategory(model);
            if (!response.Successful()) TempData["error"] = GetErrorsToString(response);
            return Redirect(Url.Action("Index"));
        }

        [HttpPost]
        public bool Remove(int fromId, int toId)
        {
            return _categoryService.ReplaceCategory(fromId, toId).Successful();
        }

        public PartialViewResult CategoryGrid()
        {
            var query = _categoryService.GetAllCategoryDisplays();
            return PartialView("_CategoryGrid", query);
        }
    }
}