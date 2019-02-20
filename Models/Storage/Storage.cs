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

        public static Response IsOwner(Storage s, Person p)
        {
            if (s == null || p == null) return new Response(404);
            if (s.Owner != p.Id) return new Response(403);
            return Response.Success();
        }
    }
}