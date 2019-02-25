using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Security;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using SmartKitchen.Domain.DisplayModel;
using SmartKitchen.Domain.Enitities;
using SmartKitchen.Domain.IService;
using SmartKitchen.Enums;
using SmartKitchen.Models;

namespace SmartKitchen.Controllers
{
	public class AccountController : Controller
    {
        private readonly IPersonService _personService;

        public AccountController(IPersonService personService)
        {
            _personService = personService;
        }

        public ActionResult Index(bool login = true)
		{
            if (User.Identity.IsAuthenticated)
                return Redirect(Url.Action("Index","Home"));
            return View();
        }

        public PartialViewResult SignIn()
        {
            return PartialView("_SignIn", new SignInModel());
        }

        public PartialViewResult SignUp()
        {
            return PartialView("_SignUp", new SignUpModel());
        }

        [HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Login(SignInModel model)
		{
			if (string.IsNullOrEmpty(model.Email)) ModelState.AddModelError("EmailIn", "Email is required");
			if (string.IsNullOrEmpty(model.Password)) ModelState.AddModelError("PasswordIn", "Password is required");
			if (ModelState.IsValid)
            {
                var person = _personService.GetPersonByEmail(model.Email);

				if (person == null)
				{
					ModelState.AddModelError("EmailIn", "User not found");
				}
				else if (!Crypto.VerifyHashedPassword(person.Password, model.Password))
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

		private void CreateTicket(Person person)
        {
            var ticket = new FormsAuthenticationTicket(
                version: 1,
                name: person.Email,
                issueDate: DateTime.Now,
                expiration: DateTime.Now.AddDays(14),
                isPersistent: false,
                userData: person.Role.GetType()
                    .GetMember(person.Role.ToString())
                    .FirstOrDefault()?
                    .GetCustomAttribute<DescriptionAttribute>()?
                    .Description
            );
			var encryptedTicket = FormsAuthentication.Encrypt(ticket);
			var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
			HttpContext.Response.Cookies.Add(cookie);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult LogOff()
		{
			FormsAuthentication.SignOut();
			return RedirectToAction("Index", "Home");
		}
	}
}