using System.ComponentModel.DataAnnotations;
using SmartKitchen.Enums;

namespace SmartKitchen.Models
{
	public class Storage
	{
		[Key]
		public virtual int Id { get; set; }
		public virtual int Owner { get; set; }
		public virtual string Name { get; set; }
		public virtual int Type { get; set; }
	}
}