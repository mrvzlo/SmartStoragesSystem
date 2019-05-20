using SmartKitchen.Domain.Extensions;
using SmartKitchen.Domain.Responses;
using System.Web.Mvc;

namespace SmartKitchen.Web.Controllers
{
    public class BaseController : Controller
    {
        protected readonly log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Copy errors from response to ModelState
        /// </summary>
        /// <param name="response"></param>
        protected void AddModelStateErrors(ServiceResponse response)
        {
            if (response.Successful()) return;
            foreach (var error in response.Errors)
                ModelState.AddModelError(error.Key, error.ErrorEnum.GetDescription());
        }

        /// <summary>
        /// Convert response errors to one string
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        protected string GetErrorsToString(ServiceResponse response)
        {
            var res = "";
            foreach (var e in response.Errors)
                res += e.ErrorEnum.GetDescription() + "\n";
            return res;
        }

        /// <summary>
        /// Get current user email
        /// </summary>
        /// <returns></returns>
        protected string CurrentUser() => HttpContext.User.Identity.Name;

    }
}