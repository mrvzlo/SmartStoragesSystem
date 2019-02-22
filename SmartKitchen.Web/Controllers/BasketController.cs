using SmartKitchen.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.UI.WebControls.WebParts;
using SmartKitchen.Enums;

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
                    var products = db.BasketProducts.Where(x => x.Basket == b.Id).OrderByDescending(x => x.Bought).Select(x => x.Id).ToList();
                    var bought = db.BasketProducts.Count(x => x.Basket == b.Id && x.Bought);
                    bdlist.Add(new BasketDescription { Basket = b, Products = products, BoughtProducts = bought });
                }
            }
            if (TempData.ContainsKey("error")) ModelState.AddModelError("Name", TempData["error"].ToString());
            return View(bdlist.OrderByDescending(x => x.Basket.CreationDate).ToList());
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

                db.Baskets.Add(new Basket { Name = name, CreationDate = DateTime.Now, Owner = person.Id });
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
                var response = Basket.IsOwner(basket, person);
                if (!response.Successfull) return Redirect(Url.Action("Index", "Error", new { id = response.Error }));
                var products = db.BasketProducts.Where(x => x.Basket == id).OrderByDescending(x => x.Bought).Select(x => x.Id).ToList();
                int bought = db.BasketProducts.Count(x => x.Basket == id && x.Bought);
                bd = new BasketDescription { Basket = basket, Products = products, BoughtProducts = bought };
            }
            return View(bd);
        }

        public PartialViewResult GetMyStorages()
        {
            var list = new List<Storage>();
            using (var db = new Context())
            {
                list = Storage.GetMyStorages(Person.Current(db).Id, db);
            }

            return PartialView("_StorageSelect", list);
        }

        public PartialViewResult CreateProduct()
        {
            return PartialView("_AddNewProduct", new BasketProductCreation());
        }

        [Authorize]
        [HttpPost]
        public RedirectResult CreateProduct(BasketProductCreation model)
        {
            if (string.IsNullOrWhiteSpace(model.Name)) return Redirect(Url.Action("Index"));
            using (var db = new Context())
            {
                var person = Person.Current(db);
                var basket = db.Baskets.Find(model.Basket);
                var storage = db.Storages.Find(model.Storage);
                var response = Basket.IsOwner(basket, person);
                if (!response.Successfull) return Redirect(Url.Action("Index", "Error", new { id = response.Error }));
                response = Storage.IsOwner(storage, person);
                if (!response.Successfull) return Redirect(Url.Action("Index", "Error", new { id = response.Error }));
                var cell = GetOrCreateAndGet(model.Name, storage.Id, db);
                if (cell != null)
                {
                    db.BasketProducts.Add(new BasketProduct {Basket = basket.Id, Cell = cell.Id, BestBefore = null});
                    db.SaveChanges();
                }
            }

            return Redirect(Url.Action("View", new { id = model.Basket }));
        }

        private Cell GetOrCreateAndGet(string name, int storage, Context db)
        {
            var product = Product.GetByName(name, db);
            if (product == null)
            {
                db.Products.Add(new Product { Category = 1, Name = name });
                db.SaveChanges();
                product = Product.GetByName(name,db);
            }

            var result = db.Cells.FirstOrDefault(x => x.Storage == storage && x.Product == product.Id);
            if (result != null) return result;
            db.Cells.Add(new Cell {Amount = Amount.None, BestBefore = null, Product = product.Id, Storage = storage});
            db.SaveChanges();
            return db.Cells.FirstOrDefault(x => x.Storage == storage && x.Product == product.Id);
        }

        public ActionResult Lock(int id)
        {
            var bd = new BasketDescription();
            using (var db = new Context())
            {
                var person = Person.Current(db);
                var basket = db.Baskets.Find(id);
                var response = Basket.IsOwner(basket, person);
                if (!response.Successfull) return Redirect(Url.Action("Index", "Error", new { id = response.Error }));
                basket.Closed = true;
                db.SaveChanges();
                bd.Basket = basket;
                bd.Products = db.BasketProducts.Where(x => x.Basket == basket.Id).OrderByDescending(x => x.Bought).Select(x => x.Id).ToList();
                bd.BoughtProducts = db.BasketProducts.Count(x => x.Basket == basket.Id && x.Bought);
            }
            return PartialView("_Description", bd);
        }

        public bool Remove(int id)
        {
            using (var db = new Context())
            {
                var person = Person.Current(db);
                var basket = db.Baskets.Find(id);
                var response = Basket.IsOwner(basket, person);
                if (!response.Successfull) return false;
                db.Baskets.Remove(basket);
                db.SaveChanges();
            }
            return true;
        }
    }
}