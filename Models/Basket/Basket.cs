using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using SmartKitchen.Enums;

namespace SmartKitchen.Models
{
    public class Basket
    {
		[Key]
        public virtual int Id { get; set; }
        public virtual int Owner { get; set; }
        public virtual bool Closed { get; set; }
        public virtual bool Private { get; set; }
        public virtual string Name { get; set; }
        public virtual DateTime CreationDate { get; set; }

        public static Response IsOwner(Basket b, Person p)
        {
            if (b == null || p == null) return new Response(404);
            if (b.Owner != p.Id) return new Response(403);
            return Response.Success();
        }
    }
}