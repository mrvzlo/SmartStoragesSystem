using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using SmartKitchen.Enums;

namespace SmartKitchen.Models
{
	public class Category
	{
		[Key]
		public virtual int Id { get; set; }
		public virtual string Name { get; set; }

		public Category()
		{
			Id = 0;
			Name = "None";
		}
	}
}