using SmartKitchen.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using SmartKitchen.Domain.Enitities;
using SmartKitchen.Domain.IServices;

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

        public ActionResult Index(int order = 1)
        {
            ViewBag.Order = order;
            if (TempData.ContainsKey("error")) ModelState.AddModelError("Name", TempData["error"].ToString());
            return View(GetAllProducts(order));
        }

        public PartialViewResult Categories()
        {
            var list = _categoryService.GetAllCategoryDisplays();
            return PartialView("_Categories", list);
        }

        [HttpPost]
        public RedirectResult Add(string name)
        {
            var response = _productService.AddProduct(name);
            if (!response.Successful())
            {
                AddModelStateErrors(response);
                TempData["error"] = "This name is already taken";
            }
            return Redirect(Url.Action("Index"));
        }

        [HttpPost]
        public RedirectResult SaveChanges(List<ProductDisplay> list)
        {
            using (var db = new Context())
            {
                foreach (var i in list)
                {
                    var product = db.Products.Find(i.Product.Id);
                    if (product == null) continue;
                    if (product.Name != i.Product.Name) product.Name = i.Product.Name.Trim();
                    if (db.Categories.Find(i.Product.Category) == null) continue;
                    if (product.Category != i.Product.Category) product.Category = i.Product.Category;
                }

                db.SaveChanges();
            }
            return Redirect(Url.Action("Index"));
        }

        private List<ProductDisplay> GetAllProducts(int order)
        {
            using (var db = new Context())
            {
                var list = new List<ProductDisplay>();
                var products = db.Products.ToList();
                foreach (var i in products)
                    list.Add(new ProductDisplay
                    {
                        Product = i,
                        CategoryName = db.Categories.Find(i.Category).Name,
                        Usages = db.Cells.Count(x => x.Product == i.Id)
                    });

                switch (order)
                {
                    default:
                        return list.OrderBy(x => x.Product.Name).ToList();
                    case -1:
                        return list.OrderByDescending(x => x.Product.Name).ToList();
                    case 2:
                        return list.OrderBy(x => x.CategoryName).ToList();
                    case -2:
                        return list.OrderByDescending(x => x.CategoryName).ToList();
                    case 3:
                        return list.OrderBy(x => x.Usages).ToList();
                    case -3:
                        return list.OrderByDescending(x => x.Usages).ToList();
                }
            }
        }
    }
}