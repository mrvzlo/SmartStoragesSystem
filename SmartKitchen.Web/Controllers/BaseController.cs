using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SmartKitchen.Domain.Responses;

namespace SmartKitchen.Controllers
{
    public class BaseController : Controller
    {
        // GET: Base
        protected void AddModelStateErrors(ServiceResponse response)
        {
            if (response.IsSuccessful || !response.Errors.Any()) return;
            foreach (var error in response.Errors)
                ModelState.AddModelError(error.Key, error.ErrorEnum.ToString());
        }
    }
}