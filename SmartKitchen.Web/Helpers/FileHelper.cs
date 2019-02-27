using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Web;

namespace SmartKitchen.Helpers
{
    public static class FileHelper
    {
        public static bool IconIsNotValid(HttpPostedFile fileIcon)
        {
            if (fileIcon == null) return false;
            if (fileIcon.ContentLength > 1 * 1024 * 1024) return true;
            try
            {
                using (var img = Image.FromStream(fileIcon.InputStream))
                {
                    return !img.RawFormat.Equals(ImageFormat.Png);
                }
            }
            catch
            {
                return true;
            }
        }

        public static void SaveImage(HttpPostedFile fileIcon, string path)
        {
            fileIcon?.SaveAs(path);
        }
    }
}