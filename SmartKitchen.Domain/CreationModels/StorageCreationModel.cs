﻿using System.ComponentModel.DataAnnotations;

namespace SmartKitchen.Domain.CreationModels
{
    public class StorageCreationModel : NameCreationModel
    {
        public int TypeId { get; set; }
    }
}
