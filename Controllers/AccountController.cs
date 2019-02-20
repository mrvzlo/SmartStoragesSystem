using System;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Security;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using SmartKitchen.Enums;
using SmartKitchen.Models;

namespace SmartKitchen.Controllers
{
	public class AccountController : Controller
	{
		public ActionResult Index(bool login = true)
		{
            if (User.Identity.IsAuthenticated)
                return Redirect(Url.Action("Index","Home"));
            return View(new AuthModel{Login = login});
		}
		
		//
		// POST: /Account/Login
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Login(AuthModel model)
		{
			if (string.IsNullOrEmpty(model.EmailIn)) ModelState.AddModelError("EmailIn", "Email is required");
			if (string.IsNullOrEmpty(model.PasswordIn)) ModelState.AddModelError("PasswordIn", "Password is required");
			if (ModelState.IsValid)
			{
				Person p;
				using (var db = new Context())
				{
					p = db.People.FirstOrDefault(x => x.Email == model.EmailIn);
				}

				if (p == null)
				{
					ModelState.AddModelError("EmailIn", "User not found");
				}
				else if (!Crypto.VerifyHashedPassword(p.Password, model.PasswordIn))
				{
					ModelState.AddModelError("PasswordIn", "Email or password is incorrect");
				}
				else
				{
					CreateTicket(p);
					return RedirectToAction("Index", "Storage");
				}
			}

			model.Login = true;
			return View("Index",model);
		}
		
		//
		// POST: /Account/Register
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Register(AuthModel model)
		{
			if (string.IsNullOrEmpty(model.EmailUp)) ModelState.AddModelError("EmailUp", "Email is required");
			else if (!model.EmailUp.Contains('@') && !model.EmailUp.Contains('.')) ModelState.AddModelError("EmailUp", "Enter valid email");
			if (string.IsNullOrEmpty(model.NameUp)) ModelState.AddModelError("NameUp", "Name is required");
			if (string.IsNullOrEmpty(model.PasswordUp)) ModelState.AddModelError("PasswordUp", "Password is required");
			else if (model.PasswordUp.Length < 8) ModelState.AddModelError("PasswordUp", "Atleast 8 symbols");
			if (string.IsNullOrEmpty(model.ConfirmUp)) ModelState.AddModelError("ConfirmUp", "Password confirm is required");
			if (ModelState.IsValid && model.PasswordUp != model.ConfirmUp) ModelState.AddModelError("ConfirmUp", "Passwords don't match");
			if (ModelState.IsValid)
			{
				Person p;
				using (var db = new Context())
				{
					p = db.People.FirstOrDefault(x => x.Name == model.NameUp);
				}

				if (p == null)
				{
					using (var db = new Context())
					{
						db.People.Add(new Person
						{
							Name = model.NameUp,
							Password = Crypto.HashPassword(model.PasswordUp),
							Email = model.EmailUp
						});
						db.SaveChanges();
						p = db.People.FirstOrDefault(x => x.Name == model.NameUp);
                        var type1 = db.StorageTypes.First();
                        db.Storages.Add(new Storage {Name = type1.Name, Owner = p.Id, Type = type1.Id});
                        db.SaveChanges();
                    }

					if (p != null)
                    {
                        CreateTicket(p);
						return RedirectToAction("About", "Home");
					}
				}
				else
				{
					ModelState.AddModelError("", "This name is taken");
				}
			}

			model.Login = false;
			return View("Index", model);
		}

		private void CreateTicket(Person user)
		{
			var ticket = new FormsAuthenticationTicket(
				version: 1,
				name: user.Email,
				issueDate: DateTime.Now,
				expiration: DateTime.Now.AddDays(14),
				isPersistent: false,
				userData: RoleIntToString(user.Role));

			var encryptedTicket = FormsAuthentication.Encrypt(ticket);
			var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
			HttpContext.Response.Cookies.Add(cookie);
		}

		public string RoleIntToString(Role role)
		{
			switch (role)
			{
				case Role.Admin: return "admin";
				case Role.Simple: return "simple";
				default: return "unknown";
			}
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