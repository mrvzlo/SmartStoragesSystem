using System.Linq;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Security;
using Microsoft.AspNet.Identity;
using SmartKitchen.Models;

namespace SmartKitchen.Controllers
{
	public class AccountController : Controller
	{
		public ActionResult Index()
		{
			return View(new AuthModel{Login = false});
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
				User p;
				using (var db = new Context())
				{
					p = db.Users.FirstOrDefault(x => x.Email == model.EmailIn);
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
					FormsAuthentication.SetAuthCookie(model.EmailIn, true);
					return RedirectToAction("Index", "Home");
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
				User p;
				using (var db = new Context())
				{
					p = db.Users.FirstOrDefault(x => x.Name == model.NameUp);
				}

				if (p == null)
				{
					using (var db = new Context())
					{
						db.Users.Add(new User
						{
							Name = model.NameUp,
							Password = Crypto.HashPassword(model.PasswordUp),
							Email = model.EmailUp
						});
						db.SaveChanges();
						p = db.Users.FirstOrDefault(x => x.Name == model.NameUp);
					}

					if (p != null)
					{
						FormsAuthentication.SetAuthCookie(model.NameUp, true);
						return RedirectToAction("Index", "Home");
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