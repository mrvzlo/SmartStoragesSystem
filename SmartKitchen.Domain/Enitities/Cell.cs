using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SmartKitchen.Domain.Enitities
{
    public class Cell
    {
        [Key]
        public virtual int Id { get; set; }
        public virtual int ProductId { get; set; }
        public virtual int StorageId { get; set; }
        public virtual DateTime? BestBefore { get; set; }

        public virtual Product Product { get; set; }
        public virtual Storage Storage { get; set; }
        public virtual ICollection<BasketProduct> BasketProducts { get; set; }
        public virtual ICollection<CellChange> CellChanges { get; set; }
    }
}