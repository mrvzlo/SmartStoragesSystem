using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Web;
using SmartKitchen.Enums;

namespace SmartKitchen.Models
{
	public class StorageTypeCreationModel
	{
        public int Id { get; set; }
        public string Name { get; set; }
        public string Background { get; set; }

    }
}