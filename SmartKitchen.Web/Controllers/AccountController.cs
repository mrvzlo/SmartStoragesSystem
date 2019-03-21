// ReSharper disable PossibleMultipleEnumeration
using SmartKitchen.Domain.CreationModels;
using SmartKitchen.Domain.Enums;
using SmartKitchen.Domain.Extensions;
using SmartKitchen.Domain.IServices;
using SmartKitchen.Web.Helpers;
using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

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
        public ActionResult Info()
        {
            var person = _personService.GetPersonByEmail(CurrentUser());
            ViewBag.PersonId = person.Id;
            ViewBag.PublicKey = person.PublicKey;
            var currencies = EnumHelper.GetAllCurrencies();
            var weights = EnumHelper.GetAllWeights();
            var currency = CookieHelper.GetCookie(HttpContext, Cookie.Currency).Value;
            var weight = CookieHelper.GetCookie(HttpContext, Cookie.Weight).Value;
            ViewBag.Currency = (int)currencies.SingleOrDefault(x => x.GetDescription() == currency);
            ViewBag.Weight = (int)weights.SingleOrDefault(x => x.GetDescription() == weight);
            ViewBag.CurrencyList = currencies.Select(x => new SelectListItem { Value = ((int)x).ToString(), Text = x.ToString() });
            ViewBag.WeightList = weights.Select(x => new SelectListItem { Value = ((int)x).ToString(), Text = x.ToString() });
            return View();
        }

        public RedirectResult UpdateCookies(int currency, int weight)
        {
            if (Enum.IsDefined(typeof(Currency), currency))
                CookieHelper.UpdateCookie(HttpContext, Cookie.Currency, (Currency)currency);

            if (Enum.IsDefined(typeof(Weight), weight))
                CookieHelper.UpdateCookie(HttpContext, Cookie.Weight, (Weight)weight);

            return Redirect(Url.Action("Info"));
        }

        [Authorize]
        [HttpPost]
        public string UpdateKeyPair()
        {
            _personService.UpdateKeyPair(CurrentUser());
            return _personService.GetPersonByEmail(CurrentUser()).PublicKey;
        }

        public PartialViewResult SignIn() =>
            PartialView("_SignIn", new SignInModel());

        public PartialViewResult SignUp() =>
            PartialView("_SignUp", new SignUpModel());

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
            var ticket = new FormsAuthenticationTicket(1, email, DateTime.Now, DateTime.Now.AddMonths(1), false, role.GetDescription());
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