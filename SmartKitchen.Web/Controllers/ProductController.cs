using SmartKitchen.Domain.CreationModels;
using SmartKitchen.Domain.DisplayModels;
using SmartKitchen.Domain.IServices;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace SmartKitchen.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ProductController : BaseController
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;

        public ProductController(IProductService productService, ICategoryService categoryService)
        {
            _productService = productService;
            _categoryService = categoryService;
        }

        public ActionResult Index()
        {
            if (TempData.ContainsKey("error")) ModelState.AddModelError("Name", TempData["error"].ToString());
            return View();
        }

        public PartialViewResult ProductGrid()
        {
            var query = _productService.GetAllProductDisplays();
            ViewBag.SelectList = _categoryService.GetAllCategoryDisplays().Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.Name
            }).ToList();
            return PartialView("_ProductGrid", query);
        }

        [HttpPost]
        public RedirectResult Add(NameCreationModel model)
        {
            if (ModelState.IsValid)
            {
                Log.Warn(CurrentUser() + " added product " + model.Name);
                var response = _productService.AddProduct(model);
                if (!response.Successful())
                    TempData["error"] = GetErrorsToString(response);
            }
            return Redirect(Url.Action("Index"));
        }

        [HttpPost]
        public RedirectResult SaveChanges(List<ProductDisplayModel> list)
        {
            if (ModelState.IsValid)
            {
                int updates = _productService.UpdateProductList(list);
                Log.Warn(CurrentUser() + " updated " + updates + " rows");
            }

            return Redirect(Url.Action("Index"));
        }

    }
}