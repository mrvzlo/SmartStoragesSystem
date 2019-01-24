﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using AutoMapper;
using SmartKitchen.Models;

namespace SmartKitchen
{
	public class MvcApplication : System.Web.HttpApplication
	{
		protected void Application_Start()
		{
			Mapper.Initialize(cfg =>
			{
				cfg.CreateMap<Storage, StorageDescription>()
					.ForMember(dest => dest.Background, opts => opts.MapFrom(src => StorageDescription.GetBackgroundName(src.Background)))
					.ForMember(dest => dest.Icon, opts => opts.MapFrom(src => StorageDescription.GetIconName(src.Icon)));
			});
			AreaRegistration.RegisterAllAreas();
			FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
			RouteConfig.RegisterRoutes(RouteTable.Routes);
			BundleConfig.RegisterBundles(BundleTable.Bundles);
		}
	}
}
