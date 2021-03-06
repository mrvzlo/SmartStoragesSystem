﻿using SmartKitchen.Domain.Extensions;
using SmartKitchen.Domain.Responses;
using System.Web.Mvc;

namespace SmartKitchen.Web.Controllers
{
    public class BaseController : Controller
    {
        protected readonly log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        protected void AddModelStateErrors(ServiceResponse response)
        {
            if (response.Successful()) return;
            foreach (var error in response.Errors)
                ModelState.AddModelError(error.Key, error.ErrorEnum.GetDescription());
        }

        protected string GetErrorsToString(ServiceResponse response)
        {
            var res = "";
            foreach (var e in response.Errors)
                res += e.ErrorEnum.GetDescription() + "\n";
            return res;
        }

        protected string CurrentUser() => HttpContext.User.Identity.Name;

    }
}