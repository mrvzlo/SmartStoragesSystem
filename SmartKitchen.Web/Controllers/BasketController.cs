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
        private readonly IStorageService _storageService;

        public BasketController(IBasketService basketService, IStorageService storageService)
        {
            _basketService = basketService;
            _storageService = storageService;
        }

        public string CurrentUser() => HttpContext.User.Identity.Name;

        #region Basket
        public ActionResult Index()
        {
            var list = _basketService.GetBasketsByOwnerEmail(CurrentUser());
            if (TempData.ContainsKey("error")) ModelState.AddModelError("Name", TempData["error"].ToString());
            return View(list);
        }

        [HttpPost]
        public RedirectResult Create(NameCreationModel name)
        {
            var response = _basketService.CreateBasket(name, CurrentUser());
            if (response.Successful())
            {
                TempData["error"] = "This name is already taken";
                return Redirect(Url.Action("Index"));
            }

            return Redirect(Url.Action("View", new { id = response.Id }));
        }

        public ActionResult View(int id)
        {
            var basket = _basketService.GetBasketWithProductsById(id, CurrentUser());
            if (basket == null) return Redirect(Url.Action("Index"));
            return View(basket);
        }

        public PartialViewResult GetMyStorages()
        {
            var list = _storageService.GetStoragesWithDescriptionByOwnerEmail(CurrentUser());
            return PartialView("_StorageSelect", list);
        }

        public ActionResult Lock(int id)
        {
            _basketService.LockBasket(id, HttpContext.User.Identity.Name);
            var description = _basketService.GetBasketById(id, CurrentUser());
            if (description == null) return Redirect(Url.Action("Index"));
            return PartialView("_Description", description);
        }

        public bool Remove(int id)
        {
            return _basketService.DeleteBasket(id, CurrentUser());
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