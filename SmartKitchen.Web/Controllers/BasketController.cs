using SmartKitchen.Domain.Enitities;
using SmartKitchen.Domain.Enums;
using SmartKitchen.Domain.IServices;
using SmartKitchen.Models;
using System;
using System.Collections.Generic;
using SmartKitchen.Domain.CreationModels;
using System.Web.Mvc;

namespace SmartKitchen.Controllers
{
    [Authorize]
    public class BasketController : Controller
    {
        private readonly IBasketService _basketService;

        public BasketController(IBasketService basketService)
        {
            _basketService = basketService;
        }

        #region Basket
        public ActionResult Index()
        {
            var list = _basketService.GetBasketsWithDescriptionByOwnerEmail(HttpContext.User.Identity.Name);
            if (TempData.ContainsKey("error")) ModelState.AddModelError("Name", TempData["error"].ToString());
            return View(list);
        }

        [HttpPost]
        public RedirectResult Create(NameCreationModel name)
        {
            var response = _basketService.CreateBasket(name, HttpContext.User.Identity.Name);
            if (response.IsSuccessful)
            {
                TempData["error"] = "This name is already taken";
                return Redirect(Url.Action("Index"));
            }

            return Redirect(Url.Action("View", new { id = response.Id }));
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
        #endregion

        #region BasketProduct
        public PartialViewResult CreateProduct(int basket)
        {
            ViewBag.Basket = basket;
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
                    db.BasketProducts.Add(new BasketProduct { Basket = basket.Id, Cell = cell.Id, BestBefore = null });
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
                product = Product.GetByName(name, db);
            }

            var result = db.Cells.FirstOrDefault(x => x.Storage == storage && x.Product == product.Id);
            if (result != null) return result;
            db.Cells.Add(new Cell { Amount = Amount.None, BestBefore = null, Product = product.Id, Storage = storage });
            db.SaveChanges();
            return db.Cells.FirstOrDefault(x => x.Storage == storage && x.Product == product.Id);
        }

        public ActionResult Description(int id)
        {
            var redirect = Redirect(Url.Action("Index"));
            var bpd = new BasketProductDescription();
            using (var db = new Context())
            {
                var bp = db.BasketProducts.Find(id);
                if (bp == null) return redirect;
                var basket = db.Baskets.Find(bp.Basket);
                var response = Basket.IsOwner(basket, Person.Current(db));
                if (!response.Successfull) return redirect;
                bpd.Product = bp;
                var cell = db.Cells.Find(bp.Cell);
                if (cell == null) return redirect;
                var product = db.Products.Find(cell.Product);
                if (product == null) return redirect;
                bpd.Name = product.Name;
            }

            return PartialView("_ProductDescription", bpd);
        }
        #endregion
    }
}