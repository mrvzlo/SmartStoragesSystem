using SmartKitchen.Domain.CreationModels;
using SmartKitchen.Domain.DisplayModels;
using SmartKitchen.Domain.IServices;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace SmartKitchen.Web.Controllers
{
    public class ProductController : BaseController
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;

        public ProductController(IProductService productService, ICategoryService categoryService)
        {
            _productService = productService;
            _categoryService = categoryService;
        }

        /// <summary>
        /// Page with products
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            if (TempData.ContainsKey("error")) ModelState.AddModelError("Name", TempData["error"].ToString());
            return View();
        }

        /// <summary>
        /// Partial with page of products
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
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

        /// <summary>
        /// Add new unused product
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = "Admin")]
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

        /// <summary>
        /// Update product list and reload products page
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public RedirectResult SaveChanges(List<ProductDisplayModel> list)
        {
            if (ModelState.IsValid)
            {
                var updates = _productService.UpdateProductList(list);
                Log.Warn(CurrentUser() + " updated " + updates + " rows");
            }

            return Redirect(Url.Action("Index"));
        }

        /// <summary>
        /// Return list of product names according to search input
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetProductsByNameStart(string name)
        {
            var names = _productService.GetProductNamesByStart(name);
            return Json(new { list = names.ToList() });
        }
    }
}