﻿using System.ComponentModel.DataAnnotations;

namespace SmartKitchen.Domain.DisplayModel
{
    public class StorageDescription
    {
        [Key]
        public int Id { get; }
        public string Name { get; set; }
        public StorageTypeDisplayModel Type { get; set; }
        public int CellCount { get; set; }

    }
}