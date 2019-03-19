using System.ComponentModel.DataAnnotations;

namespace SmartKitchen.Domain.DisplayModels
{
    public class StorageDisplayModel
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public StorageTypeDisplayModel Type { get; set; }
        public int CellCount { get; set; }

        public int Expired { get; set; }
        public int Absent { get; set; }
    }
}