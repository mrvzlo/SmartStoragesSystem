using SmartKitchen.Domain.IServices;
using System.Web.Mvc;
using SmartKitchen.Domain.Enums;

namespace SmartKitchen.Controllers
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
            var list = _categoryService.GetAllCategoryDisplays();
            return View(list);
        }

        [HttpPost]
        public ActionResult Create(string name)
        {
            var response = _categoryService.AddCategoryWithName(name);
            if (!response.Successful())
            {
                AddModelStateErrors(response);
                TempData["error"] = GeneralError.NameIsAlreadyTaken;
            }
            return Redirect(Url.Action("Index"));
        }

        public ActionResult Description(int id)
        {
            var description = _categoryService.GetCategoryDisplayById(id);
            return PartialView("_Description", description);
        }

        public ActionResult Remove(int fromId, int toId)
        {
            _categoryService.ReplaceCategory(fromId, toId);
            return Redirect(Url.Action("Index"));
        }
    }
}