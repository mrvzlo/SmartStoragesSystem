using System.Linq;
using System.Security.Policy;
using System.Web.Mvc;
using SmartKitchen.Domain.CreationModels;
using SmartKitchen.Domain.DisplayModels;
using SmartKitchen.Domain.Enums;
using SmartKitchen.Domain.Extensions;
using SmartKitchen.Domain.IServices;

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
            var query = _categoryService.GetAllCategoryDisplays();
            return View(query.ToList());
        }

        [HttpPost]
        public ActionResult Create(NameCreationModel model)
        {
            var response = _categoryService.AddCategoryWithName(model);
            if (!response.Successful()) TempData["error"] = GetErrorsToString(response);
            return Redirect(Url.Action("Index"));
        }

        public ActionResult Remove(int fromId, int toId)
        {
            _categoryService.ReplaceCategory(fromId, toId);
            return Redirect(Url.Action("Index"));
        }

        public PartialViewResult CategoryGrid()
        {
            var query = _categoryService.GetAllCategoryDisplays();
            return PartialView("_CategoryGrid", query);
        }
    }
}