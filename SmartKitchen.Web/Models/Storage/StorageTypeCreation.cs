using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Web;
using SmartKitchen.Enums;

namespace SmartKitchen.Models
{
	public class StorageTypeCreation
	{
        public int Id { get; set; }
        public string Name { get; set; }
        public string Background { get; set; }
        public HttpPostedFileBase Icon { get; set; }

        public bool IconIsValid()
        {
            if (Icon == null || Icon.ContentLength > 1 * 1024 * 1024) return false;
            try
            {
                using (var img = Image.FromStream(Icon.InputStream))
                {
                    return img.RawFormat.Equals(ImageFormat.Png);
                }
            }
            catch
            {
                return false;
            }
        }
    }
}