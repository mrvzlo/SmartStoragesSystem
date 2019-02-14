using SmartKitchen.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace SmartKitchen.Controllers
{
    [Authorize(Roles = "admin")]
    public class ProductController : Controller
    {
        // GET: Product
        public ActionResult Index()
        {
            return View(GetAllProducts());
        }

        public PartialViewResult Categories()
        {
            return PartialView("_Categories", GetAllCategories());
        }

        [HttpPost]
        public RedirectResult Add(string name)
        {
            using (var db = new Context())
            {
                if (!db.Products.Any(x => x.Name == name))
                {
                    db.Products.Add(new Product { Category = 1, Name = name });
                    db.SaveChanges();
                }
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

        private List<Category> GetAllCategories()
        {
            using (var db = new Context())
            {
                return db.Categories.ToList();
            }
        }
        private List<ProductDisplay> GetAllProducts()
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

                return list.OrderBy(x => x.Product.Name).ToList();
            }
        }
    }
}