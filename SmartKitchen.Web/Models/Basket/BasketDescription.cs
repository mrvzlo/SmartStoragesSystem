using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartKitchen.Models
{
    public class BasketDescription
    {
        public Basket Basket { get; set; }
        public List<int> Products { get; set; }
        public int BoughtProducts { get; set; }
    }
}