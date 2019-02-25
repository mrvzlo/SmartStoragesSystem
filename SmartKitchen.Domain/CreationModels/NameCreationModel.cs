using System.ComponentModel.DataAnnotations;

namespace SmartKitchen.Domain.CreationModels
{
    public class NameCreationModel
    {
        [Required]
        [DataType(DataType.Text)]
        [Display(Description = "Name")]
        [StringLength(64)]
        public string Value { get; set; }
    }
}
