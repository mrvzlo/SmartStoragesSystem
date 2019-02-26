using System.Collections.Generic;
using SmartKitchen.Models;
using System.Linq;
using System.Web.Mvc;
using SmartKitchen.Domain.DisplayModel;
using SmartKitchen.Domain.Enitities;
using SmartKitchen.Domain.IRepositories;
using SmartKitchen.Domain.IServices;

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
            if (!response.IsSuccessful) TempData["error"] = FormError.NameIsAlreadyTaken;
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