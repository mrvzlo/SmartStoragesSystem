using System.ComponentModel.DataAnnotations;

namespace SmartKitchen.Domain.CreationModels
{
    public class CellCreationModel
    {
        [Required]
        [DataType(DataType.Text)]
        [Display(Description = "Product name")]
        [StringLength(128)]
        public string Product { get; set; }
        public int Storage { get; set; }
    }
}
