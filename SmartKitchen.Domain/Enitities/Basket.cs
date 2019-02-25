using System;
using System.ComponentModel.DataAnnotations;

namespace SmartKitchen.Domain.Enitities
{
    public class Basket
    {
		[Key]
        public virtual int Id { get; set; }
        public virtual int Owner { get; set; }
        public virtual bool Closed { get; set; }
        public virtual string Name { get; set; }
        public virtual DateTime CreationDate { get; set; }
    }
}