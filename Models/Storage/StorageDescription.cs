using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace SmartKitchen.Models
{
    public class StorageDescription
    {
        [Key]
        public int Id { get; }
        public string Name { get; set; }
        public StorageType Type { get; set; }
        public List<int> Cells { get; set; }

        public StorageDescription()
        {
            Type = new StorageType();
            Cells = new List<int>();
        }

        public StorageDescription(Storage storage, Context db, int order)
        {
            Id = storage.Id;
            Name = storage.Name;
            Type = db.StorageTypes.Find(storage.Type);
            List<CellDescription> cells = new List<CellDescription>();
            foreach (var cell in db.Cells.Where(x => x.Storage == storage.Id).Select(x => x.Id).ToList())
                cells.Add(new CellDescription(db, cell));
            Cells = GetCellOrder(order, cells);

        }

        private List<int> GetCellOrder(int order, List<CellDescription> cells)
        {
            switch (order)
            {
                default:
                    return cells.OrderBy(x => x.Product.Name).Select(x => x.Cell.Id).ToList();
                case -1:
                    return cells.OrderByDescending(x => x.Product.Name).Select(x => x.Cell.Id).ToList();
                case 2:
                    return cells.OrderBy(x => x.Category.Name).Select(x => x.Cell.Id).ToList();
                case -2:
                    return cells.OrderByDescending(x => x.Category.Name).Select(x => x.Cell.Id).ToList();
                case 3:
                    return cells.OrderBy(x => x.Cell.BestBefore).Select(x => x.Cell.Id).ToList();
                case -3:
                    return cells.OrderByDescending(x => x.Cell.BestBefore).Select(x => x.Cell.Id).ToList();
                case 4:
                    return cells.OrderBy(x => x.Cell.Amount).Select(x => x.Cell.Id).ToList();
                case -4:
                    return cells.OrderByDescending(x => x.Cell.Amount).Select(x => x.Cell.Id).ToList();
            }
        }
    }
}