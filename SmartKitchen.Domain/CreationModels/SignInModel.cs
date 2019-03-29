using System.ComponentModel.DataAnnotations;

namespace SmartKitchen.Domain.CreationModels
{
	public class SignInModel
	{
        [Required]
        [DataType(DataType.Text)]
        [Display(Description = "Username")]
        [StringLength(128)]
		public string Username{ get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Description = "Password")]
        [StringLength(128)]
        public string Password{ get; set; }
        public string ReturnUrl { get; set; }
    }

}