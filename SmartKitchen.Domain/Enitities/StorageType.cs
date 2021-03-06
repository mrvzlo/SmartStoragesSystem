﻿using System.ComponentModel.DataAnnotations;

namespace SmartKitchen.Domain.Enitities
{
    public class StorageType
    {
        [Key]
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual string Background { get; set; }
    }
}