using System;
using System.ComponentModel.DataAnnotations;

namespace SmartKitchen.Domain.Enitities
{
    public class CellChange
    {
        [Key]
        public virtual int Id { get; set; }
        public virtual int CellId { get; set; }
        public virtual decimal Amount { get; set; }
        public virtual DateTime UpdateDate { get; set; }

        public virtual Cell Cell { get; set; }
    }
}