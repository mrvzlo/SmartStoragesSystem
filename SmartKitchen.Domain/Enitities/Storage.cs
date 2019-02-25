using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using SmartKitchen.Models;

namespace SmartKitchen.Domain.Enitities
{
	public class Storage
	{
		[Key]
		public virtual int Id { get; set; }
		public virtual int Owner { get; set; }
		public virtual string Name { get; set; }
		public virtual int Type { get; set; }

        public static List<Storage> GetMyStorages(int person, Context db) => db.Storages.Where(x => x.Owner == person).ToList();

        public static Response IsOwner(Storage s, Person p)
        {
            if (s == null || p == null) return new Response(404);
            if (s.Owner != p.Id) return new Response(403);
            return Response.Success();
        }
    }
}