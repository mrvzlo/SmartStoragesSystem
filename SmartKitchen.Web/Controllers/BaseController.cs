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
    }
}