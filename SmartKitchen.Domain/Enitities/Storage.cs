using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SmartKitchen.Domain.Enitities
{
	public class Storage
	{
		[Key]
		public virtual int Id { get; set; }
		public virtual int Owner { get; set; }
		public virtual string Name { get; set; }
		public virtual int TypeId { get; set; }

        public virtual ICollection<Cell> Cells { get; set; }
        public virtual StorageType Type { get; set; }
        public virtual Person Person { get; set; }
    }
}