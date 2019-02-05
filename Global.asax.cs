using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using AutoMapper;
using SmartKitchen.Enums;
using SmartKitchen.Models;

namespace SmartKitchen
{
	public class MvcApplication : System.Web.HttpApplication
	{
		protected void Application_Start()
		{
			Mapper.Initialize(cfg =>
			{
			});
			AreaRegistration.RegisterAllAreas();
			FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
			RouteConfig.RegisterRoutes(RouteTable.Routes);
			BundleConfig.RegisterBundles(BundleTable.Bundles);
		}

		public void Application_AuthenticateRequest(Object src, EventArgs e)
		{
			if (HttpContext.Current.User == null) return;
   			if (!HttpContext.Current.User.Identity.IsAuthenticated) return;
			var cookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
			var decodedTicket = FormsAuthentication.Decrypt(cookie.Value);
			var roles = decodedTicket.UserData;
			var principal = new GenericPrincipal(HttpContext.Current.User.Identity, new[] { roles });
			HttpContext.Current.User = principal;
		}
	}
}
