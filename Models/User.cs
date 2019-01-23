using System.ComponentModel.DataAnnotations;

namespace SmartKitchen.Models
{
	public class User
	{
		[Key]
		public virtual int Id { get; set; }
		public virtual string Name { get; set; }
		public virtual string Email { get; set; }
		public virtual string Password { get; set; }
	}
}