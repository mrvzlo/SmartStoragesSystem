using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SmartKitchen.Enums;
using SmartKitchen.Models;

namespace SmartKitchen.Middleware
{
	public class Helper
	{
		private static bool IsAdmin(User u)
		{
			if (u == null) return false;
			return u.Role == Role.Admin;
		}
	}
}