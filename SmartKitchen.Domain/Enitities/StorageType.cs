using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace SmartKitchen.Domain.Enitities
{
	public class StorageType
	{
		[Key]
		public virtual int Id { get; set; }
		public virtual string Name { get; set; }
		public virtual string Background{ get; set; }

		public static List<StorageType> GetAll()
		{
			using (var db = new Context())
			{
				return db.StorageTypes.OrderBy(x=>x.Name).ToList();
			}
		}
	}
}