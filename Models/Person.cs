using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using SmartKitchen.Enums;

namespace SmartKitchen.Models
{
	public sealed class Person
	{
		[Key]
		public int Id { get; set; }
		public Role Role { get; set; }
		public string Name { get; set; }
		public string Email { get; set; }
		public string Password { get; set; }
		
		public static Person Current(Context db) => db.People.FirstOrDefault(x => x.Email == HttpContext.Current.User.Identity.Name);

		public bool IsAdmin() => Role == Role.Admin;
	}
}