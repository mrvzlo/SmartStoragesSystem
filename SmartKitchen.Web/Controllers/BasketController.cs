using System;
using System.Collections.Generic;
using System.Globalization;
using SmartKitchen.Domain.CreationModels;
using SmartKitchen.Domain.IServices;
using System.Linq;
using System.Web.Mvc;
using SmartKitchen.Domain.Enums;
using SmartKitchen.Web.Helpers;

namespace SmartKitchen.Web.Controllers
{
    [Authorize]
    public class BasketController : BaseController
    {
        private readonly IBasketService _basketService;
        private readonly IBasketProductService _basketProductService;
        private readonly IStorageService _storageService;

        public BasketController(IBasketService basketService, IStorageService storageService, IBasketProductService basketProductService)
        {
            _basketService = basketService;
            _basketProductService = basketProductService;
            _storageService = storageService;
        }

        #region Basket
        public ActionResult Index()
        {
            var query = _basketService.GetBasketsByOwnerEmail(CurrentUser());
            if (TempData.ContainsKey("error")) ModelState.AddModelError("Name", TempData["error"].ToString());
            ViewBag.Currency = CookieHelper.GetCurrency(HttpContext);
            return View(query.ToList());
        }

        [HttpPost]
        public RedirectResult Create(NameCreationModel model)
        {
            var response = _basketService.AddBasket(model, CurrentUser());
            if (!response.Successful())
            {
                AddModelStateErrors(response);
                TempData["error"] = "This name is already taken";
                return Redirect(Url.Action("Index"));
            }

            return Redirect(Url.Action("View", new { id = response.AddedId }));
        }

        [HttpPost]
        public JsonResult AddListToBasket(int basket, int storage, string name, List<int> cells)
        {
            if (basket == 0)
            {
                var model = new NameCreationModel(name);
                if (!ModelState.IsValid) return Json(new { success = false, error = "Name is not valid"}); 
                var response = _basketService.AddBasket(model, CurrentUser());
                if (!response.Successful())
                {
                    AddModelStateErrors(response);
                    return Json(new { success = false, error = "This name is already taken" }); 
                }

                basket = response.AddedId;
            }

            _basketProductService.AddBasketProductList(basket, storage, CurrentUser(), cells);
            return Json(new { success = true, url = Url.Action("View","Basket", new {id = basket}) });
        }

        public ActionResult View(int id)
        {
            ViewBag.Currency = CookieHelper.GetCurrency(HttpContext);
            ViewBag.Weight = CookieHelper.GetCookie(HttpContext, Cookie.Weight).Value;
            var basket = _basketService.GetBasketById(id, CurrentUser());
            if (basket == null) return Redirect(Url.Action("Index"));
            return View(basket);
        }

        [HttpPost]
        public bool Remove(int id)
        {
            return _basketService.DeleteBasket(id, CurrentUser());
        }

        [HttpPost]
        public bool UpdateName(string name, int id)
        {
            var model = new NameCreationModel(name);
            return ModelState.IsValid && _basketService.UpdateBasketName(model, id, CurrentUser());
        }

        [HttpPost]
        public ActionResult FinishAndClose(int id)
        {
            _basketService.FinishAndCloseBasket(id, CurrentUser());
            return Redirect(Url.Action("Index", new {id}));
        }
        #endregion

        #region BasketProduct
        public PartialViewResult CreateProduct(int basket)
        {
            var basketProduct = new BasketProductCreationModel
            {
                Basket = basket
            };
            ViewBag.SelectList = _storageService.GetStoragesByOwnerEmail(CurrentUser()).Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.Name
            }).ToList();
            return PartialView("_AddNewProduct", basketProduct);
        }

        [Authorize]
        [HttpPost]
        public RedirectResult CreateProduct(BasketProductCreationModel model)
        {
            var response = _basketProductService.AddBasketProductByModel(model, CurrentUser());
            if (!response.Successful())
                AddModelStateErrors(response);
            return Redirect(Url.Action("View", new { id = response.AddedGroupId }));
        }

        public PartialViewResult BasketProductGrid(int id)
        {
            var basketProductList = _basketProductService.GetBasketProductDisplayModelByBasket(id, CurrentUser());
            ViewBag.Currency = CookieHelper.GetCurrency(HttpContext);
            ViewBag.Weight = CookieHelper.GetCookie(HttpContext, Cookie.Weight).Value;
            return PartialView("_ProductGrid", basketProductList);
        }

        [HttpPost]
        public bool MarkProductBought(int id, bool status) => 
            _basketProductService.MarkProductBought(id, status, CurrentUser()).Successful();

        [HttpPost]
        public bool RemoveBasketProduct(int id) =>
            _basketProductService.DeleteBasketProductByIdAndEmail(id, CurrentUser()).Successful();

        [HttpPost]
        public void SetAmount(int id, int amount) =>
            _basketProductService.UpdateProductAmount(id, amount, CurrentUser());

        [HttpPost]
        public void SetPrice(int id, decimal price) =>
            _basketProductService.UpdateProductPrice(id, price, CurrentUser());

        [HttpPost]
        public void DateUpdate(int id, string dateStr)
        {
            DateTime? newDate;
            try
            {
                newDate = DateTime.ParseExact(dateStr, "d/M/yyyy", CultureInfo.InvariantCulture);
            }
            catch (Exception)
            {
                newDate = null;
            }

            _basketProductService.UpdateProductBestBefore(id, newDate, CurrentUser());
        }
        #endregion
    }
}