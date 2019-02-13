using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using SmartKitchen.Models;

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
            return PartialView("_Categories", GetAllCategories().Select(x => x.Name));
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
                var list = Mapper.Map<List<ProductDisplay>>(db.Products);
                foreach (var i in list)
                {
                    i.CategoryName = db.Categories.Find(i.Category).Name;
                }

                return list.OrderBy(x=>x.Category).ToList();
            }
        }
    }
}