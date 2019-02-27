using System.Linq;
using SmartKitchen.Domain.Responses;
using System.Web.Mvc;

namespace SmartKitchen.Controllers
{
    public class BaseController : Controller
    {
        protected void AddModelStateErrors(ServiceResponse response)
        {
            if (response.Successful()) return;
            foreach (var error in response.Errors)
                ModelState.AddModelError(error.Key, error.ErrorEnum.ToString());
        }

        protected string GetErrorsToString(ServiceResponse response) => 
            response.Errors.Any() ? string.Join("<br/>", response.Errors) : "";

        protected string CurrentUser() => HttpContext.User.Identity.Name;
    }
}