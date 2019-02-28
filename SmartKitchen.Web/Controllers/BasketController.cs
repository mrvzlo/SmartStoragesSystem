using System.Linq;
using System.Web.Mvc;
using SmartKitchen.Domain.CreationModels;
using SmartKitchen.Domain.IServices;

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
            return View(query.ToList());
        }

        [HttpPost]
        public RedirectResult Create(NameCreationModel model)
        {
            var response = _basketService.AddBasket(model, CurrentUser());
            if (response.Successful())
            {
                AddModelStateErrors(response);
                TempData["error"] = "This name is already taken";
                return Redirect(Url.Action("Index"));
            }

            return Redirect(Url.Action("View", new { id = response.AddedId }));
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
            var locked = _basketService.LockBasket(id, CurrentUser());
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
            var basketProduct = new BasketProductCreationModel
            {
                Basket = basket
            };
            return PartialView("_AddNewProduct", basketProduct);
        }

        [Authorize]
        [HttpPost]
        public RedirectResult CreateProduct(BasketProductCreationModel model)
        {
            var response = _basketProductService.AddBasketProduct(model, CurrentUser());
            if (!response.Successful())
            {
                AddModelStateErrors(response);
                return Redirect(Url.Action("Index"));
            }
            return Redirect(Url.Action("View", new { id = response.AddedGroupId }));
        }

        public ActionResult Description(int id)
        {
            var basketProduct = _basketProductService.GetBasketProductDisplayModelById(id, CurrentUser());
            if (basketProduct == null) return Redirect(Url.Action("Index"));
            return PartialView("_ProductDescription", basketProduct);
        }
        #endregion
    }
}