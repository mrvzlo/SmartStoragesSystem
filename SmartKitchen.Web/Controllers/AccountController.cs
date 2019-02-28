﻿using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using SmartKitchen.Domain.CreationModels;
using SmartKitchen.Domain.IServices;
using SmartKitchen.Enums;
using SmartKitchen.Web.Helpers;

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
        public JsonResult SignIn(SignInModel model)
        {
            if (ModelState.IsValid)
            {
                var response = _authenticationService.SignIn(model);
                if (response.Successful())
                {
                    CreateTicket(response.Email, response.Role);
                    return Json(new {success = true, url = Url.Action("Index", "Storage")});
                }
                AddModelStateErrors(response);

            }
            return Json(new { success = false, formHTML = this.RenderPartialViewToString("_SignIn",model) });
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
                    return Json(new { success = true, url = Url.Action("About", "Home") });
                }

                AddModelStateErrors(response);
            }

            return Json(new { success = false, formHTML = this.RenderPartialViewToString("_SignUp", model) });
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