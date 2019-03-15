using System;
using SmartKitchen.Domain.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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
        public virtual Guid Token { get; set; }

        public virtual ICollection<Basket> Baskets { get; set; }
        public virtual ICollection<Storage> Storages { get; set; }
    }
}