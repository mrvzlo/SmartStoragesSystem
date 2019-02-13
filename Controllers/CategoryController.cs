﻿using AutoMapper;
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
            return View(CategoryDisplay.GetIds());
        }

        [HttpPost]
        public ActionResult Create(string name)
        {
            var result = Redirect(Url.Action("Index"));
            if (string.IsNullOrWhiteSpace(name)) return result;
            name = name.Trim();
            name = name[0].ToString().ToUpper() + name.Substring(1).ToLower();
            using (var db = new Context())
            {
                var exists = db.Categories.FirstOrDefault(x => x.Name == name);
                if (exists != null) return result;
                db.Categories.Add(new Category { Name = name });
                db.SaveChanges();
            }

            return result;
        }

        public ActionResult Description(int id)
        {
            CategoryDisplay description;
            using (var db = new Context())
            {
                description = Mapper.Map<CategoryDisplay>(db.Categories.Find(id));
                description.ProductsCount = db.Products.Count(x => x.Category == description.Id);
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