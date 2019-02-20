using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Web;
using SmartKitchen.Enums;

namespace SmartKitchen.Models
{
    public class BasketProductCreation
    {
        public int Basket { get; set; }
        public string Name { get; set; }
    }
}