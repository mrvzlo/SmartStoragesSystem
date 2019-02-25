using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace SmartKitchen.Domain.Enitities
{
	public class Product
	{
		[Key]
		public virtual int Id { get; set; }
		public virtual string Name { get; set; }
		public virtual int Category { get; set; }

        public static Product GetByName(string name, Context db) => db.Products.FirstOrDefault(x => x.Name.Equals(name.Trim(), StringComparison.OrdinalIgnoreCase));
	}
}