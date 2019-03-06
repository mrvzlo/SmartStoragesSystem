using SmartKitchen.Domain.CreationModels;
using SmartKitchen.Domain.Enums;
using SmartKitchen.Domain.IServices;
using SmartKitchen.Web.Helpers;
using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using NonFactors.Mvc.Grid;
using SmartKitchen.Domain.Extensions;

namespace SmartKitchen.Web.Controllers
{
    public class AccountController : BaseController
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IPersonService _personService;

        public AccountController(IAuthenticationService authenticationService, IPersonService personService)
        {
            _authenticationService = authenticationService;
            _personService = personService;
        }

        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
                return Redirect(Url.Action("Index", "Home"));
            return View();
        }

        [Authorize]
        public ActionResult Token()
        {
            var token = _personService.GetPersonByEmail(CurrentUser()).Token;
            return View(token);
        }

        [Authorize]
        [HttpPost]
        public string ResetToken()
        {
            _personService.UpdateToken(CurrentUser());
            return _personService.GetPersonByEmail(CurrentUser()).Token.ToString();
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
        public JsonResult SignIn(SignInModel model)
        {
            if (ModelState.IsValid)
            {
                var response = _authenticationService.SignIn(model);
                if (response.Successful())
                {
                    CreateTicket(response.Email, response.Role);
                    Log.Info("Successful login attempt for " + model.Email);
                    return Json(new { success = true, url = Url.Action("Index", "Storage") });
                }
                AddModelStateErrors(response);
                Log.Info("Unsuccessful login attempt for " + model.Email);

            }
            return Json(new { success = false, formHTML = this.RenderPartialViewToString("_SignIn", model) });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Register(SignUpModel model)
        {
            if (ModelState.IsValid)
            {
                var response = _authenticationService.SignUp(model);
                if (response.Successful())
                {
                    CreateTicket(response.Email, response.Role);
                    Log.Info("Successful registration for " + model.Email);
                    return Json(new { success = true, url = Url.Action("About", "Home") });
                }

                AddModelStateErrors(response);
            }

            return Json(new { success = false, formHTML = this.RenderPartialViewToString("_SignUp", model) });
        }

        private void CreateTicket(string email, Role role)
        {
            var ticket = new FormsAuthenticationTicket(
                1,
                email,
                DateTime.Now,
                DateTime.Now.AddDays(14),
                false,
                role.GetDescription()
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