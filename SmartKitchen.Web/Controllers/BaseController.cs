using System.IO;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.UI;
using JetBrains.Annotations;
using SmartKitchen.Domain.Extensions;
using SmartKitchen.Domain.Responses;

namespace SmartKitchen.Web.Controllers
{
    public class BaseController : Controller
    {
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
                res += e.ErrorEnum.GetDescription()+"\n";
            return res;
        }

        protected string CurrentUser() => HttpContext.User.Identity.Name;

    }
}