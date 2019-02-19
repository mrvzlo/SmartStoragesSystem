using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using SmartKitchen.Models;

namespace SmartKitchen.Controllers
{
	[Authorize]
	public class StorageController : Controller
    {
        #region CRD

        public ActionResult Index()
        {
			List<StorageDescription> storages = new List<StorageDescription>();
	        using (var db = new Context())
	        {
		        var person = Person.Current(db);
		        if (person == null) return Redirect(Url.Action("Index", "Home"));
		        var s = db.Storages.Where(x => x.Owner == person.Id).ToList();
		        foreach (var storage in s)
		        {
					storages.Add(new StorageDescription(storage,db,1));
		        }
	        }
            return View(storages);
        }
		
		public ActionResult Delete(int id)
	    {
		    using (var db = new Context())
		    {
			    var person = Person.Current(db);
			    var storage = db.Storages.Find(id);
			    if (storage !=null && storage.Owner == person.Id)
			    {
				    db.Storages.Remove(storage);
				    db.Cells.RemoveRange(db.Cells.Where(x => x.Storage == id));
				    db.SaveChanges();
			    }
		    }
		    return Redirect(Url.Action("Index"));
	    }

	    public ActionResult View(int id, int order = 1)
	    {
		    var content = new StorageDescription();
			using (var db = new Context())
			{
				var storage = db.Storages.Find(id);
				content = new StorageDescription(storage, db, order);
			}

			if (content.Type == null) return Redirect(Url.Action("Index"));
            ViewBag.Order = order;
            return View(content);
	    }

	    public ActionResult Create()
	    {
		    return View(StorageType.GetAll());
	    }

        [HttpPost]
	    [ValidateAntiForgeryToken]
        public ActionResult Create(Storage storage)
	    {
		    if (string.IsNullOrEmpty(storage.Name)) ModelState.AddModelError("Name", "Name is required");
		    Person person;
		    using (var db = new Context())
		    {
			    person = Person.Current(db);
			    if (db.Storages.FirstOrDefault(x => x.Owner == person.Id && x.Name == storage.Name) != null)
				    ModelState.AddModelError("Name", "This name is already taken");
			    if (db.StorageTypes.Find(storage.Type) == null) ModelState.AddModelError("Type", "Unknown type");
		    }
		    if (ModelState.IsValid)
		    {
			    using (var db = new Context())
			    {
				    storage.Owner = person.Id;
				    db.Storages.Add(storage);
				    db.SaveChanges();
				    return Redirect(Url.Action("Index"));
			    }
		    }

		    ViewBag.Selected = storage.Type;
			return View(StorageType.GetAll());
	    }

        #endregion

        #region Types CD

        [Authorize(Roles = "admin")]
        public ActionResult CreateType(string s)
	    {
		    return View(StorageType.GetAll());
		}

	    [HttpPost]
	    [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public ActionResult CreateType(StorageTypeCreation newType)
	    {
		    if (string.IsNullOrEmpty(newType.Name)) ModelState.AddModelError("Name", "Name is required");
            if (newType.Icon == null) ModelState.AddModelError("Icon", "Icon is required");
            if (!newType.IconIsValid()) ModelState.AddModelError("Icon", "Select a PNG image smaller than 1MB");
            if (ModelState.IsValid)
            {
                newType.Icon.SaveAs(Server.MapPath("~/Content/images/" + newType.Id+".png"));
                using (var db = new Context())
				{
					var old = db.StorageTypes.Find(newType.Id);
					if (old == null) db.StorageTypes.Add(new StorageType{ Background = newType.Background, Name = newType.Name});
					else
                    {
                        old.Name = newType.Name;
						old.Background = newType.Background;
					}

					db.SaveChanges();
				}
				return Redirect(Url.Action("CreateType"));
			}
		    return View(StorageType.GetAll());
		}

        public ActionResult RemoveType(int fromId, int toId)
        {
            var result = Redirect(Url.Action("Index"));
            using (var db = new Context())
            {
                var from = db.StorageTypes.Find(fromId);
                var to = db.StorageTypes.Find(toId);
                if (from == null || to == null || fromId == 1) return result;
                foreach (var product in db.Storages.Where(x => x.Type == fromId))
                {
                    product.Type = toId;
                }
                db.StorageTypes.Remove(from);
                db.SaveChanges();
            }
            return result;
        }

        #endregion
    }
}