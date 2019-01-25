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
		    List<StorageType> list = new List<StorageType>();
		    using (var db = new Context())
		    {
			    list = db.StorageTypes.ToList();
		    }
		    return View(list);
		}

		[HttpPost]
	    public ActionResult Create(Storage storage)
	    {
		    List<StorageType> list = new List<StorageType>();
		    using (var db = new Context())
		    {
			    list = db.StorageTypes.ToList();
		    }
		    return View(list);
	    }

	    public ActionResult CreateType(string s)
	    {
		    return View();
		}

	    [HttpPost]
		public ActionResult CreateType(StorageType type)
	    {
		    using (var db = new Context())
		    {
			    db.StorageTypes.Add(type);
			    db.SaveChanges();
		    }
		    return Redirect(Url.Action("Create"));
	    }
	}
}