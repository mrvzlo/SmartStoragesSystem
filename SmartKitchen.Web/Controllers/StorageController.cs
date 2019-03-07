using System;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using SmartKitchen.Domain.CreationModels;
using SmartKitchen.Domain.Enums;
using SmartKitchen.Domain.IServices;
using SmartKitchen.Web.Helpers;

namespace SmartKitchen.Web.Controllers
{
    [Authorize]
    public class StorageController : BaseController
    {
        private readonly IStorageService _storageService;
        private readonly IStorageTypeService _storageTypeService;
        private readonly ICellService _cellService;

        public StorageController(IStorageService storageService, ICellService cellService, IStorageTypeService storageTypeService)
        {
            _storageService = storageService;
            _cellService = cellService;
            _storageTypeService = storageTypeService;
        }

        #region CRD

        public ActionResult Index()
        {
            var storages = _storageService.GetStoragesWithDescriptionByOwnerEmail(CurrentUser()).ToList();
            return View(storages);
        }

        public ActionResult Delete(int id)
        {
            _storageService.DeleteStorageById(id);
            return Redirect(Url.Action("Index"));
        }

        public ActionResult View(int id)
        {
            var description = _storageService.GetStorageDescriptionById(id, HttpContext.User.Identity.Name);
            if (description == null) return Redirect(Url.Action("Index"));
            if (TempData.ContainsKey("error"))
                ModelState.AddModelError("Name", TempData["error"].ToString());
            return View(description);
        }

        public ActionResult Create()
        {
            var query = _storageTypeService.GetAllStorageTypes();
            return View(query.ToList());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(StorageCreationModel storage)
        {
            if (ModelState.IsValid)
            {
                var response = _storageService.AddStorage(storage, CurrentUser());
                if (response.Successful()) return Redirect(Url.Action("View", "Storage", new {id = response.AddedId}));
                AddModelStateErrors(response);
            }

            ViewBag.Selected = storage.TypeId;
            var query = _storageTypeService.GetAllStorageTypes();
            return View(query.ToList());
        }

        #endregion

        #region Types CD

        [Authorize(Roles = "Admin")]
        public ActionResult CreateType()
        {
            var query = _storageTypeService.GetAllStorageTypes();
            return View(query.ToList());
        }

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
                    file.SaveAs(Server.MapPath("~/Content/images/" + response.AddedId + ".png"));
            }


            if (response.Successful() && ModelState.IsValid)
                return Json(new { success = true, url = Url.Action("CreateType", "Storage" )});
            AddModelStateErrors(response);
            return Json(new { success = false, formHTML = this.RenderPartialViewToString("_CreateTypeForm", model) });
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public bool RemoveType(int fromId, int toId)
        {
            return _storageTypeService.ReplaceStorageType(fromId, toId);
        }

        #endregion

        #region Cell

        [HttpPost]
        public void SetAmount(int cell, decimal amount)
        {
            _cellService.UpdateCellAmount(cell, amount, CurrentUser());
        }

        [HttpPost]
        public bool Remove(int cellId)
        {
            var response = _cellService.DeleteCellById(cellId, CurrentUser());
            if (response.Successful()) FileHelper.RemoveImage(Server.MapPath("~/Content/images/" + cellId + ".png"));
            return response.Successful();
        }

        [HttpPost]
        public void DateUpdate(int cell, string dateStr)
        {
            DateTime? newDate;
            try
            {
                newDate = DateTime.ParseExact(dateStr, "d/M/yyyy", CultureInfo.InvariantCulture);
            }
            catch (Exception e)
            {
                newDate = null;
            }

            _cellService.UpdateCellBestBefore(cell, newDate, CurrentUser());
        }

        public PartialViewResult ShowAllCells(int storage)
        {
            var query = _cellService.GetCellsOfStorage(storage, CurrentUser());
            return PartialView("_ShowAllCells", query);
        }

        #endregion
    }
}