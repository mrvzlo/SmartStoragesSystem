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

        /// <summary>
        /// Page with list of all baskets
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            var query = _basketService.GetBasketsByOwnerEmail(CurrentUser());
            if (TempData.ContainsKey("error")) ModelState.AddModelError("Name", TempData["error"].ToString());
            ViewBag.Currency = CookieHelper.GetCurrency(HttpContext);
            return View(query.ToList());
        }

        /// <summary>
        /// Add new basket and open it else reload page with list of baskets
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Add list of storage product to basket and open it if basket is created or can be created else reload form
        /// </summary>
        /// <param name="basket"></param>
        /// <param name="storage"></param>
        /// <param name="name"></param>
        /// <param name="cells"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Open basket details page
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult View(int id)
        {
            ViewBag.Currency = CookieHelper.GetCurrency(HttpContext);
            ViewBag.Weight = CookieHelper.GetCookie(HttpContext, Cookie.Weight).Value;
            var basket = _basketService.GetBasketById(id, CurrentUser());
            if (basket == null) return Redirect(Url.Action("Index"));
            return View(basket);
        }

        /// <summary>
        /// Remove basket and return successfulness of action
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public bool Remove(int id) => _basketService.DeleteBasket(id, CurrentUser());

        /// <summary>
        /// Rename basket and return successfulness of action
        /// </summary>
        /// <param name="name"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public bool UpdateName(string name, int id)
        {
            var model = new NameCreationModel(name);
            return ModelState.IsValid && _basketService.UpdateBasketName(model, id, CurrentUser());
        }

        /// <summary>
        /// Close basket and reload page with list of baskets
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult FinishAndClose(int id)
        {
            _basketService.FinishAndCloseBasket(id, CurrentUser());
            return Redirect(Url.Action("Index", new {id}));
        }

        /// <summary>
        /// Reopen basket and reload page with list of baskets
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Reopen(int id)
        {
            _basketService.ReopenBasket(id, CurrentUser());
            return Redirect(Url.Action("Index", new { id }));
        }
        #endregion

        #region BasketProduct

        /// <summary>
        /// Partial page ofr basket product creation
        /// </summary>
        /// <param name="basket"></param>
        /// <returns></returns>
        public PartialViewResult CreateProduct(int basket)
        {
            var basketProduct = new BasketProductCreationModel { Basket = basket };
            ViewBag.SelectList = _storageService.GetStoragesByOwnerEmail(CurrentUser()).Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.Name
            }).ToList();
            return PartialView("_AddNewProduct", basketProduct);
        }

        /// <summary>
        /// Add product to basket and reload basket page
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        public RedirectResult CreateProduct(BasketProductCreationModel model)
        {
            var response = _basketProductService.AddBasketProductByModel(model, CurrentUser());
            if (!response.Successful()) AddModelStateErrors(response);
            return Redirect(Url.Action("View", new { id = response.AddedGroupId }));
        }

        /// <summary>
        /// Get list of products in basket
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public PartialViewResult BasketProductGrid(int id)
        {
            var basketProductList = _basketProductService.GetBasketProductDisplayModelByBasket(id, CurrentUser());
            ViewBag.Currency = CookieHelper.GetCurrency(HttpContext);
            ViewBag.Weight = CookieHelper.GetCookie(HttpContext, Cookie.Weight).Value;
            return PartialView("_ProductGrid", basketProductList);
        }

        /// <summary>
        /// Mark basket product and return successfulness of action
        /// </summary>
        /// <param name="id"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        [HttpPost]
        public bool MarkProductBought(int id, bool status) => 
            _basketProductService.MarkProductBought(id, status, CurrentUser()).Successful();

        /// <summary>
        /// Remove basket product and return successfulness of action
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public bool RemoveBasketProduct(int id) =>
            _basketProductService.DeleteBasketProductByIdAndEmail(id, CurrentUser()).Successful();

        /// <summary>
        /// Update basket product amount and return successfulness of action
        /// </summary>
        /// <param name="id"></param>
        /// <param name="amount"></param>
        [HttpPost]
        public void SetAmount(int id, int amount) =>
            _basketProductService.UpdateProductAmount(id, amount, CurrentUser());

        /// <summary>
        /// Update basket product price and return successfulness of action
        /// </summary>
        /// <param name="id"></param>
        /// <param name="price"></param>
        [HttpPost]
        public void SetPrice(int id, decimal price) =>
            _basketProductService.UpdateProductPrice(id, price, CurrentUser());

        /// <summary>
        /// Convert datetime form string to DateTime
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dateStr"></param>
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