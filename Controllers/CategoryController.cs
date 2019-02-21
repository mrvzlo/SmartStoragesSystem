using System.Collections.Generic;
using SmartKitchen.Models;
using System.Linq;
using System.Web.Mvc;

namespace SmartKitchen.Controllers
{
    [Authorize(Roles = "admin")]
    public class CategoryController : Controller
    {
        public ActionResult Index()
        {
            if (TempData.ContainsKey("error")) ModelState.AddModelError("Name", TempData["error"].ToString());
            var list = new List<CategoryDisplay>();
            using (var db = new Context())
            {
                foreach (var i in db.Categories.ToList())
                {
                    var cd = new CategoryDisplay();
                    cd.Category = i;
                    cd.ProductsCount = db.Products.Count(x => x.Category == i.Id);
                    list.Add(cd);
                }
            }
            return View(list);
        }

        [HttpPost]
        public ActionResult Create(string name)
        {
            var result = Redirect(Url.Action("Index"));
            if (string.IsNullOrWhiteSpace(name)) return result;
            using (var db = new Context())
            {
                if (db.Categories.Any(x => x.Name == name.Trim()))
                {
                    TempData["error"] = "This name is already taken";
                    return Redirect(Url.Action("Index"));
                }

                db.Categories.Add(new Category { Name = name });
                db.SaveChanges();
            }

            return result;
        }

        public ActionResult Description(int id)
        {
            CategoryDisplay description = new CategoryDisplay();
            using (var db = new Context())
            {
                description.Category = db.Categories.Find(id);
                description.ProductsCount = db.Products.Count(x => x.Category == description.Category.Id);
            }
            return PartialView("_Description", description);
        }

        public ActionResult Remove(int fromId, int toId)
        {
            var result = Redirect(Url.Action("Index"));
            using (var db = new Context())
            {
                var from = db.Categories.Find(fromId);
                var to = db.Categories.Find(toId);
                if (from == null || to == null || fromId == 1) return result;
                foreach (var product in db.Products.Where(x => x.Category == fromId))
                {
                    product.Category = toId;
                }
                db.Categories.Remove(from);
                db.SaveChanges();
            }
            return result;
        }
    }
}