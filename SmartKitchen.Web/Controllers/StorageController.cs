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
            CellDescription description = new CellDescription();
            using (var db = new Context())
            {
                Cell cellold = db.Cells.Find(cell);
                var storage = db.Storages.Find(cellold.Storage);
                var response = Storage.IsOwner(storage, Person.Current(db));
                if (!response.Successfull) return Redirect(Url.Action("Index", "Error", new { id = response.Error }));
                if (cellold.Amount == Amount.None || amount == (int)Amount.None) cellold.BestBefore = null;
                if (amount > (int)Amount.Plenty) cellold.Amount = Amount.Plenty;
                else if (amount < (int)Amount.None) cellold.Amount = Amount.None;
                else cellold.Amount = (Amount)amount;
                db.SaveChanges();
                description = new CellDescription(db, cell);
            }

            return PartialView("_Description", description);
        }

        public void Remove(int cellId)
        {
            using (var db = new Context())
            {
                var cell = db.Cells.Find(cellId);
                if (cell == null) return;
                var storage = db.Storages.Find(cell.Storage);
                var response = Storage.IsOwner(storage, Person.Current(db));
                if (!response.Successfull) return;
                db.Cells.Remove(cell);
                db.SaveChanges();
            }
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
            CellDescription description = new CellDescription();
            using (var db = new Context())
            {
                Cell cellold = db.Cells.Find(cell);
                var storage = db.Storages.Find(cellold.Storage);
                var response = Storage.IsOwner(storage, Person.Current(db));
                if (!response.Successfull) return Redirect(Url.Action("Index", "Error", new { id = response.Error }));
                if (cellold != null)
                {
                    if (newDate != null) cellold.BestBefore = newDate;
                    db.SaveChanges();
                    description = new CellDescription(db, cell);
                }
            }

            return PartialView("_Description", description);
        }

        public ActionResult ShowAllCells(int storage, int order)
        {
            var cells = new List<CellDescription>();
            using (var db = new Context())
            {
                foreach (var cell in db.Cells.Where(x => x.Storage == storage).Select(x => x.Id).ToList())
                    cells.Add(new CellDescription(db, cell));
                cells = GetCellOrder(order, cells);
            }
            return PartialView("_ShowAllCells", cells);
        }

        #endregion
    }
}