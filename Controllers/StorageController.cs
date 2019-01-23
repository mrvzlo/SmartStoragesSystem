using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SmartKitchen.Models;

namespace SmartKitchen.Controllers
{
    public class StorageController : Controller
    {
        public ActionResult Index()
        {
			List<Storage> storages = new List<Storage>();
	        using (var db = new Context())
	        {
		        User u = db.Users.FirstOrDefault(x => x.Email == HttpContext.User.Identity.Name);
		        if (u == null) return Redirect(Url.Action("Index", "Home"));
		        storages = db.Storages.Where(x => x.UserId == u.Id).ToList();
	        }
            return View(storages);
        }
    }
}