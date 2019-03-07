using System.ComponentModel.DataAnnotations;

namespace SmartKitchen.Domain.CreationModels
{
	public class StorageTypeCreationModel
	{
        public int Id { get; set; }
        [Required]
        [DataType(DataType.Text)]
        [Display(Description = "Name")]
        [StringLength(64)]
        public string Name { get; set; }
        [Required]
        [Display(Description = "Color")]
        [StringLength(6, MinimumLength = 6)]
        public string Background { get; set; }

        public StorageTypeCreationModel()
        {
            Background = "FFFFFF";
            Id = 0;
        }
    }
}