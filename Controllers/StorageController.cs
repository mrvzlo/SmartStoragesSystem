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
			        var type = db.StorageTypes.Find(storage.Type);
					storages.Add(new StorageDescription(storage,type));
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
				    db.SaveChanges();
			    }
		    }
		    return Redirect(Url.Action("Index"));
	    }

	    public ActionResult Edit(int id)
	    {
		    throw new NotImplementedException();
		}

	    public ActionResult Create()
	    {
		    return View(StorageType.GetAll());
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
	    public ActionResult Create(StorageDescription storage)
	    {
			if (string.IsNullOrEmpty(storage.Name)) ModelState.AddModelError("Name","Name is required");
		    StorageType type;
		    Person person;
			using (var db = new Context())
			{
				person = Person.Current(db);
				type = db.StorageTypes.FirstOrDefault(x => x.Name == storage.TypeName);
				if (db.Storages.FirstOrDefault(x => x.Owner == person.Id && x.Type == type.Id) != null) ModelState.AddModelError("TypeName", "This storage already exists");
				if (db.Storages.FirstOrDefault(x => x.Owner == person.Id && x.Name == storage.Name) != null) ModelState.AddModelError("Name", "This name is already taken");
				if (type==null) ModelState.AddModelError("TypeName", "Unknown type");
			}
			if (ModelState.IsValid)
		    {
				using (var db = new Context())
				{
					db.Storages.Add(new Storage{Name = storage.Name, Owner = person.Id, Type = type.Id});
					db.SaveChanges();
					return Redirect(Url.Action("Index"));
				}
			}
		    return View(StorageType.GetAll());
	    }

	    public ActionResult CreateType(string s)
	    {
		    return View(StorageType.GetAll());
		}

	    [HttpPost]
	    [ValidateAntiForgeryToken]
		public ActionResult CreateType(StorageType newType)
	    {
		    if (string.IsNullOrEmpty(newType.Name)) ModelState.AddModelError("Name", "Name is required");
		    if (string.IsNullOrEmpty(newType.Icon)) ModelState.AddModelError("Icon", "Icon is required");
		    newType.Icon = "fa fa-" + newType.Icon;
		    using (var db = new Context())
		    {
			    if (!Person.Current(db).IsAdmin()) return Redirect(Url.Action("Index"));
		    }

		    if (ModelState.IsValid)
			{
				using (var db = new Context())
				{
					var old = db.StorageTypes.FirstOrDefault(x => x.Name == newType.Name);
					if (old == null) db.StorageTypes.Add(newType);
					else
					{
						old.Background = newType.Background;
						old.Icon = newType.Icon;
					}

					db.SaveChanges();
				}
				return Redirect(Url.Action("CreateType"));
			}
		    return View(StorageType.GetAll());
		}
	}
}