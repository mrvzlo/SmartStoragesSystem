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

        /// <summary>
        /// Page with all categories
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            if (TempData.ContainsKey("error")) ModelState.AddModelError("Name", TempData["error"].ToString());
            var query = _categoryService.GetAllCategoryDisplays().Select(x => x.Id);
            return View(query.ToList());
        }

        /// <summary>
        /// Add new category and reload page with list of categories
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Create(NameCreationModel model)
        {
            var response = _categoryService.AddCategory(model);
            if (!response.Successful()) TempData["error"] = GetErrorsToString(response);
            return Redirect(Url.Action("Index"));
        }

        /// <summary>
        /// Replace one category with another and return successfulness of action
        /// </summary>
        /// <param name="fromId"></param>
        /// <param name="toId"></param>
        /// <returns></returns>
        [HttpPost]
        public bool Remove(int fromId, int toId) => _categoryService.ReplaceCategory(fromId, toId).Successful();

        /// <summary>
        /// Partial page with list of categories
        /// </summary>
        /// <returns></returns>
        public PartialViewResult CategoryGrid()
        {
            var query = _categoryService.GetAllCategoryDisplays();
            return PartialView("_CategoryGrid", query);
        }
    }
}