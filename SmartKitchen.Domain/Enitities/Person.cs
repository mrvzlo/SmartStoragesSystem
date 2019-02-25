using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using SmartKitchen.Enums;
using SmartKitchen.Models;

namespace SmartKitchen.Domain.Enitities
{
	public class Person
	{
		[Key]
		public virtual int Id { get; set; }
		public virtual Role Role { get; set; }
		public virtual string Name { get; set; }
		public virtual string Email { get; set; }
		public virtual string Password { get; set; }
		
		public static Person Current(Context db) => db.People.FirstOrDefault(x => x.Email == HttpContext.Current.User.Identity.Name);
	}
}