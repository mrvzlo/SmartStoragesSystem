using System.Linq;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Security;
using Microsoft.AspNet.Identity;
using SmartKitchen.Models;

namespace RPS.Controllers
{
	public class AccountController : Controller
	{
		public ActionResult Login()
		{
			return View();
		}

		//
		// POST: /Account/Login
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Login(LoginModel model)
		{
			if (ModelState.IsValid)
			{
				User p = null;
				using (var db = new Context())
				{
					p = db.Users.FirstOrDefault(x => x.Name == model.Name);
					if (p != null && !Crypto.VerifyHashedPassword(p.Password, model.Password)) p = null;
				}

				if (p != null)
				{
					FormsAuthentication.SetAuthCookie(model.Name, true);
					return RedirectToAction("Index", "Home");
				}
				else
				{
					ModelState.AddModelError("", "User not found");
				}
			}
			return View(model);
		}

		public ActionResult Register()
		{
			return View();
		}

		//
		// POST: /Account/Register
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Register(RegisterModel model)
		{
			if (model.Password != model.Confirm) ModelState.AddModelError("Confirm", "Passwords don't match");
			if (model.Password.Length < 8) ModelState.AddModelError("Password", "Atleast 8 symbols");
			if (!model.Email.Contains('@') && !model.Email.Contains('.')) ModelState.AddModelError("Email", "Enter valid email");
			if (ModelState.IsValid)
			{
				User p = null;
				using (var db = new Context())
				{
					p = db.Users.FirstOrDefault(x => x.Name == model.Name);
				}

				if (p == null)
				{
					using (var db = new Context())
					{
						db.Users.Add(new User
						{
							Name = model.Name,
							Password = Crypto.HashPassword(model.Password),
							Email = model.Email
						});
						db.SaveChanges();
						p = db.Users.FirstOrDefault(x => x.Name == model.Name);
					}

					if (p != null)
					{
						FormsAuthentication.SetAuthCookie(model.Name, true);
						return RedirectToAction("Index", "Home");
					}
				}
				else
				{
					ModelState.AddModelError("", "This name is taken");
				}
			}

			// If we got this far, something failed, redisplay form
			return View(model);
		}

		//
		// POST: /Account/LogOff
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult LogOff()
		{
			FormsAuthentication.SignOut();
			return RedirectToAction("Index", "Home");
		}
	}
}