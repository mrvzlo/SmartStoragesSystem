using SmartKitchen.Domain.CreationModels;
using SmartKitchen.Domain.IServices;
using SmartKitchen.Web.Helpers;
using System;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using SmartKitchen.Domain.Enums;

namespace SmartKitchen.Web.Controllers
{
    [Authorize]
    public class StorageController : BaseController
    {
        private readonly IStorageService _storageService;
        private readonly IStorageTypeService _storageTypeService;
        private readonly ICellService _cellService;
        private readonly IBasketService _basketService;

        public StorageController(IStorageService storageService, ICellService cellService, IStorageTypeService storageTypeService, IBasketService basketService)
        {
            _storageService = storageService;
            _cellService = cellService;
            _storageTypeService = storageTypeService;
            _basketService = basketService;
        }

        #region CRD

        /// <summary>
        /// Open page with list of storages
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            var storages = _storageService.GetStoragesByOwnerEmail(CurrentUser()).ToList();
            return View(storages);
        }

        /// <summary>
        /// Delete storage and reload page with list of storages
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Delete(int id)
        {
            _storageService.DeleteStorageById(id, CurrentUser());
            return Redirect(Url.Action("Index"));
        }

        /// <summary>
        /// Open storage details
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult View(int id)
        {
            var description = _storageService.GetStorageById(id, HttpContext.User.Identity.Name);
            if (description == null) return Redirect(Url.Action("Index"));
            if (TempData.ContainsKey("error"))
                ModelState.AddModelError("Name", TempData["error"].ToString());
            var selectList = _basketService.GetBasketsByOwnerEmail(CurrentUser()).Where(x => !x.Closed).Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.Name
            }).ToList();
            selectList.Add(new SelectListItem { Value = "0", Text = "New" });
            ViewBag.SelectList = selectList;
            ViewBag.Weight = CookieHelper.GetCookie(HttpContext, Cookie.Weight).Value;
            return View(description);
        }

        /// <summary>
        /// Storage creation page with available storage types
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            var query = _storageTypeService.GetAllStorageTypes();
            return View(query.ToList());
        }

        /// <summary>
        /// Perform storage creation and open it or reload page
        /// </summary>
        /// <param name="storage"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(StorageCreationModel storage)
        {
            if (ModelState.IsValid)
            {
                var response = _storageService.AddStorage(storage, CurrentUser());
                if (response.Successful()) return Redirect(Url.Action("View", "Storage", new { id = response.AddedId }));
                AddModelStateErrors(response);
            }

            ViewBag.Selected = storage.TypeId;
            var query = _storageTypeService.GetAllStorageTypes();
            return View(query.ToList());
        }

        /// <summary>
        /// Rename storage and return successfulness of action
        /// </summary>
        /// <param name="name"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public bool UpdateName(string name, int id)
        {
            var model = new NameCreationModel(name);
            return ModelState.IsValid && _storageService.UpdateStorageName(model, id, CurrentUser());
        }

        #endregion

        #region Types CD

        /// <summary>
        /// Storage type creation page with available types
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        public ActionResult CreateType()
        {
            var query = _storageTypeService.GetAllStorageTypes();
            return View(query.ToList());
        }

        /// <summary>
        /// Update existing of create new storage type and reload page or form
        /// If image is applied check correctness and add in icons folder
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public JsonResult CreateType(StorageTypeCreationModel model)
        {
            ViewBag.Selected = model;
            if (System.Web.HttpContext.Current.Request.Files.Count > 0)
            {
                var file = System.Web.HttpContext.Current.Request.Files[0];
                if (FileHelper.IconIsNotValid(file))
                    ModelState.AddModelError("Icon", "Select a PNG image smaller than 1MB");
            }

            if (!ModelState.IsValid)
                return Json(new { success = false, formHTML = this.RenderPartialViewToString("_CreateTypeForm", model) });
            var response = _storageTypeService.AddOrUpdateStorageType(model);

            if (System.Web.HttpContext.Current.Request.Files.Count > 0)
            {
                var file = System.Web.HttpContext.Current.Request.Files[0];
                if (file.ContentLength > 0)
                    file.SaveAs(Server.MapPath("~/Content/icons/" + response.AddedId + ".png"));
            }


            if (response.Successful() && ModelState.IsValid)
                return Json(new { success = true, url = Url.Action("CreateType", "Storage") });
            AddModelStateErrors(response);
            return Json(new { success = false, formHTML = this.RenderPartialViewToString("_CreateTypeForm", model) });
        }

        /// <summary>
        /// Remove storage type and its image and return successfulness of action
        /// </summary>
        /// <param name="fromId"></param>
        /// <param name="toId"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public bool RemoveType(int fromId, int toId)
        {
            var result = _storageTypeService.ReplaceStorageType(fromId, toId);
            if (result) FileHelper.RemoveImage(Server.MapPath("~/Content/icons/" + fromId + ".png"));
            return result;
        }

        #endregion

        #region Cell

        /// <summary>
        /// Create cell and reload page with its storage
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CreateCell(CellCreationModel model)
        {
            var response = _cellService.AddCell(model, CurrentUser());
            if (response.Successful())
            {
                AddModelStateErrors(response);
                TempData["error"] = "This name is already taken";
            }
            return Redirect(Url.Action("View", new { id = response.AddedGroupId }));
        }

        /// <summary>
        /// Update cell amount on page
        /// </summary>
        /// <param name="cell"></param>
        /// <param name="amount"></param>
        [HttpPost]
        public void SetAmount(int cell, int amount) =>
            _cellService.UpdateCellAmount(cell, amount, CurrentUser());

        /// <summary>
        /// Remove cell and return successfulness of action
        /// </summary>
        /// <param name="cellId"></param>
        /// <returns></returns>
        [HttpPost]
        public bool Remove(int cellId) =>
            _cellService.DeleteCellById(cellId, CurrentUser()).Successful();

        /// <summary>
        /// Parse datetime and update cell best before on page
        /// </summary>
        /// <param name="cell"></param>
        /// <param name="dateStr"></param>
        [HttpPost]
        public void DateUpdate(int cell, string dateStr)
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

            _cellService.UpdateCellBestBefore(cell, newDate, CurrentUser());
        }

        /// <summary>
        /// Partial page with list of cells
        /// </summary>
        /// <param name="storage"></param>
        /// <returns></returns>
        public PartialViewResult ShowAllCells(int storage)
        {
            ViewBag.Weight = CookieHelper.GetCookie(HttpContext, Cookie.Weight).Value;
            var query = _cellService.GetCellsOfStorage(storage, CurrentUser());
            return PartialView("_ShowAllCells", query);
        }

        #endregion
    }
}