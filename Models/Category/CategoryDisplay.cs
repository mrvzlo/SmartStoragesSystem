using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using SmartKitchen.Enums;

namespace SmartKitchen.Models
{
	public class CategoryDisplay
    {
        public int Id { get; set; }
        public int ProductsCount { get; set; }
		public string Name { get; set; }

        public static List<int> GetIds()
        {
            using (var db = new Context())
            {
                return db.Categories.Select(x => x.Id).ToList();
            }
        }
	}
}