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
	public class AccountController : BaseController
    {
        private readonly IAuthenticationService _authenticationService;

        public AccountController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
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
		public ActionResult SignIn(SignInModel model)
        {
            var response = _authenticationService.SignIn(model);
            if (!response.IsSuccessful)
            {
                AddModelStateErrors(response);
                return PartialView("_SignIn", model);
            }
            CreateTicket(response.Person);
            return Redirect(Url.Action("Index", "Storage"));
        }
		
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Register(SignUpModel model)
        {
            var response = _authenticationService.SignUp(model);
            if (!response.IsSuccessful)
            {
                AddModelStateErrors(response);
                return PartialView("_SignUp", model);
            }
            CreateTicket(response.Person);
            return RedirectToAction("About", "Home");
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