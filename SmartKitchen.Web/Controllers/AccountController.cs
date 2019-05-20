﻿// ReSharper disable PossibleMultipleEnumeration
// ReSharper disable PossibleNullReferenceException
// ReSharper disable InvertIf
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

        /// <summary>
        /// Sign in and sign up page
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        public ActionResult Index(string returnUrl = null)
        {
            if (User.Identity.IsAuthenticated)
                return Redirect(Url.Action("Index", "Home"));
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        /// <summary>
        /// Public key partial page
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public PartialViewResult Key()
        {
            var person = _personService.GetPersonByEmail(CurrentUser());
            ViewBag.PersonId = person.Id;
            ViewBag.PublicKey = person.PublicKey;
            return PartialView("_Key");
        }

        /// <summary>
        /// Preferences partial page
        /// </summary>
        /// <returns></returns>
        public PartialViewResult Preferences()
        {
            var currencies = EnumHelper.GetAllCurrencies();
            var weights = EnumHelper.GetAllWeights();
            var currency = CookieHelper.GetCookie(HttpContext, Cookie.Currency).Value;
            var weight = CookieHelper.GetCookie(HttpContext, Cookie.Weight).Value;
            ViewBag.Currency = (int)currencies.SingleOrDefault(x => x.GetDescription() == currency);
            ViewBag.Weight = (int)weights.SingleOrDefault(x => x.GetDescription() == weight);
            ViewBag.CurrencyList = currencies.Select(x => new SelectListItem { Value = ((int)x).ToString(), Text = x.ToString() });
            ViewBag.WeightList = weights.Select(x => new SelectListItem { Value = ((int)x).ToString(), Text = x.ToString() });
            return PartialView("_Preferences");
        }

        /// <summary>
        /// Update currency and weight preferences and reload to settings page
        /// </summary>
        /// <param name="currency"></param>
        /// <param name="weight"></param>
        /// <returns></returns>
        public RedirectResult UpdatePreferences(int currency, int weight)
        {
            if (Enum.IsDefined(typeof(Currency), currency))
                CookieHelper.UpdateCookie(HttpContext, Cookie.Currency, (Currency)currency);

            if (Enum.IsDefined(typeof(Weight), weight))
                CookieHelper.UpdateCookie(HttpContext, Cookie.Weight, (Weight)weight);

            return Redirect(Url.Action("Settings", new { tab = 1 }));
        }

        /// <summary>
        /// Generate and return new key
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        public string UpdateKeyPair()
        {
            _personService.UpdateKeyPair(CurrentUser());
            return _personService.GetPersonByEmail(CurrentUser()).PublicKey;
        }

        /// <summary>
        /// Settings page
        /// </summary>
        /// <param name="tab"></param>
        /// <returns></returns>
        public ActionResult Settings(int tab = 0)
        {
            ViewBag.Active = tab;
            return View();
        }

        /// <summary>
        /// Sign in partial
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        public PartialViewResult SignIn(string returnUrl = null) => PartialView("_SignIn", new SignInModel { ReturnUrl = returnUrl });

        /// <summary>
        /// Sign up partial
        /// </summary>
        /// <returns></returns>
        public PartialViewResult SignUp() => PartialView("_SignUp", new SignUpModel());

        /// <summary>
        /// Password change partial
        /// </summary>
        /// <returns></returns>
        public PartialViewResult ChangePassword() => PartialView("_ChangePassword", new PasswordResetModel());
        
        /// <summary>
        /// If signed in redirects to returnUrl or storages else reload form
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
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
                    Log.Info("Successful login attempt for " + model.Username);
                    var url = Url.Action("Index", "Storage");
                    if (model.ReturnUrl != null && Url.IsLocalUrl(model.ReturnUrl)) url = "http://" + Request.Url.Authority + model.ReturnUrl;
                    else if (model.ReturnUrl != null) Log.Warn("Tried redirect to external " + model.ReturnUrl);
                    return Json(new { success = true, url });
                }
                AddModelStateErrors(response);
                Log.Info("Unsuccessful login attempt for " + model.Username);

            }
            return Json(new { success = false, formHTML = this.RenderPartialViewToString("_SignIn", model) });
        }

        /// <summary>
        /// If signed up redirects to guide else reload form
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Create auth ticket
        /// </summary>
        /// <param name="email"></param>
        /// <param name="role"></param>
        private void CreateTicket(string email, Role role)
        {
            var ticket = new FormsAuthenticationTicket(1, email, DateTime.Now, DateTime.Now.AddMonths(1), false, role.GetDescription());
            var encryptedTicket = FormsAuthentication.Encrypt(ticket);
            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
            HttpContext.Response.Cookies.Add(cookie);
        }

        /// <summary>
        /// Change password and reload settings page else reload form
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult ChangePassword(PasswordResetModel model)
        {
            model.EmailConfirm = CurrentUser();
            if (ModelState.IsValid)
            {
                var response = _authenticationService.ResetPassword(model);
                if (response.Successful())
                {
                    Log.Info("Successful password change for " + model.Email);
                    return Json(new { success = true, url = Url.Action("Settings", new{tab = 3}) });
                }

                AddModelStateErrors(response);
            }

            model.EmailConfirm = "";
            return Json(new { success = false, formHTML = this.RenderPartialViewToString("_ChangePassword", model) });
        }

        /// <summary>
        /// Sign out
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }
    }
}