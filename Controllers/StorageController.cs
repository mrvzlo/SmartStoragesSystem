using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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
		        var u = db.Users.FirstOrDefault(x => x.Email == HttpContext.User.Identity.Name);
		        if (u == null) return Redirect(Url.Action("Index", "Home"));
		        var s = db.Storages.Where(x => x.UserId == u.Id).ToList();
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
	    public ActionResult Create(StorageDescription storage)
	    {
			if (string.IsNullOrEmpty(storage.Name)) ModelState.AddModelError("Name","Name is required");
		    StorageType type;
		    User user;
			using (var db = new Context())
			{
				user = db.Users.FirstOrDefault(x => x.Email == User.Identity.Name);
				type = db.StorageTypes.FirstOrDefault(x => x.Name == storage.TypeName);
				if (db.Storages.FirstOrDefault(x => x.UserId == user.Id && x.Type == type.Id) != null) ModelState.AddModelError("TypeName", "This storage already exists");
				if (db.Storages.FirstOrDefault(x => x.UserId == user.Id && x.Name == storage.Name) != null) ModelState.AddModelError("Name", "This name is already exists");
				if (type==null) ModelState.AddModelError("TypeName", "Unknown type");
			}
			if (ModelState.IsValid)
		    {
				using (var db = new Context())
				{
					db.Storages.Add(new Storage{Name = storage.Name, UserId = user.Id, Type = type.Id});
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
		public ActionResult CreateType(StorageType type)
	    {
		    type.Icon = "fa fa-"+type.Icon;
		    using (var db = new Context())
		    {
			    db.StorageTypes.Add(type);
			    db.SaveChanges();
		    }
		    return Redirect(Url.Action("Create"));
	    }
	}
}