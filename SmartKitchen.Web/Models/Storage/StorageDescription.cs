using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using SmartKitchen.Domain.Enitities;

namespace SmartKitchen.Models
{
    public class StorageDescription
    {
        [Key]
        public int Id { get; }
        public string Name { get; set; }
        public StorageType Type { get; set; }
        public int CellCount { get; set; }

        public StorageDescription()
        {
            Type = new StorageType();
        }

        public StorageDescription(Storage storage, Context db)
        {
            Id = storage.Id;
            Name = storage.Name;
            Type = db.StorageTypes.Find(storage.Type);
            CellCount = db.Cells.Count(x => x.Storage == storage.Id);
        }
    }
}