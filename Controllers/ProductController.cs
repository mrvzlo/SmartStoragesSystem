using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using SmartKitchen.Models;

namespace SmartKitchen.Controllers
{
	[Authorize]
	public class ProductController : Controller
	{
		public ActionResult Create(ProductDesctiption product)
		{
			return Redirect(Url.Action("View", "Storage", new {product.Status.Storage}));
		}
	}
}