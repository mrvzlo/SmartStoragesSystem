using SmartKitchen.Domain.DisplayModels;
using SmartKitchen.Domain.Enitities;
using SmartKitchen.Domain.Enums;
using SmartKitchen.Domain.IServices;
using SmartKitchen.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SmartKitchen.Domain.CreationModels;
using SmartKitchen.Domain.IRepositories;
using SmartKitchen.Helpers;

namespace SmartKitchen.Controllers
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
            var response = _storageService.AddStorage(storage, CurrentUser());
            if (!response.Successful())
            {
                AddModelStateErrors(response);
                ViewBag.Selected = storage.TypeId;
                var query = _storageTypeService.GetAllStorageTypes();
                return View(query.ToList());
            }
            return Redirect(Url.Action("Index"));
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
            var file = System.Web.HttpContext.Current.Request.Files[0];
            var response = _storageTypeService.AddOrUpdateStorageType(model);
            if (FileHelper.IconIsNotValid(file)) ModelState.AddModelError("Icon", "Select a PNG image smaller than 1MB");
            if (response.Successful() && ModelState.IsValid) return Redirect(Url.Action("CreateType"));
            if (file.ContentLength > 0) FileHelper.SaveImage(file, Server.MapPath("~/Content/images/" + model.Id + ".png"));
            var query = _storageTypeService.GetAllStorageTypes();
            return View(query.ToList());
        }

        [Authorize(Roles = "Admin")]
        public ActionResult RemoveType(int fromId, int toId)
        {
            _storageTypeService.ReplaceType(fromId, toId);
            return Redirect(Url.Action("Index"));
        }

        #endregion

        #region Cell
        public PartialViewResult CellDescription(int cell)
        {
            var description = _cellService.GetCellDisplayModelById(cell, CurrentUser());
            return PartialView("_Description", description);
        }

        public ActionResult SetAmount(int cell, int amount)
        {
            _cellService.UpdateCellAmount(cell, amount, CurrentUser());
            var description = _cellService.GetCellDisplayModelById(cell, CurrentUser());
            return PartialView("_Description", description);
        }

        public bool Remove(int cellId)
        {
            var response = _cellService.DeleteCellById(cellId, CurrentUser());
            return response.Successful();
        }

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

            _cellService.UpdateCellBestBefore(cell, newDate, CurrentUser());
            var description = _cellService.GetCellDisplayModelById(cell,CurrentUser());
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