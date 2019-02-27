using SmartKitchen.Domain.DisplayModels;
using SmartKitchen.Domain.IServices;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using SmartKitchen.Domain.CreationModels;

namespace SmartKitchen.Controllers
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
            return PartialView("_ProductGrid", query.ToList());
        }

        public PartialViewResult Categories()
        {
            var query = _categoryService.GetAllCategoryDisplays();
            return PartialView("_Categories", query.ToList());
        }

        [HttpPost]
        public RedirectResult Add(NameCreationModel model)
        {
            var response = _productService.AddProduct(model);
            if (!response.Successful())
                TempData["error"] = GetErrorsToString(response);
            return Redirect(Url.Action("Index"));
        }

        [HttpPost]
        public RedirectResult SaveChanges(List<ProductDisplayModel> list)
        {
            _productService.UpdateProductList(list);
            return Redirect(Url.Action("Index"));
        }
        
    }
}