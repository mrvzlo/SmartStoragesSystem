using System;
using System.ComponentModel.DataAnnotations;

namespace SmartKitchen.Domain.CreationModels
{
    public class NameCreationModel
    {
        [Required]
        [DataType(DataType.Text)]
        [Display(Description = "Name")]
        [StringLength(64)]
        public string Name { get; set; }

        public NameCreationModel(string name) => Name = name;
        public NameCreationModel() => Name = string.Empty;
    }
}
