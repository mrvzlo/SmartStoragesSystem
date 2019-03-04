using System;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using SmartKitchen.Domain.CreationModels;
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
        public ActionResult CreateType(string s)
        {
            var query = _storageTypeService.GetAllStorageTypes();
            return View(query.ToList());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult CreateType(StorageTypeCreationModel model)
        {
            if (!ModelState.IsValid) return Redirect(Url.Action("CreateType"));
            var file = System.Web.HttpContext.Current.Request.Files[0];
            var response = _storageTypeService.AddOrUpdateStorageType(model);
            if (FileHelper.IconIsNotValid(file)) ModelState.AddModelError("Icon", "Select a PNG image smaller than 1MB");
            if (file.ContentLength > 0) FileHelper.SaveImage(file, Server.MapPath("~/Content/images/" + response.AddedId+ ".png"));
            if (response.Successful() && ModelState.IsValid) return Redirect(Url.Action("CreateType"));
            var query = _storageTypeService.GetAllStorageTypes();
            return View(query.ToList());
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public bool RemoveType(int fromId, int toId)
        {
            return _storageTypeService.ReplaceType(fromId, toId);
        }

        #endregion

        #region Cell

        [HttpPost]
        public ActionResult SetAmount(int cell, int amount)
        {
            var description = _cellService.UpdateCellAmount(cell, amount, CurrentUser());
            return PartialView("_Description", description);
        }

        [HttpPost]
        public bool Remove(int cellId)
        {
            var response = _cellService.DeleteCellById(cellId, CurrentUser());
            if (response.Successful()) FileHelper.RemoveImage(Server.MapPath("~/Content/images/" + cellId + ".png"));
            return response.Successful();
        }

        [HttpPost]
        public ActionResult DateUpdate(int cell, string dateStr)
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

            var description = _cellService.UpdateCellBestBefore(cell, newDate, CurrentUser());
            return PartialView("_Description", description);
        }

        public ActionResult ShowAllCells(int storage)
        {
            var list = _cellService.GetCellsOfStorage(storage, CurrentUser());
            return PartialView("_ShowAllCells", list);
        }

        #endregion
    }
}