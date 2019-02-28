using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using SmartKitchen.Domain.CreationModels;
using SmartKitchen.Domain.IServices;
using SmartKitchen.Enums;

namespace SmartKitchen.Web.Controllers
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
                return Redirect(Url.Action("Index", "Home"));
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
            if (!ModelState.IsValid)
                return PartialView("_SignIn", model);
            var response = _authenticationService.SignIn(model);
            if (!response.Successful())
            {
                AddModelStateErrors(response);
                return PartialView("_SignIn", model);
            }
            CreateTicket(response.Email, response.Role);
            return Redirect(Url.Action("Index", "Storage"));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(SignUpModel model)
        {
            if (!ModelState.IsValid)
                return PartialView("_SignUp", model);
            var response = _authenticationService.SignUp(model);
            if (!response.Successful())
            {
                AddModelStateErrors(response);
                return PartialView("_SignUp", model);
            }
            CreateTicket(response.Email, response.Role);
            return RedirectToAction("About", "Home");
        }

        private void CreateTicket(string email, Role role)
        {
            var ticket = new FormsAuthenticationTicket(
                version: 1,
                name: email,
                issueDate: DateTime.Now,
                expiration: DateTime.Now.AddDays(14),
                isPersistent: false,
                userData: role.GetType()
                    .GetMember(role.ToString())
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