using System.ComponentModel.DataAnnotations;

namespace SmartKitchen.Domain.Enitities
{
	public class Category
	{
		[Key]
		public virtual int Id { get; set; }
		public virtual string Name { get; set; }
	}
}