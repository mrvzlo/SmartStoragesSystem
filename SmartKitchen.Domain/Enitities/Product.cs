﻿using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace SmartKitchen.Domain.Enitities
{
	public class Product
	{
		[Key]
		public virtual int Id { get; set; }
		public virtual string Name { get; set; }
		public virtual int CategoryId { get; set; }
        public virtual Category Category { get; set; }
    }
}