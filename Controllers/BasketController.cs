using System;
using SmartKitchen.Enums;
using SmartKitchen.Models;
using System.Web.Security;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace SmartKitchen.Controllers
{
    public class BasketController : Controller
    {
        [Authorize]
        public ActionResult Index()
        {
            List<BasketDescription> bdlist = new List<BasketDescription>();
            using (var db = new Context())
            {
                var person = Person.Current(db);
                foreach (var b in db.Baskets.Where(x => x.Owner == person.Id).ToList())
                {
                    var products = db.BasketProducts.Where(x => x.Basket == b.Id).OrderByDescending(x=>x.Bought).Select(x=>x.Id).ToList();
                    int bought = db.BasketProducts.Count(x => x.Basket == b.Id && x.Bought);
                    bdlist.Add(new BasketDescription { Basket = b, Products = products, BoughtProducts = bought });
                }
            }
            if (TempData.ContainsKey("error")) ModelState.AddModelError("Name", TempData["error"].ToString());
            return View(bdlist.OrderByDescending(x=>x.Basket.CreationDate).ToList());
        }

        [Authorize]
        [HttpPost]
        public RedirectResult Create(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return Redirect(Url.Action("Index"));
            Basket b;
            using (var db = new Context())
            {
                var person = Person.Current(db);
                if (db.Baskets.Any(x => x.Name == name && x.Owner == person.Id))
                {
                    TempData["error"] = "This name is already taken";
                    return Redirect(Url.Action("Index"));
                }

                db.Baskets.Add(new Basket { Name = name, CreationDate = DateTime.Now, Owner = person.Id, Private = true});
                db.SaveChanges();
                b = db.Baskets.FirstOrDefault(x => x.Name == name && x.Owner == person.Id);
            }

            return Redirect(Url.Action("View", new { id = b.Id }));
        }

        public ActionResult View(int id)
        {
            BasketDescription bd;
            using (var db = new Context())
            {
                var person = Person.Current(db);
                var basket = db.Baskets.Find(id);
                var response = Basket.IsOwner(basket, Person.Current(db));
                if (!response.Successfull) return Redirect(Url.Action("Index", "Error", new { id = response.Error }));
                var products = db.BasketProducts.Where(x => x.Basket == id).OrderByDescending(x => x.Bought).Select(x => x.Id).ToList();
                int bought = db.BasketProducts.Count(x => x.Basket == id && x.Bought);
                bd = new BasketDescription { Basket = basket, Products = products, BoughtProducts = bought };
            }
            return View(bd);
        }

        [Authorize]
        [HttpPost]
        public RedirectResult CreateProduct(string name, int basket)
        {
            if (string.IsNullOrWhiteSpace(name)) return Redirect(Url.Action("Index"));
            Basket b;
            using (var db = new Context())
            {
                var person = Person.Current(db);
                b = db.Baskets.Find(basket);
                var response = Basket.IsOwner(b, Person.Current(db));
                if (!response.Successfull) return Redirect(Url.Action("Index", "Error", new { id = response.Error }));
                db.BasketProducts.Add(new BasketProduct{  });
                db.SaveChanges();
                b = db.Baskets.First(x => x.Name == name);
            }

            return Redirect(Url.Action("View", new { id = b.Id }));
        }
    }
}